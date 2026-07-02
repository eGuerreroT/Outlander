# OutlanderTopMenu

`OutlanderTopMenu` is a responsive top navigation component designed for
application headers in business solutions.

It provides layout slots for left and right content, action buttons,
notification badges, and integration with `OutlanderNavMenu` and
`OutlanderThemeSelector`.

------------------------------------------------------------------------

# Features

-   Responsive top bar
-   Navigation menu toggle button
-   Left and right content areas
-   Action buttons
-   Notification badges
-   Mobile offcanvas actions
-   Theme selector integration
-   Sticky mode support
-   Bootstrap 5.3 compatible
-   Bootstrap Icons integration

------------------------------------------------------------------------

# Basic Usage

``` razor
<OutlanderTopMenu
    ShowNavMenuToggle="true"
    ShowThemeSelector="true"
    @bind-MobileMenuOpen="MobileMenuOpen"
    @bind-NavMenuCollapsed="NavMenuCollapsed">
    <LeftContent>
        <span class="fw-semibold">Dashboard</span>
    </LeftContent>
</OutlanderTopMenu>
```

------------------------------------------------------------------------

# Navigation Toggle

Use the top menu to control the side navigation state.

``` razor
<OutlanderTopMenu
    ShowNavMenuToggle="true"
    @bind-MobileMenuOpen="MobileMenuOpen"
    @bind-NavMenuCollapsed="NavMenuCollapsed" />
```

The component automatically:

-   Toggles the mobile menu on small screens
-   Toggles the collapsed navigation state on desktop
-   Persists the collapsed menu state using browser storage

------------------------------------------------------------------------

# Content Slots

The component supports templated regions for customization.

``` razor
<OutlanderTopMenu ShowThemeSelector="true">
    <LeftContent>
        <span>Sales Portal</span>
    </LeftContent>

    <RightContent>
        <button class="btn btn-outline-secondary btn-sm">Refresh</button>
    </RightContent>
</OutlanderTopMenu>
```

------------------------------------------------------------------------

# Action Items

When `RightContent` is not provided, the component can render predefined
action items using `ActionItems`.

``` csharp
private IEnumerable<OutlanderTopMenuActionItem> Actions =
[
    new()
    {
        Title = "Notifications",
        Icon = "bi-bell",
        BadgeText = "3",
        OnClick = EventCallback.Factory.Create(this, OpenNotifications),
        KeepVisibleOnMobile = true
    },
    new()
    {
        Title = "Profile",
        Icon = "bi-person-circle",
        OnClick = EventCallback.Factory.Create(this, OpenProfile)
    }
];
```

``` razor
<OutlanderTopMenu
    ActionItems="@Actions"
    ShowThemeSelector="true" />
```

------------------------------------------------------------------------

# Mobile Behavior

In smaller viewports, actions can be collapsed into an offcanvas area
to keep the top bar clean and accessible.

This behavior is controlled by:

-   `CollapseActionsToOffcanvasOnMobile`
-   `IncludeLeftContentInMobileOffcanvas`
-   `MobileOffcanvasTitle`

------------------------------------------------------------------------

# Sticky Mode

Use `Sticky="true"` to keep the top menu visible while scrolling.

``` razor
<OutlanderTopMenu
    Sticky="true"
    ShowNavMenuToggle="true"
    ShowThemeSelector="true" />
```

------------------------------------------------------------------------

# Parameters

| Parameter                              | Description |
|----------------------------------------|-------------|
| `BreakPointForMobile`                  | Breakpoint used to switch to mobile mode |
| `ShowThemeSelector`                    | Displays the theme selector |
| `ShowNavMenuToggle`                    | Displays the navigation toggle button |
| `Sticky`                               | Keeps the top menu visible during scroll |
| `CollapseActionsToOffcanvasOnMobile`   | Collapses action items into a mobile offcanvas panel |
| `IncludeLeftContentInMobileOffcanvas`  | Also renders left content inside the mobile offcanvas |
| `MobileOffcanvasTitle`                 | Title displayed in the mobile offcanvas panel |
| `LeftContent`                          | Custom left header content |
| `RightContent`                         | Custom right header content |
| `ActionItems`                          | Predefined action items rendered when `RightContent` is not provided |
| `MobileMenuOpen`                       | Controls the mobile navigation menu open state |
| `NavMenuCollapsed`                     | Controls the desktop navigation collapsed state |

------------------------------------------------------------------------

# Recommendations

Use `OutlanderTopMenu` together with:

-   OutlanderLayout
-   OutlanderNavMenu
-   OutlanderThemeSelector

to build complete responsive application shells.

------------------------------------------------------------------------

# Troubleshooting

## Toggle button does not collapse nav menu

Verify that:

-   `ShowNavMenuToggle` is enabled
-   The parent component binds `@bind-MobileMenuOpen`
-   The parent component binds `@bind-NavMenuCollapsed`

## Actions are not visible on mobile

Verify:

-   Bootstrap 5.3+ is loaded
-   Offcanvas styles/scripts are available
-   `CollapseActionsToOffcanvasOnMobile` is enabled when needed

## Theme selector is not displayed

Verify that:

-   `ShowThemeSelector="true"` is configured