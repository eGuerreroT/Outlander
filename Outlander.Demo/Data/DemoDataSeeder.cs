namespace Outlander.Demo.Data;

public static class DemoDataSeeder
{
    public static void Seed(DemoDbContext db)
    {
        if (db.Servers.Any())
            return;

        var providers = new[] { "VMware", "Alibaba", "AWS", "Azure" };
        var statuses = new[] { "Running", "Powered Off", "Maintenance" };
        var clusters = new[] { "Cluster-Prod", "Cluster-Apps", "Cluster-Web", "Cluster-Test" };
        var systems = new[] { "Ubuntu 22.04", "RHEL 8.6", "Debian 11", "Windows Server 2022" };

        var rng = new Random(2026);
        var rows = new List<ServerEntity>(1500);

        for (var i = 1; i <= 1500; i++)
        {
            rows.Add(new ServerEntity
            {
                Id = i,
                Name = $"vm-{i:0000}",
                Provider = providers[rng.Next(providers.Length)],
                Status = statuses[rng.Next(statuses.Length)],
                Ip = $"10.10.{rng.Next(1, 255)}.{rng.Next(1, 255)}",
                Cluster = clusters[rng.Next(clusters.Length)],
                OperatingSystem = systems[rng.Next(systems.Length)],
                CreatedAt = DateTime.Today.AddDays(-rng.Next(0, 900)),
                MemoryGb = rng.Next(4, 257)
            });
        }

        db.Servers.AddRange(rows);
        db.SaveChanges();
    }
}