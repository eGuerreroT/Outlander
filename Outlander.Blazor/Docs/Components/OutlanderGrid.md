# OutlanderGrid

`OutlanderGrid` is an enterprise-ready data grid component for Blazor
applications focused on productivity and business workflows.

It supports rich data operations including sorting, filtering, searching,
selection, export, and responsive rendering.

------------------------------------------------------------------------

# Features

-   Server-side and client-side data binding
-   Sorting
-   Filtering
-   Global search
-   Row selection
-   Column customization
-   Footer summaries
-   Excel export
-   PDF export
-   Responsive design
-   Bootstrap 5.3 integration
-   Dark theme support

------------------------------------------------------------------------

# Basic Usage

``` razor
<OutlanderGrid TItem="OrderDto"
               Items="@Orders"
               Pageable="true"
               Sortable="true"
               Filterable="true" />
```

------------------------------------------------------------------------

# Columns

Define columns using child content.

``` razor
<OutlanderGrid TItem="OrderDto" Items="@Orders">
    <OutlanderGridDataColumn Field="Id" Title="Order #" />
    <OutlanderGridDataColumn Field="CustomerName" Title="Customer" />
    <OutlanderGridDataColumn Field="Total" Title="Total" />
</OutlanderGrid>
```

------------------------------------------------------------------------

# Search and Filtering

Enable global search and per-column filtering to improve data discovery.

Typical configuration:

-   Search box enabled
-   Column filter UI enabled
-   Combined search + filter behavior

------------------------------------------------------------------------

# Selection

Selection features support row-based workflows such as:

-   Bulk actions
-   Batch processing
-   Context operations

Use selection settings/columns according to your scenario.

------------------------------------------------------------------------

# Export

`OutlanderGrid` supports exporting current dataset views to:

-   Excel
-   PDF

Useful for reporting and business distribution workflows.

------------------------------------------------------------------------

# Parameters

| Parameter      | Description |
|---------------|-------------|
| `Items`       | Data source for rendering rows |
| `TItem`       | Model type of each row |
| `Sortable`    | Enables sorting behavior |
| `Filterable`  | Enables filtering behavior |
| `Searchable`  | Enables global search |
| `Pageable`    | Enables paging |
| `Selectable`  | Enables row selection |

------------------------------------------------------------------------

# Recommendations

Use `OutlanderGrid` for high-density business data screens such as:

-   Orders
-   Customers
-   Inventory
-   Financial reports

------------------------------------------------------------------------

# Troubleshooting

## Grid renders but interactions do not work

Verify:

-   Required scripts are loaded
-   Component parameters are configured correctly
-   Data source is not null

## Export buttons do not generate files

Verify that:

-   Export settings are enabled
-   Export dependencies are correctly registered