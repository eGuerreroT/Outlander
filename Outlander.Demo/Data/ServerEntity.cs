namespace Outlander.Demo.Data;

public sealed class ServerEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string Cluster { get; set; } = string.Empty;
    public string OperatingSystem { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal MemoryGb { get; set; }
}