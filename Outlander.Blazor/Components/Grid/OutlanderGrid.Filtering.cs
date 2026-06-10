using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Outlander.Blazor.Components.OutlanderGrid;

public partial class OutlanderGrid<TItem>
{
    private OutlanderGridFilterValue GetOrCreateFilter(string fieldName)
    {
        if (!_filters.TryGetValue(fieldName, out var filter))
        {
            filter = new OutlanderGridFilterValue();
            _filters[fieldName] = filter;
        }

        return filter;
    }

    private string GetFilterValue(string fieldName)
        => GetOrCreateFilter(fieldName).Value ?? string.Empty;

    private string GetFilterValueTo(string fieldName)
        => GetOrCreateFilter(fieldName).ValueTo ?? string.Empty;

    private void SetFilterValue(string fieldName, string? value)
    {
        GetOrCreateFilter(fieldName).Value = value ?? string.Empty;
        CurrentPage = 1;
    }

    private void SetFilterValueTo(string fieldName, string? value)
    {
        GetOrCreateFilter(fieldName).ValueTo = value ?? string.Empty;
        CurrentPage = 1;
    }

    private IEnumerable<TItem> ApplyFilters(IEnumerable<TItem> items)
    {
        var query = items;

        foreach (var entry in _filters.Where(f =>
                     !string.IsNullOrWhiteSpace(f.Value.Value) ||
                     !string.IsNullOrWhiteSpace(f.Value.ValueTo)))
        {
            var fieldName = entry.Key;
            var filter = entry.Value;
            var column = VisibleColumnsDefinition.FirstOrDefault(c =>
                string.Equals(c.FieldName, fieldName, StringComparison.OrdinalIgnoreCase));

            if (column is null || !column.AllowFilter)
                continue;

            var mode = ResolveFilterMode(column);

            query = query.Where(item => MatchesFilter(item, column, mode, filter));
        }

        return query;
    }

    private OutlanderGridFilterContext BuildFilterContext(OutlanderGridColumnDefinition<TItem> column)
    {
        return new OutlanderGridFilterContext
        {
            FieldName = column.FieldName,
            Value = GetFilterValue(column.FieldName),
            SetValue = value => SetFilterValue(column.FieldName, value)
        };
    }

    private static Type? GetColumnPropertyType(string fieldName)
    {
        var prop = typeof(TItem).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        return prop?.PropertyType;
    }

    private GridFilterMode ResolveFilterMode(OutlanderGridColumnDefinition<TItem> column)
    {
        if (column.FilterTemplate is not null)
            return column.FilterMode;

        if (column.FilterMode != GridFilterMode.Auto)
            return column.FilterMode;

        var propertyType = GetColumnPropertyType(column.FieldName);
        if (propertyType is null)
            return GridFilterMode.Text;

        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType == typeof(bool))
            return GridFilterMode.Select;

        if (underlyingType == typeof(DateTime)
#if NET6_0_OR_GREATER
            || underlyingType == typeof(DateOnly)
#endif
           )
            return GridFilterMode.Date;

