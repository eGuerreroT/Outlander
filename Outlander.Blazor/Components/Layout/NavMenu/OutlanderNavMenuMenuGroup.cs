public class OutlanderNavMenuMenuGroup
{
    /// <summary>
    /// Optional section title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Items rendered inside the group.
    /// </summary>
    public List<OutlanderNavMenuItem> Items { get; set; } = [];
}