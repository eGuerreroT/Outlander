namespace Outlander.Blazor.Components;

/// <summary>
/// Defines how data is processed by <see cref="OutlanderGrid{TItem}"/>.
/// </summary>
public enum OutlanderGridDataMode
{
    /// <summary>
    /// Filtering, searching, sorting and paging are executed in-memory.
    /// </summary>
    Client = 0,

    /// <summary>
    /// Filtering, searching, sorting and paging are executed by the data provider.
    /// </summary>
    Server = 1
}