        return GridFilterMode.Text;
    }

    private RenderFragment RenderAutoFilter(OutlanderGridColumnDefinition<TItem> column) => builder =>
    {
        var mode = ResolveFilterMode(column);
        var fieldName = column.FieldName;
        var seq = 0;

        switch (mode)
        {
            case GridFilterMode.Select:
                builder.OpenElement(seq++, "select");
                builder.AddAttribute(seq++, "class", "form-select form-select-sm");
                builder.AddAttribute(seq++, "value", GetFilterValue(fieldName));
                builder.AddAttribute(seq++, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetFilterValue(fieldName, e.Value?.ToString())));

                builder.OpenElement(seq++, "option");
                builder.AddAttribute(seq++, "value", "");
                builder.AddContent(seq++, ResolvedFilterAllText);
                builder.CloseElement();

                builder.OpenElement(seq++, "option");
                builder.AddAttribute(seq++, "value", "true");
                builder.AddContent(seq++, BooleanTrueText);
                builder.CloseElement();

                builder.OpenElement(seq++, "option");
                builder.AddAttribute(seq++, "value", "false");
                builder.AddContent(seq++, BooleanFalseText);
                builder.CloseElement();

                builder.CloseElement();
                break;

            case GridFilterMode.Date:
                builder.OpenElement(seq++, "input");
                builder.AddAttribute(seq++, "class", "form-control form-control-sm");
                builder.AddAttribute(seq++, "type", "date");
                builder.AddAttribute(seq++, "value", GetFilterValue(fieldName));
                builder.AddAttribute(seq++, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetFilterValue(fieldName, e.Value?.ToString())));
                builder.CloseElement();
                break;

            case GridFilterMode.Range:
                if (!IsRangeSupported(column))
                {
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "class", "d-flex align-items-center justify-content-center");

                    builder.OpenElement(seq++, "i");
                    builder.AddAttribute(seq++, "class", "bi bi-exclamation-triangle-fill text-bg-warning px-2 py-1 rounded bg-gradient bg-opacity-50 cursor-help");
                    builder.AddAttribute(seq++, "title", GetInvalidRangeReason(column));
                    builder.AddAttribute(seq++, "data-bs-toggle", "tooltip");
                    builder.AddAttribute(seq++, "data-bs-placement", "top");
                    builder.AddAttribute(seq++, "role", "img");
                    builder.AddAttribute(seq++, "aria-label", "Advertencia de filtro");
                    builder.CloseElement();

                    builder.CloseElement();
                    break;
                }

                var inputType = GetRangeInputType(column);

                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "app-grid-range-filter");

                builder.OpenElement(seq++, "input");
                builder.AddAttribute(seq++, "class", "form-control form-control-sm");
                builder.AddAttribute(seq++, "type", inputType);
                builder.AddAttribute(seq++, "placeholder", ResolvedFilterFromText);
                builder.AddAttribute(seq++, "value", GetFilterValue(fieldName));
                builder.AddAttribute(seq++, "oninput",
                    EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetFilterValue(fieldName, e.Value?.ToString())));
                builder.CloseElement();

                builder.OpenElement(seq++, "input");
                builder.AddAttribute(seq++, "class", "form-control form-control-sm mt-1");
                builder.AddAttribute(seq++, "type", inputType);
                builder.AddAttribute(seq++, "placeholder", ResolvedFilterToText);
                builder.AddAttribute(seq++, "value", GetFilterValueTo(fieldName));
                builder.AddAttribute(seq++, "oninput",
                    EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetFilterValueTo(fieldName, e.Value?.ToString())));
                builder.CloseElement();

                builder.CloseElement();
                break;

            case GridFilterMode.Text:
            case GridFilterMode.Auto:
            default:
                builder.OpenElement(seq++, "input");
                builder.AddAttribute(seq++, "class", "form-control form-control-sm");
                builder.AddAttribute(seq++, "placeholder", ResolvedFilterNullText);
                builder.AddAttribute(seq++, "value", GetFilterValue(fieldName));
                builder.AddAttribute(seq++, "oninput",
                    EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetFilterValue(fieldName, e.Value?.ToString())));
                builder.CloseElement();
                break;
        }
    };

    private bool MatchesFilter(TItem item, OutlanderGridColumnDefinition<TItem> column, GridFilterMode mode, OutlanderGridFilterValue filter)
    {
        return mode switch
        {
            GridFilterMode.Select => MatchesSelectFilter(item, column, filter),
            GridFilterMode.Date => MatchesDateFilter(item, column, filter),
            GridFilterMode.Range => MatchesRangeFilter(item, column, filter),
            _ => MatchesTextFilter(item, column, filter)
        };
    }

    private bool MatchesTextFilter(TItem item, OutlanderGridColumnDefinition<TItem> column, OutlanderGridFilterValue filter)
    {
        var filterText = NormalizeText(filter.Value);
        if (string.IsNullOrWhiteSpace(filterText))
            return true;

        var text = column.FilterTextSelector is not null
            ? column.FilterTextSelector(item) ?? string.Empty
            : GetPropertyValue(item, column.FieldName)?.ToString() ?? string.Empty;

        return NormalizeText(text).Contains(filterText, StringComparison.OrdinalIgnoreCase);
    }

    private bool MatchesSelectFilter(TItem item, OutlanderGridColumnDefinition<TItem> column, OutlanderGridFilterValue filter)
    {
        if (string.IsNullOrWhiteSpace(filter.Value))
            return true;

        var value = GetPropertyValue(item, column.FieldName);
        if (value is null)
            return false;

        var normalized = filter.Value.Trim().ToLowerInvariant();

        if (value is bool boolValue)
        {
            return normalized switch
            {
                "true" => boolValue,
                "false" => !boolValue,
                _ => true
            };
        }

        return string.Equals(value.ToString(), filter.Value, StringComparison.OrdinalIgnoreCase);
    }

    private bool MatchesDateFilter(TItem item, OutlanderGridColumnDefinition<TItem> column, OutlanderGridFilterValue filter)
    {
        if (string.IsNullOrWhiteSpace(filter.Value))
            return true;

        var value = GetPropertyValue(item, column.FieldName);
        if (value is null)
            return false;

        if (!DateTime.TryParse(filter.Value, out var filterDate))
            return true;

        return value switch
        {
            DateTime dateTimeValue => dateTimeValue.Date == filterDate.Date,
#if NET6_0_OR_GREATER
            DateOnly dateOnlyValue => dateOnlyValue == DateOnly.FromDateTime(filterDate),
#endif
            _ => string.Equals(value.ToString(), filter.Value, StringComparison.OrdinalIgnoreCase)
        };
    }

    private bool MatchesRangeFilter(TItem item, OutlanderGridColumnDefinition<TItem> column, OutlanderGridFilterValue filter)
    {
        if (!IsRangeSupported(column))
            return true;

        var from = filter.Value;
        var to = filter.ValueTo;

        if (string.IsNullOrWhiteSpace(from) && string.IsNullOrWhiteSpace(to))
            return true;

        var value = GetPropertyValue(item, column.FieldName);
        if (value is null)
            return false;

        if (value is IComparable comparableValue)
        {
            var propertyType = GetColumnPropertyType(column.FieldName);
            var targetType = Nullable.GetUnderlyingType(propertyType ?? value.GetType()) ?? value.GetType();

            var fromValue = ConvertToComparable(from, targetType);
            var toValue = ConvertToComparable(to, targetType);

            if (fromValue is IComparable fromComparable && comparableValue.CompareTo(fromComparable) < 0)
                return false;

            if (toValue is IComparable toComparable && comparableValue.CompareTo(toComparable) > 0)
                return false;

            return true;
        }

        return true;
    }

    private static object? ConvertToComparable(string? value, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        try
        {
            if (targetType == typeof(DateTime))
                return DateTime.Parse(value);

#if NET6_0_OR_GREATER
            if (targetType == typeof(DateOnly))
                return DateOnly.Parse(value);
#endif

            if (targetType.IsEnum)
                return Enum.Parse(targetType, value, true);

            return Convert.ChangeType(value, targetType);
        }
        catch
        {
            return null;
        }
    }

    private bool IsRangeSupported(OutlanderGridColumnDefinition<TItem> column)
    {
        var propertyType = GetColumnPropertyType(column.FieldName);
        if (propertyType is null)
            return false;

        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType == typeof(bool) ||
            underlyingType == typeof(string) ||
            underlyingType == typeof(TimeSpan) ||
            underlyingType.IsEnum)
        {
            return false;
        }

        return underlyingType == typeof(byte)
            || underlyingType == typeof(short)
            || underlyingType == typeof(int)
            || underlyingType == typeof(long)
            || underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal)
            || underlyingType == typeof(DateTime)
