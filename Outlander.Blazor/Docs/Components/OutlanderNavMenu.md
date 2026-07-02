# OutlanderNavMenu

`OutlanderNavMenu` is a responsive navigation component designed for
business applications.

It supports desktop and mobile layouts, nested menu structures,
collapsible navigation, flyout submenus, and persistent menu state.

------------------------------------------------------------------------

# Features

-   Responsive navigation
-   Desktop collapsed mode
-   Mobile navigation drawer
-   Nested menu items
-   Flyout menus
-   Footer menu section
-   Persistent collapsed state
-   Bootstrap 5.3 compatible
-   Bootstrap Icons integration

------------------------------------------------------------------------

# Basic Usage

``` razor
<OutlanderNavMenu
    @bind-MobileMenuOpen="MobileMenuOpen"
    @bind-NavMenuCollapsed="NavMenuCollapsed"
    BrandText="Outlander"
    Groups="@MenuGroups" />
```

------------------------------------------------------------------------

# Two-way Binding

The component exposes two bindable properties.

``` razor
<OutlanderNavMenu
    @bind-MobileMenuOpen="MobileMenuOpen"
    @bind-NavMenuCollapsed="NavMenuCollapsed" />
```

  Property             Description
  -------------------- ---------------------------------------
  `MobileMenuOpen`     Controls the mobile drawer visibility
  `NavMenuCollapsed`   Controls the desktop collapsed state

------------------------------------------------------------------------

# Menu Structure

Navigation is organized into menu groups.

``` csharp
private IEnumerable<OutlanderNavMenuMenuGroup> MenuGroups =
[
    new()
    {
        Title = "Sales",
        Items =
        [
            new()
            {
                Text = "Orders",
                Icon = "bi-cart",
                Url = "/orders"
            }
        ]
    }
];
```

Each item supports:

-   Text
-   Icon
-   Url
-   Tooltip
-   Child items
-   Expanded by default
-   Active state

------------------------------------------------------------------------

# Nested Menus

Items may contain child items.

``` csharp
new()
{
    Text = "Administration",
    Icon = "bi-gear",
    Children =
    [
        new()
        {
            Text = "Users",
            Url="/users"
        },
        new()
        {
            Text="Roles",
            Url="/roles"
        }
    ]
}
```

Desktop mode displays expandable submenus.

Collapsed mode automatically displays flyout menus.

------------------------------------------------------------------------

# Mobile Behavior

When the viewport width is below `BreakPointForMobile`, the component
automatically switches to mobile mode.

Default breakpoint:

``` text
768
```

The mobile menu can be controlled using:

``` razor
@bind-MobileMenuOpen
```

------------------------------------------------------------------------

# Persistent Collapsed State

The desktop collapsed state is automatically persisted in the browser.

No additional code is required.

The value is restored automatically during component initialization.

------------------------------------------------------------------------

# Footer Items

Footer actions can be rendered independently.

``` razor
<OutlanderNavMenu
    FooterItems="@FooterItems" />
```

Typical usage includes:

-   Settings
-   Profile
-   Help
-   Logout

------------------------------------------------------------------------

# Tooltips

When the menu is collapsed, tooltips help identify menu items.

Tooltips are automatically initialized using Bootstrap.

Custom tooltips can be provided through the `Tooltip` property.

------------------------------------------------------------------------

# Brand Area

The header supports a custom application name.

``` razor
<OutlanderNavMenu
    BrandText="ERP System" />
```

------------------------------------------------------------------------

# Parameters

The NavMenu exposes many configuration parameters.

The most commonly used are:

  |Parameter  |Description|
  |-----------|-----------|
  |`BrandText`            | Application title |
  |`Groups`               | Main navigation groups |
  |`FooterItems`          | Footer navigation items |
  |`ShowNavMenuToggle`    | Shows the collapse button |
  |`BreakPointForMobile`  | Mobile breakpoint in pixels |

------------------------------------------------------------------------

# Recommendations

Use `OutlanderNavMenu` together with:

-   OutlanderTopMenu
-   OutlanderThemeSelector

to build complete application layouts.

------------------------------------------------------------------------

# Troubleshooting

## Menu state is not persisted

Verify that JavaScript is enabled.

The collapsed state is stored using browser local storage.

## Tooltips are not displayed

Verify:

-   Bootstrap Bundle 5.3+
-   Bootstrap JavaScript loaded
-   Bootstrap initialized correctly

## Mobile menu does not open

Verify that the parent component binds:

``` razor
@bind-MobileMenuOpen
```
