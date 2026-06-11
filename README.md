
<p align="center">
  <img src="Assets/Social_Media.png" width="80%" />
</p>

<h1 align="center">Outlander.Blazor</h1>

<h2 align="center">
  Build Business Applications Faster with Blazor
</h2>

Outlander.Blazor is a modern component library for Blazor applications focused on productivity, performance, and enterprise scenarios.

The project provides reusable UI components designed to simplify the development of business applications such as ERP, CRM, POS, reporting systems, dashboards, and internal management platforms.

---

## Features

### OutlanderGrid

The first component included in the library is a powerful data grid with support for:

- Server-side and client-side data binding
- Sorting
- Filtering
- Global search
- Row selection
- Column customization
- Footer summaries
- Excel export
- PDF export
- Responsive design
- Bootstrap 5 integration (Dark theme compatible)
- Blazor Server support
- Blazor WebAssembly support

---

> [!IMPORTANT]
> Outlander.Blazor requires **Bootstrap Bundle 5.3 or later**.
>
> Make sure the Bootstrap bundle script is loaded before using Outlander components:
>
> ```html
> <script src="@Assets["lib/bootstrap/dist/js/bootstrap.bundle.js"]"></script>
> ```
>
> If Bootstrap is missing or an unsupported version is loaded, a runtime exception similar to the following will be thrown:
>
> ```text
> Bootstrap {bootstrapVersion} is not supported. Bootstrap 5.3 or later is required.
> ```
>
> The Bootstrap bundle is required because Outlander.Blazor relies on Bootstrap JavaScript components and Popper functionality.


> [!IMPORTANT]
> Outlander.Blazor currently uses Bootstrap Icons internally.
>
> The library automatically loads Bootstrap Icons through its stylesheet using:
>
> ```css
> @import url('https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css');
> ```
>
> Internet access to the CDN is required for icons to be displayed correctly.
>
> A self-contained icon system is planned for a future release.

## Installation

Install the package from NuGet:

```bash
dotnet add package Outlander.Blazor
```

---

## Registration

Add the namespace to your `_Imports.razor`:

```razor
@using Outlander.Blazor
@using Outlander.Blazor.Components
```

If required by future components:

```csharp
builder.Services.AddOutlander();
```

Styles

Add the link to your `App.razor`:

```razor
<link href="_content/Outlander.Blazor/Outlander.Blazor.styles.css" rel="stylesheet">
```

---

## Examples

<details>
<summary>Basic Implementation</summary>

```razor
<OutlanderGrid TItem="ServerItem"
            Items="@ServersA"
            @bind-FocusedRow="@FocusedServerA"
            @bind-SelectedItems="@SelectedServersA"
            ShowFilterRow="true"
            PageSize="7"
            EmptyText="Servers not found.">

    <Columns>

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="Name" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="Provider" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="Status" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="Ip" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="Cluster" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="OperatingSystem" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="IsNew" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="MemoryGb"
                            FilterMode="GridFilterMode.Range" />

        <OutlanderGridDataColumn TItem="ServerItem"
                            FieldName="CreatedAt"
                            FilterMode="GridFilterMode.Range" />
    </Columns>
</OutlanderGrid>
```

</details>

<details>
<summary>Custom Implementation</summary>
  
