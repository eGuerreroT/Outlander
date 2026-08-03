namespace Outlander.Blazor.Components;

/// <summary>
/// Represents the export request sent by the grid in server-side mode.
/// </summary>
public sealed class OutlanderGridExportRequest
{
    /// <summary>
    /// Gets or sets the current global search text.
    /// </summary>
    public string SearchText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the field name used for sorting.
    /// </summary>
    public string? SortField { get; set; }

    /// <summary>
    /// Gets or sets the current sort direction.
    /// </summary>
    public GridColumnSortOrder SortOrder { get; set; } = GridColumnSortOrder.None;

    /// <summary>
    /// Gets or sets the filter values keyed by field name.
    /// </summary>
    public Dictionary<string, OutlanderGridFilterValue> Filters { get; set; } = [];
}