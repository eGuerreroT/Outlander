public class OutlanderNavMenuItem
{
    /// <summary>
    /// Unique identifier for the menu item.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Display text for the menu item.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Bootstrap icon class suffix. Example: bi-grid
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Optional navigation URL.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Optional tooltip text.
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Indicates whether the item should be marked as active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indicates whether the submenu should start expanded.
    /// </summary>
    public bool IsExpandedByDefault { get; set; }

    /// <summary>
    /// Child menu items.
    /// </summary>
    public List<OutlanderNavMenuItem> Children { get; set; } = [];

    /// <summary>
    /// Indicates whether the item has child items.
    /// </summary>
    public bool HasChildren => Children.Count > 0;

    internal bool IsRouteActive { get; set; }
    internal bool HasActiveDescendant { get; set; }
    internal bool IsExpanded { get; set; }
}