namespace Outlander.Blazor.Components.OutlanderGrid;

public partial class OutlanderGrid<TItem>
{
    private void SetColumnVisibility(string fieldName, bool visible)
    {
        var column = AllColumnsDefinition.FirstOrDefault(c =>
            string.Equals(c.FieldName, fieldName, StringComparison.OrdinalIgnoreCase));

        if (column is null)
            return;

        if (!visible && VisibleColumnsDefinition.Count == 1 && column.Visible)
            return;

        column.Visible = visible;

        if (!visible && string.Equals(_sortFieldName, fieldName, StringComparison.OrdinalIgnoreCase))
        {
            _sortFieldName = null;
            _sortOrder = GridColumnSortOrder.None;
        }

        CurrentPage = 1;
        StateHasChanged();
    }

    private string GetHeaderCellCssClass(OutlanderGridColumnDefinition<TItem> column)
    {
        var classes = new List<string>();

        if (column.IsSelectionColumn || !column.AllowExport)
        {
            classes.Add("app-grid-export-ignore");
        }

        return string.Join(" ", classes);
    }

    private string GetDataCellCssClass(OutlanderGridColumnDefinition<TItem> column)
    {
        var classes = new List<string>();

        if (column.IsSelectionColumn || !column.AllowExport)
        {
            classes.Add("app-grid-export-ignore");
        }

        return string.Join(" ", classes);
    }

}