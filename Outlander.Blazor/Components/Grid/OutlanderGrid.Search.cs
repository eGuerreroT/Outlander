using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net;
using System.Text;

namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem>
{
    private static string NormalizeText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var ch in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    private void HandleSearchTextChanged(ChangeEventArgs e)
    {
        SearchBoxText = e.Value?.ToString() ?? string.Empty;
        CurrentPage = 1;
    }

    private IEnumerable<string> GetSearchTerms()
    {
        if (string.IsNullOrWhiteSpace(SearchBoxText))
            return Enumerable.Empty<string>();

        if (ResolvedSearchTextParseMode == GridSearchTextParseMode.ExactMatch)
            return new[] { SearchBoxText.Trim() };

        return SearchBoxText
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private string GetSearchableText(TItem item, OutlanderGridColumnDefinition<TItem> column)
    {
        if (column.FilterTextSelector is not null)
            return column.FilterTextSelector(item) ?? string.Empty;

        if (column.SortTextSelector is not null)
            return column.SortTextSelector(item) ?? string.Empty;

        return GetPropertyValue(item, column.FieldName)?.ToString() ?? string.Empty;
    }

    private IEnumerable<OutlanderGridColumnDefinition<TItem>> GetSearchableColumns()
    {
        return VisibleColumnsDefinition.Where(c =>
            c.SearchEnabled &&
            !string.IsNullOrWhiteSpace(c.FieldName) &&
            (!c.FieldName.StartsWith("_") || c.FilterTextSelector is not null || c.SortTextSelector is not null));
    }

    private IEnumerable<TItem> ApplyGlobalSearch(IEnumerable<TItem> items)
    {
        var terms = GetSearchTerms().ToList();

        if (terms.Count == 0)
            return items;

        var normalizedTerms = terms
            .Select(NormalizeText)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();

        var searchableColumns = GetSearchableColumns().ToList();

        if (searchableColumns.Count == 0 || normalizedTerms.Count == 0)
            return items;

        return items.Where(item =>
        {
            var values = searchableColumns
                .Select(column => NormalizeText(GetSearchableText(item, column)))
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList();

            if (values.Count == 0)
                return false;

            return ResolvedSearchTextParseMode switch
            {
                GridSearchTextParseMode.GroupWordsByAnd =>
                    normalizedTerms.All(term => values.Any(value => value.Contains(term, StringComparison.OrdinalIgnoreCase))),

                GridSearchTextParseMode.GroupWordsByOr =>
                    normalizedTerms.Any(term => values.Any(value => value.Contains(term, StringComparison.OrdinalIgnoreCase))),

                GridSearchTextParseMode.ExactMatch =>
                    values.Any(value => value.Contains(normalizedTerms[0], StringComparison.OrdinalIgnoreCase)),

                _ => false
            };
        });
    }

    private OutlanderGridCellContext<TItem> BuildCellContext(TItem item, OutlanderGridColumnDefinition<TItem> column)
    {
        return new OutlanderGridCellContext<TItem>
        {
            Item = item,
            FieldName = column.FieldName,
            SearchText = SearchBoxText,
            Highlight = text => HighlightText(text)
        };
    }

    private MarkupString HighlightAutomaticCell(TItem item, OutlanderGridColumnDefinition<TItem> column)
    {
        var value = GetPropertyValue(item, column.FieldName);
        var text = FormatDisplayValue(value);
        return HighlightText(text);
    }

    private MarkupString HighlightText(string? text)
    {
        var raw = text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(raw) || string.IsNullOrWhiteSpace(SearchBoxText))
            return new MarkupString(WebUtility.HtmlEncode(raw));

        var normalizedTerms = GetSearchTerms()
            .Select(NormalizeText)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalizedTerms.Count == 0)
            return new MarkupString(WebUtility.HtmlEncode(raw));

        var map = BuildNormalizedTextMap(raw);
        var normalizedMatches = FindNormalizedMatches(map.NormalizedText, normalizedTerms);

        if (normalizedMatches.Count == 0)
            return new MarkupString(WebUtility.HtmlEncode(raw));

        var originalRanges = MapMatchesToOriginalRanges(map, normalizedMatches);
        var html = BuildHighlightedHtml(map.OriginalText, originalRanges);

        return new MarkupString(html);
    }

    private sealed class NormalizedTextMap
    {
        public string OriginalText { get; set; } = string.Empty;
        public string NormalizedText { get; set; } = string.Empty;
        public List<int> OriginalIndexMap { get; set; } = new();
    }

    private static NormalizedTextMap BuildNormalizedTextMap(string? text)
    {
        var original = text ?? string.Empty;
        var normalizedBuilder = new StringBuilder();
        var indexMap = new List<int>();

        for (var i = 0; i < original.Length; i++)
        {
            var current = original[i].ToString();
            var decomposed = current.Normalize(NormalizationForm.FormD);

            foreach (var ch in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) == UnicodeCategory.NonSpacingMark)
                    continue;

                normalizedBuilder.Append(char.ToLowerInvariant(ch));
                indexMap.Add(i);
            }
        }

        return new NormalizedTextMap
        {
            OriginalText = original,
            NormalizedText = normalizedBuilder.ToString(),
            OriginalIndexMap = indexMap
        };
    }

    private static List<(int Start, int Length)> FindNormalizedMatches(string normalizedText, List<string> normalizedTerms)
    {
        var matches = new List<(int Start, int Length)>();

        foreach (var term in normalizedTerms
                     .Where(t => !string.IsNullOrWhiteSpace(t))
                     .Distinct(StringComparer.OrdinalIgnoreCase)
                     .OrderByDescending(t => t.Length))
        {
            var startIndex = 0;

            while (startIndex < normalizedText.Length)
            {
                var index = normalizedText.IndexOf(term, startIndex, StringComparison.OrdinalIgnoreCase);
                if (index < 0)
                    break;

                matches.Add((index, term.Length));
                startIndex = index + term.Length;
            }
        }

        return MergeOverlappingMatches(matches);
    }

    private static List<(int Start, int Length)> MergeOverlappingMatches(List<(int Start, int Length)> matches)
    {
        if (matches.Count == 0)
            return matches;

        var ordered = matches
            .OrderBy(m => m.Start)
            .ThenByDescending(m => m.Length)
            .ToList();

        var merged = new List<(int Start, int Length)>();
        var currentStart = ordered[0].Start;
        var currentEnd = ordered[0].Start + ordered[0].Length;

        for (var i = 1; i < ordered.Count; i++)
        {
            var nextStart = ordered[i].Start;
            var nextEnd = ordered[i].Start + ordered[i].Length;

            if (nextStart <= currentEnd)
            {
                currentEnd = Math.Max(currentEnd, nextEnd);
            }
            else
            {
                merged.Add((currentStart, currentEnd - currentStart));
                currentStart = nextStart;
                currentEnd = nextEnd;
            }
        }

        merged.Add((currentStart, currentEnd - currentStart));
        return merged;
    }

    private static List<(int Start, int Length)> MapMatchesToOriginalRanges(
        NormalizedTextMap map,
        List<(int Start, int Length)> normalizedMatches)
    {
        var ranges = new List<(int Start, int Length)>();

        foreach (var match in normalizedMatches)
        {
            if (match.Start < 0 || match.Start >= map.OriginalIndexMap.Count)
                continue;

            var normalizedEnd = match.Start + match.Length - 1;
            if (normalizedEnd < 0 || normalizedEnd >= map.OriginalIndexMap.Count)
                continue;

            var originalStart = map.OriginalIndexMap[match.Start];
            var originalEnd = map.OriginalIndexMap[normalizedEnd];

            ranges.Add((originalStart, originalEnd - originalStart + 1));
        }

        return MergeOverlappingMatches(ranges);
    }

    private static string BuildHighlightedHtml(string originalText, List<(int Start, int Length)> ranges)
    {
        if (string.IsNullOrEmpty(originalText))
            return string.Empty;

        if (ranges.Count == 0)
            return WebUtility.HtmlEncode(originalText);

        var sb = new StringBuilder();
        var currentIndex = 0;

        foreach (var range in ranges)
        {
            if (range.Start > currentIndex)
            {
                sb.Append(WebUtility.HtmlEncode(originalText[currentIndex..range.Start]));
            }

            var end = range.Start + range.Length;
            if (end > originalText.Length)
                end = originalText.Length;

            sb.Append("<mark>");
            sb.Append(WebUtility.HtmlEncode(originalText[range.Start..end]));
            sb.Append("</mark>");

            currentIndex = end;
        }

        if (currentIndex < originalText.Length)
        {
            sb.Append(WebUtility.HtmlEncode(originalText[currentIndex..]));
        }

        return sb.ToString();
    }

    private string FormatDisplayValue(object? value)
    {
        if (value is null)
            return string.Empty;

        return value switch
        {
            bool b => b ? BooleanTrueText : BooleanFalseText,
            DateTime dt => dt.ToString("dd/MM/yyyy HH:mm:ss"),
#if NET6_0_OR_GREATER
            DateOnly d => d.ToString("dd/MM/yyyy"),
#endif
            _ => value.ToString() ?? string.Empty
        };
    }

}