# OutlanderLayout

`OutlanderLayout` is a layout shell component that combines top navigation,
side navigation, and main content for modern business applications.

It is designed to work seamlessly with `OutlanderNavMenu`,
`OutlanderTopMenu`, and `OutlanderThemeSelector`.

------------------------------------------------------------------------

# Features

-   Complete application shell
-   Integrated top and side navigation
-   Responsive desktop and mobile behavior
-   Collapsible side navigation
-   Mobile drawer support
-   Content area for routed pages
-   Theme-ready structure
-   Bootstrap 5.3 compatible

------------------------------------------------------------------------

# Basic Usage

``` razor
<OutlanderLayout
    BrandText="Outlander"
    Groups="@MenuGroups">
    <Body>
        @Body
    </Body>
</OutlanderLayout>
```

------------------------------------------------------------------------

# Layout Structure

The layout typically includes:

-   Top menu area
-   Side navigation (NavMenu)
-   Main content container
-   Optional footer/header actions

This structure helps standardize navigation and page composition across
the application.

------------------------------------------------------------------------

# Menu Integration

`OutlanderLayout` is designed to consume navigation groups and footer
items used by `OutlanderNavMenu`.

``` csharp
private IEnumerable<OutlanderNavMenuMenuGroup> MenuGroups =
[
    new()
    {
        Title = "Operations",
        Items =
        [
            new() { Text = "Dashboard", Icon = "bi-speedometer2", Url = "/" },
            new() { Text = "Orders", Icon = "bi-cart", Url = "/orders" }
        ]
    }
];
```

------------------------------------------------------------------------

# Responsive Behavior

The layout automatically adapts between desktop and mobile modes.

Desktop:

-   Side navigation can be expanded or collapsed

Mobile:

-   Side navigation opens as a drawer
-   Top menu remains the primary entry point

------------------------------------------------------------------------

# Parameters

| Parameter            | Description |
|---------------------|-------------|
| `BrandText`         | Application title displayed in navigation |
| `Groups`            | Main navigation menu groups |
| `FooterItems`       | Footer navigation items |
| `BreakPointForMobile` | Mobile breakpoint in pixels |
| `Body`              | Main content fragment rendered by the layout |

------------------------------------------------------------------------

# Recommendations

Use `OutlanderLayout` as the main layout in `App.razor` or a custom
layout component to keep navigation and content composition consistent.

------------------------------------------------------------------------

# Troubleshooting

## Layout does not render menu correctly

Verify that:

-   `Groups` contains valid items
-   Bootstrap styles are loaded
-   Layout CSS is included in the application

## Mobile menu does not open

Verify that:

-   Bootstrap JavaScript bundle is loaded
-   Bound state values are updated when toggling menu