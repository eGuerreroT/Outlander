namespace Outlander.Blazor.Components;

/// <summary>
/// Represents the server-side response consumed by <see cref="OutlanderGrid{TItem}"/>.
/// </summary>
public sealed class OutlanderGridDataResult<TItem>
{
    /// <summary>
    /// Gets or sets the current page items.
    /// </summary>
    public IReadOnlyList<TItem> Items { get; set; } = Array.Empty<TItem>();

    /// <summary>
    /// Gets or sets the total item count after filters/search (before paging).
    /// </summary>
    public int TotalCount { get; set; }
}