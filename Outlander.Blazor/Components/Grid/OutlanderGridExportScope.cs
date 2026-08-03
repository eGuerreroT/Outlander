namespace Outlander.Blazor.Components;

/// <summary>
/// Defines which dataset should be exported by the grid.
/// </summary>
public enum OutlanderGridExportScope
{
    /// <summary>
    /// Exports the items currently rendered in the grid page.
    /// </summary>
    CurrentPage = 0,

    /// <summary>
    /// Exports all items that match current search/sort/filter state.
    /// </summary>
    AllFiltered = 1
}