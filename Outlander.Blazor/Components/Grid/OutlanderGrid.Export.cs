using Microsoft.JSInterop;

namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem>
{
    private IReadOnlyList<OutlanderGridColumnDefinition<TItem>> ExportableColumns =>
        [.. VisibleColumnsDefinition.Where(c => c.AllowExport && !c.IsSelectionColumn)];

    private IEnumerable<TItem> GetExportItems()
    {
        return ProcessedItems;
    }

    private object? GetExportValue(TItem item, OutlanderGridColumnDefinition<TItem> column)
    {
        if (column.ExportValueSelector is not null)
            return column.ExportValueSelector(item);

        if (column.FilterTextSelector is not null)
            return column.FilterTextSelector(item);

        if (column.SortTextSelector is not null)
            return column.SortTextSelector(item);

        return GetPropertyValue(item, column.FieldName);
    }

    private string GetExportHeader(OutlanderGridColumnDefinition<TItem> column)
    {
        return string.IsNullOrWhiteSpace(column.ExportCaption)
            ? column.Caption
            : column.ExportCaption!;
    }

    private string FormatExportValue(object? value)
    {
        if (value is null)
            return string.Empty;

        return value switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
#if NET6_0_OR_GREATER
            DateOnly d => d.ToString("yyyy-MM-dd"),
#endif
            bool b => b ? BooleanTrueText : BooleanFalseText,
            _ => value.ToString() ?? string.Empty
        };
    }

    private List<string> BuildExportHeaders()
    {
        return [.. ExportableColumns.Select(GetExportHeader)];
    }

    private List<List<string>> BuildExportRows()
    {
        var columns = ExportableColumns;
        var items = GetExportItems();

        var rows = new List<List<string>>();

        foreach (var item in items)
        {
            var row = new List<string>();

            foreach (var column in columns)
            {
                var value = GetExportValue(item, column);
                row.Add(FormatExportValue(value));
            }

            rows.Add(row);
        }

        return rows;
    }

    private async Task ExportExcelAsync()
    {
        if (_module is null)
            return;

        switch (ResolvedExcelExportMode)
        {
            case OutlanderGridExportMode.Data:
            default:
                {
                    var headers = BuildExportHeaders();
                    var rows = BuildExportRows();

                    await _module.InvokeVoidAsync(
                        "exportExcel",
                        SanitizeExportFileName(ResolvedExportFileName),
                        ResolvedExportTitle,
                        headers,
                        rows);

                    break;
                }

            case OutlanderGridExportMode.Wysiwyg:
                {
                    // Por ahora usamos Data también, hasta tener una estrategia WYSIWYG real para Excel.
                    var headers = BuildExportHeaders();
                    var rows = BuildExportRows();

                    await _module.InvokeVoidAsync(
                        "exportExcel",
                        SanitizeExportFileName(ResolvedExportFileName),
                        ResolvedExportTitle,
                        headers,
                        rows);

                    break;
                }
        }
    }

    private async Task ExportPdfAsync()
    {
        if (_module is null)
            return;

        switch (ResolvedPdfExportMode)
        {
            case OutlanderGridExportMode.Wysiwyg:
                {
                    await _module.InvokeVoidAsync(
                        "exportPdfWysiwyg",
                        _tableWrapperRef,
                        SanitizeExportFileName(ResolvedExportFileName),
                        ResolvedExportTitle);

                    break;
                }

            case OutlanderGridExportMode.Data:
            default:
                {
                    var headers = BuildExportHeaders();
                    var rows = BuildExportRows();

                    await _module.InvokeVoidAsync(
                        "exportPdf",
                        SanitizeExportFileName(ResolvedExportFileName),
                        ResolvedExportTitle,
                        headers,
                        rows);

                    break;
                }
        }
    }

    private async Task PrintAsync()
    {
        if (_module is null)
            return;

        switch (ResolvedPrintExportMode)
        {
            case OutlanderGridExportMode.Wysiwyg:
                {
                    await _module.InvokeVoidAsync(
                        "printGridWysiwyg",
                        _tableWrapperRef,
                        ResolvedExportTitle);

                    break;
                }

            case OutlanderGridExportMode.Data:
            default:
                {
                    var headers = BuildExportHeaders();
                    var rows = BuildExportRows();

                    await _module.InvokeVoidAsync(
                        "printGrid",
                        ResolvedExportTitle,
                        headers,
                        rows);

                    break;
                }
        }
    }

    private static string SanitizeExportFileName(string? fileName)
    {
        var fallback = "grid-export";
        var name = string.IsNullOrWhiteSpace(fileName) ? fallback : fileName.Trim();

        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '-');
        }

        return string.IsNullOrWhiteSpace(name) ? fallback : name;
    }

}