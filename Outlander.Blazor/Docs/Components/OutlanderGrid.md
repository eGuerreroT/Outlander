# OutlanderGrid

`OutlanderGrid` is a high-performance data grid component designed for business and enterprise applications.

It provides built-in support for filtering, sorting, searching, exporting, selection, templating and responsive layouts while keeping the API simple and Blazor-friendly.

------------------------------------------------------------------------

# Features

- Server-side and client-side data binding
- Sorting
- Filtering
- Global search
- Row selection
- Focused row
- Responsive layout
- Column customization
- Templates
- Footer summaries
- Excel export
- PDF export
- Print export
- Bootstrap 5.3 integration
- Dark theme support
- Blazor Server
- Blazor WebAssembly
- Blazor Web App

------------------------------------------------------------------------

# Data Binding Modes

OutlanderGrid supports two processing modes through `DataMode`:

- `OutlanderGridDataMode.Client` (default): filtering/search/sorting/paging are in-memory.
- `OutlanderGridDataMode.Server`: filtering/search/sorting/paging are resolved by your data provider through callbacks.

------------------------------------------------------------------------

# Basic Usage (Client-side)

```razor
<OutlanderGrid TItem="ServerItem"
               Items="@Servers">

    <Columns>
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Name" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Provider" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Status" />
    </Columns>

</OutlanderGrid>
```

------------------------------------------------------------------------

# Server-side Binding

Use `DataMode="OutlanderGridDataMode.Server"` and provide `OnRead`.

```razor
<OutlanderGrid TItem="ServerItem"
               DataMode="OutlanderGridDataMode.Server"
               OnRead="LoadServersAsync"
               ShowSearchBox="true"
               ShowFilterRow="true"
               AllowSort="true"
               ShowPageSizeSelector="true">
    <Columns>
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Name" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Provider" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Status" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="MemoryGb" FilterMode="GridFilterMode.Range" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="CreatedAt" FilterMode="GridFilterMode.Date" />
    </Columns>
</OutlanderGrid>
```

`OnRead` receives `OutlanderGridDataRequest` and must return `OutlanderGridDataResult<TItem>`:

```csharp
private async Task<OutlanderGridDataResult<ServerItem>> LoadServersAsync(OutlanderGridDataRequest request)
{
    // Apply search/filter/sort/paging in your provider (EF, API, etc.)
    // Return page items + filtered total count.
}
```

------------------------------------------------------------------------

# Server-side Export Scope

In server mode, export can be configured to use:

- current page rows
- all filtered rows

Use:

- `ServerExportScope` (`CurrentPage` or `AllFiltered`)
- `OnReadExport` callback for full filtered exports

```razor
<OutlanderGrid TItem="ServerItem"
               DataMode="OutlanderGridDataMode.Server"
               OnRead="LoadServersAsync"
               OnReadExport="LoadServersForExportAsync"
               ServerExportScope="OutlanderGridExportScope.AllFiltered"
               ShowExportButtons="true">
    <Columns>
        ...
    </Columns>
</OutlanderGrid>
```

```csharp
private async Task<IReadOnlyList<ServerItem>> LoadServersForExportAsync(OutlanderGridExportRequest request)
{
    // Apply current search/filter/sort criteria
    // Return the rows to export
}
```

------------------------------------------------------------------------

# Refreshing Data Programmatically

`OutlanderGrid` exposes a public `RefreshAsync()` method.

Use it through `@ref` when you need manual reloads after external actions (imports, sync jobs, etc.):

```razor
<OutlanderGrid TItem="ServerItem"
               @ref="gridRef"
               DataMode="OutlanderGridDataMode.Server"
               OnRead="LoadServersAsync">
    <Columns>...</Columns>
</OutlanderGrid>

<button class="btn btn-primary" @onclick="RefreshGridAsync">Refresh</button>

@code {
    private OutlanderGrid<ServerItem>? gridRef;

    private async Task RefreshGridAsync()
    {
        if (gridRef is not null)
            await gridRef.RefreshAsync();
    }
}
```

------------------------------------------------------------------------

# Settings-based Configuration (Recommended)