```razor
<OutlanderGrid TItem="ServerItem"
               Items="@ServersB"
               RowClick="OnRowClick"
               RowDoubleClick="OnRowDoubleClick"
               AllowSort="true"
               AllowHotTrackRow="true"
               AllowFocusedRow="true"
               @bind-FocusedRow="@FocusedServerB"
               @bind-SelectedItems="@SelectedServersB"
               ShowSearchBox="true"
               SearchBoxNullText="Search in all columns..."
               SearchBoxText=""
               ShowExportButtons="true"
               ExcelExportMode="OutlanderGridExportMode.Data"
               PdfExportMode="OutlanderGridExportMode.Data"
               PrintExportMode="OutlanderGridExportMode.Data"
               ExportFileName="Servers"
               ExportTitle="Servers List"
               SearchBoxParseMode="GridSearchTextParseMode.GroupWordsByAnd"
               PageSize="@PageSize"
               PageSizeChanged="OnPageSizeChanged"
               ShowPageSizeSelector="true"
               ShowColumnChooser="true"
               ShowFilterRow="true"
               EmptyText="No Records.">
    <ToolbarTemplate>
        <div class="d-flex align-items-center gap-2 flex-wrap justify-content-end">
            <button class="btn btn-primary"
                    data-bs-toggle="modal"
                    data-bs-target="#importServersModal"
                    disabled="@(SelectedServers.Count == 0)">
                <i class="bi bi-download me-2"></i>
                <span>Import @SelectedServers.Count Selected</span>
            </button>
        </div>
    </ToolbarTemplate>
    <Columns>
        <OutlanderGridSelectionColumn TItem="ServerItem"
                                      Width="48px"
                                      AllowSelectAllItems="true" />

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="Name"
                                 Caption="Name"
                                 AllowFilter="true"
                                 AllowSort="true"
                                 SortMode="GridColumnSortMode.DisplayText"
                                 SortOrder="GridColumnSortOrder.Ascending"
                                 SortIndex="1"
                                 SortTextSelector="item => item.Name"
                                 FilterTextSelector="item => item.Name">
            <FilterTemplate Context="filter">
                <input class="form-control form-control-sm"
                       placeholder="Filter by Name..."
                       value="@filter.Value"
                       @oninput="e => filter.SetValue(e.Value?.ToString())" />
            </FilterTemplate>
            <CellTemplate Context="cell">
                <div class="name-cell">
                    @cell.Highlight(cell.Item.Name)
                    @if (cell.Item.IsNew)
                    {
                        <span class="bg-danger bg-opacity-10 text-danger border border-danger-subtle px-2 py-1 small rounded-pill ms-2">NUEVO</span>
                    }
                </div>
            </CellTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="Provider"
                                 Caption="Provider"
                                 AllowFilter="true"
                                 AllowSort="true"
                                 SortMode="GridColumnSortMode.DisplayText"
                                 SortTextSelector="item => item.Provider"
                                 FilterTextSelector="item => item.Provider">
            <FilterTemplate Context="filter">
                <select class="form-select form-select-sm"
                        value="@filter.Value"
                        @onchange="e => filter.SetValue(e.Value?.ToString())">
                    <option value="">Todos</option>
                    <option value="VMware">VMware</option>
                    <option value="Alibaba">Alibaba</option>
                </select>
            </FilterTemplate>
            <CellTemplate Context="cell">
                <div class="provider-cell">
                    <span class="provider-mini @cell.Item.BootStrapIcon">@cell.Item.Abreviature</span>
                    <span>@cell.Highlight(cell.Item.Provider)</span>
                </div>
            </CellTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="Status"
                                 Caption="Status"
                                 AllowFilter="true"
                                 AllowSort="true"
                                 SortMode="GridColumnSortMode.DisplayText"
                                 SortTextSelector="item => item.Status"
                                 FilterTextSelector="item => item.Status">
            <FilterTemplate Context="filter">
                <select class="form-select form-select-sm"
                        value="@filter.Value"
                        @onchange="e => filter.SetValue(e.Value?.ToString())">
                    <option value="">Todos</option>
                    <option value="Running">Running</option>
                    <option value="Powered Off">Powered Off</option>
                </select>
            </FilterTemplate>
            <CellTemplate Context="cell">
                <div class="status-cell">
                    <span class="status-dot @(cell.Item.Status == "Running" ? "status-green" : "status-orange")"></span>
                    <span>@cell.Highlight(cell.Item.Status)</span>
                </div>
            </CellTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="Ip"
                                 Caption="IP"
                                 AllowFilter="true"
                                 AllowSort="true"
                                 FilterTextSelector="item => item.Ip">
            <FilterTemplate Context="filter">
                <input class="form-control form-control-sm"
                       placeholder="IP..."
                       value="@filter.Value"
                       @oninput="e => filter.SetValue(e.Value?.ToString())" />
            </FilterTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="Cluster"
                                 Caption="Cluster / Resource"
                                 AllowFilter="true"
                                 AllowSort="true"
                                 SortMode="GridColumnSortMode.DisplayText"
                                 SortTextSelector="item => item.Cluster"
                                 FilterTextSelector="item => item.Cluster">
            <FilterTemplate Context="filter">
                <input class="form-control form-control-sm"
                       placeholder="Cluster..."
                       value="@filter.Value"
                       @oninput="e => filter.SetValue(e.Value?.ToString())" />
            </FilterTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="OperatingSystem"
                                 Caption="Operating System"
                                 AllowFilter="true"
                                 AllowSort="true"
                                 SortMode="GridColumnSortMode.DisplayText"
                                 FilterMode="GridFilterMode.Range"
                                 SortTextSelector='item => $"{item.Provider}-({item.OperatingSystem})"'
                                 FilterTextSelector='item => $"{item.Provider}-({item.OperatingSystem})"'>            
            <CellTemplate Context="cell">
                <div class="status-cell">
                    <span class="fw-bold">@cell.Highlight(cell.Item.Provider + "-")</span>
                    <span>@cell.Highlight(cell.Item.OperatingSystem)</span>
                </div>
            </CellTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="_registered"
                                 Caption="Registered"
                                 AllowSort="false"
                                 FilterTextSelector='item => "No"'>
            <CellTemplate Context="cell">
                <span class="bg-danger bg-opacity-10 border border-danger-subtle fw-normal px-2 py-1 rounded small text-danger">
                    @cell.Highlight("No")
                </span>
            </CellTemplate>
        </OutlanderGridDataColumn>

        <OutlanderGridDataColumn TItem="ServerItem"
                                 FieldName="_actions"
                                 Caption="Action"
                                 AllowFilter="false"
                                 AllowSort="false"
                                 AllowExport="false"
                                 Width="90px">
            <CellTemplate Context="cell">
                <button class="btn btn-default btn-default-sm app-grid-export-ignore">
                    <i class="bi bi-download"></i>
                </button>
            </CellTemplate>
        </OutlanderGridDataColumn>
    </Columns>
</OutlanderGrid>
```

