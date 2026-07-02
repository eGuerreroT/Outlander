using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents an action that can be rendered by the <c>TopMenu</c> component.
/// </summary>
public class OutlanderTopMenuActionItem
{
    /// <summary>
    /// Gets or sets the accessible title and tooltip text for the action.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the Bootstrap icon class suffix for the action.
    /// Example: <c>bi-bell</c>.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional badge text displayed on the action.
    /// </summary>
    public string? BadgeText { get; set; }

    /// <summary>
    /// Gets or sets optional additional CSS classes for the rendered button.
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the action is clicked.
    /// </summary>
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this action should remain visible
    /// in the mobile top bar instead of being moved into the mobile offcanvas panel.
    /// </summary>
    public bool KeepVisibleOnMobile { get; set; } = false;
}