Most grid behavior can be configured through the `Settings` section.

```razor
<OutlanderGrid TItem="ServerItem" Items="@Servers">

    <Settings>
        <OutlanderGridSearchSettings Show="true" />
        <OutlanderGridFilterSettings Show="true" />
        <OutlanderGridFooterSettings />
        <OutlanderGridExportSettings ShowButtons="true" />
        <OutlanderGridSelectionSettings />
    </Settings>

    <Columns>
        ...
    </Columns>

</OutlanderGrid>
```

Using settings keeps the grid markup cleaner and groups related configuration together.

------------------------------------------------------------------------

# Toolbar

Additional actions can be placed in the built-in toolbar.

```razor
<ToolbarTemplate>
    <button class="btn btn-primary">Import</button>
</ToolbarTemplate>
```

------------------------------------------------------------------------

# Searching

Enable the built-in search box.

```razor
<OutlanderGridSearchSettings
    Show="true"
    NullText="Search..."
    ParseMode="GridSearchTextParseMode.GroupWordsByAnd" />
```

The search is automatically applied across all searchable columns (client mode) or forwarded in request payload (server mode).

------------------------------------------------------------------------

# Filtering

Each column can define its own filter behavior.

```razor
<OutlanderGridDataColumn
    FieldName="MemoryGb"
    FilterMode="GridFilterMode.Range" />
```

Custom filter templates are also supported.

```razor
<FilterTemplate Context="filter">
    <input class="form-control"
           value="@filter.Value"
           @oninput="e => filter.SetValue(e.Value?.ToString())" />
</FilterTemplate>
```

------------------------------------------------------------------------

# Sorting

Sorting can be enabled globally or configured per column.

```razor
<OutlanderGridDataColumn
    FieldName="Name"
    AllowSort="true"
    SortOrder="GridColumnSortOrder.Ascending" />
```

------------------------------------------------------------------------

# Selection

Selection is supported through the built-in selection column.

```razor
<OutlanderGridSelectionColumn AllowSelectAllItems="true" />
```

The selected rows can be synchronized using two-way binding.

```razor
<OutlanderGrid @bind-SelectedItems="SelectedServers" />
```

------------------------------------------------------------------------

# Focused Row

The focused row can also be synchronized.

```razor
<OutlanderGrid @bind-FocusedRow="FocusedServer" />
```

------------------------------------------------------------------------

# Templates

The grid provides several customization points.

Supported templates include:

- ToolbarTemplate
- CellTemplate
- FilterTemplate

Example:

```razor
<CellTemplate Context="cell">
    <span class="fw-bold">@cell.Highlight(cell.Item.Name)</span>
</CellTemplate>
```

------------------------------------------------------------------------

# Exporting

Built-in exporting supports:

- Excel
- PDF
- Print

Example:

```razor
<OutlanderGridExportSettings
    ShowButtons="true"
    AllowExcel="true"
    AllowPdf="true"
    AllowPrint="true"
    FileName="Servers"
    Title="Servers List" />
```

For server mode, combine with `OnReadExport` and `ServerExportScope` for predictable export behavior.

------------------------------------------------------------------------

# Column Types

Outlander currently includes:

- OutlanderGridDataColumn
- OutlanderGridSelectionColumn

Additional column types will be added in future releases.

------------------------------------------------------------------------

# Key Types

- `OutlanderGridDataMode`
- `OutlanderGridDataRequest`
- `OutlanderGridDataResult<TItem>`
- `OutlanderGridExportRequest`
- `OutlanderGridExportScope`

------------------------------------------------------------------------

# Parameters (Common)

| Parameter | Description |
|------------|-------------|
| Items | Data source for client mode. Optional in server mode. |
| DataMode | Selects client or server processing mode. |
| OnRead | Callback used in server mode to fetch page data. |
| OnReadExport | Callback used in server mode for export datasets. |
| ServerExportScope | Export strategy in server mode (`CurrentPage`/`AllFiltered`). |
| PageSize | Number of rows per page. |
| EmptyText | Message shown when no records exist. |
| ShowColumnChooser | Displays the column chooser. |
| AllowSort | Enables sorting. |
| AllowFocusedRow | Enables focused row support. |
| AllowHotTrackRow | Enables row hover highlighting. |
| ShowFilterRow | Displays the filter row. |
| ShowSearchBox | Displays the search box. |
| ShowExportButtons | Displays export buttons. |

