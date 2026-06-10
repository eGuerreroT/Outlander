namespace Outlander.Blazor.Components.OutlanderGrid;

public partial class OutlanderGrid<TItem>
{
    private void EnsureInitialSort()
    {
        if (_initialSortApplied || VisibleColumnsDefinition.Count == 0)
            return;

        var initialColumn = VisibleColumnsDefinition
            .Where(c => c.AllowSort && c.SortOrder != GridColumnSortOrder.None)
            .OrderBy(c => c.SortIndex ?? int.MaxValue)
            .FirstOrDefault();

        if (initialColumn is not null)
        {
            _sortFieldName = initialColumn.FieldName;
            _sortOrder = initialColumn.SortOrder;
        }

        _initialSortApplied = true;
    }

    private void ToggleSort(OutlanderGridColumnDefinition<TItem> column)
    {
        if (!AllowSort || !column.AllowSort)
            return;

        if (_sortFieldName != column.FieldName)
        {
            _sortFieldName = column.FieldName;
            _sortOrder = GridColumnSortOrder.Ascending;
            CurrentPage = 1;
            return;
        }

        _sortOrder = _sortOrder switch
        {
            GridColumnSortOrder.None => GridColumnSortOrder.Ascending,
            GridColumnSortOrder.Ascending => GridColumnSortOrder.Descending,
            GridColumnSortOrder.Descending => GridColumnSortOrder.None,
            _ => GridColumnSortOrder.None
        };

        if (_sortOrder == GridColumnSortOrder.None)
        {
            _sortFieldName = null;
        }

        CurrentPage = 1;
    }

    private string GetSortIcon(OutlanderGridColumnDefinition<TItem> column)
    {
        if (!AllowSort || !column.AllowSort)
            return string.Empty;

        if (_sortFieldName != column.FieldName)
            return "bi bi-arrow-down-up";

        return _sortOrder switch
        {
            GridColumnSortOrder.Ascending => "bi bi-sort-down-alt",
            GridColumnSortOrder.Descending => "bi bi-sort-down",
            _ => "bi bi-arrow-down-up"
        };
    }

    private IEnumerable<TItem> ApplySorting(IEnumerable<TItem> items)
    {
        EnsureInitialSort();

        if (!AllowSort || string.IsNullOrWhiteSpace(_sortFieldName) || _sortOrder == GridColumnSortOrder.None)
            return items;

        var column = VisibleColumnsDefinition.FirstOrDefault(c =>
            string.Equals(c.FieldName, _sortFieldName, StringComparison.OrdinalIgnoreCase));

        if (column is null)
            return items;

        if (column.SortMode == GridColumnSortMode.DisplayText)
        {
            Func<TItem, string?> sortTextSelector = column.SortTextSelector
                ?? (item => GetPropertyValue(item, column.FieldName)?.ToString());

            return _sortOrder == GridColumnSortOrder.Ascending
                ? items.OrderBy(sortTextSelector, StringComparer.OrdinalIgnoreCase)
                : items.OrderByDescending(sortTextSelector, StringComparer.OrdinalIgnoreCase);
        }

        Func<TItem, object?> valueSelector = item => GetPropertyValue(item, column.FieldName);

        return _sortOrder == GridColumnSortOrder.Ascending
            ? items.OrderBy(valueSelector)
            : items.OrderByDescending(valueSelector);
    }

}