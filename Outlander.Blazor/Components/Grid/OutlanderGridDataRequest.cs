namespace Outlander.Blazor.Components;

/// <summary>
/// Represents the data request sent by the grid when using server-side data mode.
/// </summary>
public sealed class OutlanderGridDataRequest
{
    /// <summary>
    /// Gets or sets the requested page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the requested page size.
    /// </summary>
    public int PageSize { get; set; } = 10;

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