</details>

<details>
<summary>Model</summary>

```csharp
public class ServerItem
    {
        public string Name { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Cluster { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string BootStrapIcon { get; set; } = string.Empty;
        public string Abreviature { get; set; } = string.Empty;
        public bool IsNew { get; set; }
        public bool Selected { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal MemoryGb { get; set; }
    }

    private List<ServerItem> ServersA = new()
    {
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 29), MemoryGb = 16 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 04, 29), MemoryGb = 20 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Clúster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 06, 29), MemoryGb = 16 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 05, 9), MemoryGb = 16 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2025, 05, 29), MemoryGb = 32 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2024, 05, 29), MemoryGb = 64 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 07, 29), MemoryGb = 128 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 08, 29), MemoryGb = 32 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 29), MemoryGb = 32 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 29), MemoryGb = 16 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 07, 30), MemoryGb = 64 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 08, 30), MemoryGb = 16 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 20), MemoryGb = 20 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 20), MemoryGb = 20 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 2, 19), MemoryGb = 32 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 2, 9), MemoryGb = 32 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 2, 2), MemoryGb = 20 },
        new() { Name = "vm-web-02", Provider = "Alibaba", Status = "Powered Off", Ip = "10.10.10.12", Cluster = "Cluster-Web", OperatingSystem = "Ubuntu 20.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 04, 29), MemoryGb = 20 },
        new() { Name = "vm-web-02", Provider = "Alibaba", Status = "Powered Off", Ip = "10.10.10.12", Cluster = "Cluster-Web", OperatingSystem = "Ubuntu 20.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 09, 29), MemoryGb = 20 },
        new() { Name = "vm-web-02", Provider = "Alibaba", Status = "Powered Off", Ip = "10.10.10.12", Cluster = "Cluster-Web", OperatingSystem = "Ubuntu 20.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 10, 29), MemoryGb = 20 },
        new() { Name = "vm-app-03", Provider = "VMware", Status = "Running", Ip = "10.10.10.13", Cluster = "Cluster-Apps", OperatingSystem = "CentOS 7", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 11, 29), MemoryGb = 20 },
        new() { Name = "vm-app-03", Provider = "VMware", Status = "Running", Ip = "10.10.10.13", Cluster = "Cluster-Apps", OperatingSystem = "CentOS 7", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 12, 29), MemoryGb = 64 },
        new() { Name = "vm-app-03", Provider = "VMware", Status = "Running", Ip = "10.10.10.13", Cluster = "Cluster-Apps", OperatingSystem = "CentOS 7", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 1, 29), MemoryGb = 64 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 2), MemoryGb = 120 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 19), MemoryGb = 120 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 03, 9), MemoryGb = 16 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 07, 9), MemoryGb = 16 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 07, 19), MemoryGb = 16 },
    };

    private List<ServerItem> ServersB = new()
    {
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 29), MemoryGb = 16 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 04, 29), MemoryGb = 20 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Clúster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 06, 29), MemoryGb = 16 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 05, 9), MemoryGb = 16 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2025, 05, 29), MemoryGb = 32 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2024, 05, 29), MemoryGb = 64 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 07, 29), MemoryGb = 128 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 08, 29), MemoryGb = 32 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 29), MemoryGb = 32 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 29), MemoryGb = 16 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 07, 30), MemoryGb = 64 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 08, 30), MemoryGb = 16 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 20), MemoryGb = 20 },
        new() { Name = "vm-prod-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.10", Cluster = "Cluster-Prod", OperatingSystem = "Ubuntu 22.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 20), MemoryGb = 20 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 2, 19), MemoryGb = 32 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 2, 9), MemoryGb = 32 },
        new() { Name = "vm-db0-01", Provider = "Alibaba", Status = "Running", Ip = "10.10.10.11", Cluster = "Cluster-Prod", OperatingSystem = "RHEL 8.6", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 2, 2), MemoryGb = 20 },
        new() { Name = "vm-web-02", Provider = "Alibaba", Status = "Powered Off", Ip = "10.10.10.12", Cluster = "Cluster-Web", OperatingSystem = "Ubuntu 20.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 04, 29), MemoryGb = 20 },
        new() { Name = "vm-web-02", Provider = "Alibaba", Status = "Powered Off", Ip = "10.10.10.12", Cluster = "Cluster-Web", OperatingSystem = "Ubuntu 20.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 09, 29), MemoryGb = 20 },
        new() { Name = "vm-web-02", Provider = "Alibaba", Status = "Powered Off", Ip = "10.10.10.12", Cluster = "Cluster-Web", OperatingSystem = "Ubuntu 20.04", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-orange", Abreviature = "a", CreatedAt = new DateTime(2026, 10, 29), MemoryGb = 20 },
        new() { Name = "vm-app-03", Provider = "VMware", Status = "Running", Ip = "10.10.10.13", Cluster = "Cluster-Apps", OperatingSystem = "CentOS 7", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 11, 29), MemoryGb = 20 },
        new() { Name = "vm-app-03", Provider = "VMware", Status = "Running", Ip = "10.10.10.13", Cluster = "Cluster-Apps", OperatingSystem = "CentOS 7", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 12, 29), MemoryGb = 64 },
        new() { Name = "vm-app-03", Provider = "VMware", Status = "Running", Ip = "10.10.10.13", Cluster = "Cluster-Apps", OperatingSystem = "CentOS 7", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 1, 29), MemoryGb = 64 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 2), MemoryGb = 120 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 05, 19), MemoryGb = 120 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 03, 9), MemoryGb = 16 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 07, 9), MemoryGb = 16 },
        new() { Name = "vm-test-01", Provider = "VMware", Status = "Running", Ip = "10.10.10.14", Cluster = "Cluster-Test", OperatingSystem = "Debian 11", IsNew = true, Selected = true, BootStrapIcon = "provider-badge-primary", Abreviature = "vm", CreatedAt = new DateTime(2026, 07, 19), MemoryGb = 16 },
    };
```

</details>

---

## Roadmap

### Version 0.x

- [x] OutlanderGrid
- [x] Excel Export
- [x] PDF Export
- [x] Search
- [x] Filtering
- [x] Selection
- [x] Focus
- [ ] Virtualization
- [ ] Column Reordering
- [ ] State Persistence

### Version 1.x

- [ ] OutlanderButton
- [ ] OutlanderDialog
- [ ] OutlanderToast
- [ ] OutlanderTextBox
- [ ] OutlanderSelect
- [ ] OutlanderDatePicker
- [ ] OutlanderTabs

---

## Browser Support

Outlander.Blazor supports all modern browsers:

- Microsoft Edge
- Google Chrome
- Mozilla Firefox
- Safari

---

## Compatibility

| Framework | Supported |
|-----------|-----------|
| .NET 8 | ✅ |
| .NET 9 | ✅ |
| .NET 10 | ✅ |

---

## Contributing

Contributions, bug reports, feature requests, and suggestions are welcome.

Please open an issue or submit a pull request.

---

## License

MIT License