------------------------------------------------------------------------

# Common Scenarios

Typical implementations include:

- Basic grid
- CRUD applications
- Reporting dashboards
- Administration portals
- ERP systems
- CRM applications
- Server-side paging
- Read-only reporting

------------------------------------------------------------------------

# EF Core End-to-End Sample

This sample uses `DataMode.Server` with Entity Framework Core to resolve paging, filtering, sorting and export from the database.

```razor
<OutlanderGrid TItem="ServerItem"
               DataMode="OutlanderGridDataMode.Server"
               OnRead="LoadServersAsync"
               OnReadExport="LoadServersForExportAsync"
               ServerExportScope="OutlanderGridExportScope.AllFiltered"
               ShowSearchBox="true"
               ShowFilterRow="true"
               AllowSort="true"
               ShowExportButtons="true">
    <Columns>
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Name" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Provider" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="Status" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="MemoryGb" FilterMode="GridFilterMode.Range" />
        <OutlanderGridDataColumn TItem="ServerItem" FieldName="CreatedAt" FilterMode="GridFilterMode.Date" />
    </Columns>
</OutlanderGrid>
```

```csharp
private async Task<OutlanderGridDataResult<ServerItem>> LoadServersAsync(OutlanderGridDataRequest request)
{
    IQueryable<ServerEntity> query = Db.Servers.AsNoTracking();

    // Apply search/filter/sort...
    // Count total, then Skip/Take for requested page.

    var totalCount = await query.CountAsync();

    var pageItems = await query
        .Skip((request.Page - 1) * request.PageSize)
        .Take(request.PageSize)
        .Select(x => new ServerItem
        {
            Name = x.Name,
            Provider = x.Provider,
            Status = x.Status,
            Ip = x.Ip,
            Cluster = x.Cluster,
            OperatingSystem = x.OperatingSystem,
            CreatedAt = x.CreatedAt,
            MemoryGb = x.MemoryGb
        })
        .ToListAsync();

    return new OutlanderGridDataResult<ServerItem>
    {
        Items = pageItems,
        TotalCount = totalCount
    };
}

private async Task<IReadOnlyList<ServerItem>> LoadServersForExportAsync(OutlanderGridExportRequest request)
{
    IQueryable<ServerEntity> query = Db.Servers.AsNoTracking();

    // Apply same search/filter/sort criteria used by OnRead...

    return await query
        .Select(x => new ServerItem
        {
            Name = x.Name,
            Provider = x.Provider,
            Status = x.Status,
            Ip = x.Ip,
            Cluster = x.Cluster,
            OperatingSystem = x.OperatingSystem,
            CreatedAt = x.CreatedAt,
            MemoryGb = x.MemoryGb
        })
        .ToListAsync();
}
```

------------------------------------------------------------------------

# Performance Notes

The grid is optimized for:

- Large datasets
- Blazor Server
- Blazor WebAssembly
- Minimal rendering
- Enterprise applications

For large datasets, prefer `DataMode.Server` with provider-side paging/filtering/sorting.

Virtualization support is planned for a future release.

------------------------------------------------------------------------

# Troubleshooting

Verify:

- Bootstrap 5.3+
- Bootstrap Bundle loaded
- Outlander.Blazor stylesheet loaded

For server mode:

- `OnRead` must be provided when `DataMode="Server"`
- return accurate `TotalCount` to keep pager/footer correct

For export functionality:

- Browser popup blocking disabled when printing
- Export buttons enabled
- In server mode, provide `OnReadExport` when exporting full filtered datasets

------------------------------------------------------------------------

# Related Documentation

- [Getting Started](../GettingStarted.md)
- [Themes](../Themes.md)
- [OutlanderNavMenu](OutlanderNavMenu.md)
- [OutlanderTopMenu](OutlanderTopMenu.md)