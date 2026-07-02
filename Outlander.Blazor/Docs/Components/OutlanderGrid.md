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

# Basic Usage

```razor
<OutlanderGrid TItem="ServerItem"
               Items="@Servers">

    <Columns>

        <OutlanderGridDataColumn
            TItem="ServerItem"
            FieldName="Name" />

        <OutlanderGridDataColumn
            TItem="ServerItem"
            FieldName="Provider" />

        <OutlanderGridDataColumn
            TItem="ServerItem"
            FieldName="Status" />

    </Columns>

</OutlanderGrid>
```

------------------------------------------------------------------------

# Settings-based Configuration (Recommended)

Most grid behavior can be configured through the `Settings` section.

```razor
<OutlanderGrid
    TItem="ServerItem"
    Items="@Servers">

    <Settings>

        <OutlanderGridSearchSettings
            Show="true" />

        <OutlanderGridFilterSettings
            Show="true" />

        <OutlanderGridFooterSettings />

        <OutlanderGridExportSettings
            ShowButtons="true" />

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

    <button class="btn btn-primary">

        Import

    </button>

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

The search is automatically applied across all searchable columns.

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
<OutlanderGridSelectionColumn

    AllowSelectAllItems="true" />
```

The selected rows can be synchronized using two-way binding.

```razor
<OutlanderGrid

    @bind-SelectedItems="SelectedServers" />
```

------------------------------------------------------------------------

# Focused Row

The focused row can also be synchronized.

```razor
<OutlanderGrid

    @bind-FocusedRow="FocusedServer" />
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

    <span class="fw-bold">

        @cell.Highlight(cell.Item.Name)

    </span>

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

------------------------------------------------------------------------

# Column Types

Outlander currently includes:

- OutlanderGridDataColumn
- OutlanderGridSelectionColumn

Additional column types will be added in future releases.

------------------------------------------------------------------------

# Parameters

The grid exposes many configuration parameters.

The most commonly used are:

| Parameter | Description |
|------------|-------------|
| Items | Data source. |
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

# Performance Notes

The grid is optimized for:

- Large datasets
- Blazor Server
- Blazor WebAssembly
- Minimal rendering
- Enterprise applications

Virtualization support is planned for a future release.

------------------------------------------------------------------------

# Troubleshooting

Verify:

- Bootstrap 5.3+
- Bootstrap Bundle loaded
- Outlander.Blazor stylesheet loaded

For export functionality:

- Browser popup blocking disabled when printing
- Export buttons enabled

------------------------------------------------------------------------

# Related Documentation

- [Getting Started](../GettingStarted.md)
- [Themes](../Themes.md)
- [OutlanderNavMenu](OutlanderNavMenu.md)
- [OutlanderTopMenu](OutlanderTopMenu.md)