#if NET6_0_OR_GREATER
            || underlyingType == typeof(DateOnly)
#endif
            ;
    }

    private string GetRangeInputType(OutlanderGridColumnDefinition<TItem> column)
    {
        var propertyType = GetColumnPropertyType(column.FieldName);
        var underlyingType = propertyType is null
            ? null
            : Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType == typeof(DateTime)
#if NET6_0_OR_GREATER
            || underlyingType == typeof(DateOnly)
#endif
           )
        {
            return "date";
        }

        if (underlyingType == typeof(byte)
            || underlyingType == typeof(short)
            || underlyingType == typeof(int)
            || underlyingType == typeof(long)
            || underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal))
        {
            return "number";
        }

        return "text";
    }

    private string GetInvalidRangeReason(OutlanderGridColumnDefinition<TItem> column)
    {
        var propertyType = GetColumnPropertyType(column.FieldName);
        if (propertyType is null)
            return $"La propiedad '{column.FieldName}' no existe o no pudo resolverse.";

        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType == typeof(bool))
            return $"Range no es válido para columnas booleanas ({underlyingType.Name}).";

        if (underlyingType == typeof(string))
            return $"Range no es válido para columnas de texto ({underlyingType.Name}).";

        if (underlyingType == typeof(TimeSpan))
            return $"Range no es válido para columnas TimeSpan.";

        if (underlyingType.IsEnum)
            return $"Range no es válido para enumeraciones ({underlyingType.Name}).";

        return $"Range no es compatible con el tipo {underlyingType.Name}.";
    }

}