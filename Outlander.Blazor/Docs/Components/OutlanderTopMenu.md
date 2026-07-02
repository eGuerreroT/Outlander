# OutlanderTopMenu

`OutlanderTopMenu` is a responsive top navigation component designed for
application headers in business solutions.

It provides layout slots for left and right content, action buttons,
notification badges, and integration with `OutlanderNavMenu` and
`OutlanderThemeSelector`.

------------------------------------------------------------------------

# Features

-   Responsive top bar
-   Navigation toggle button
-   Left and right content areas
-   Action buttons
-   Notification badges
-   Mobile offcanvas actions
-   Theme selector integration
-   Bootstrap 5.3 compatible
-   Bootstrap Icons integration

------------------------------------------------------------------------

# Basic Usage

``` razor
<OutlanderTopMenu
    ShowNavToggle="true"
    OnNavToggle="HandleNavToggle">
    <LeftContent>
        <span class="fw-semibold">Dashboard</span>
    </LeftContent>
    <RightContent>
        <OutlanderThemeSelector />
    </RightContent>
</OutlanderTopMenu>
```

------------------------------------------------------------------------

# Navigation Toggle

Use the top menu to control the side navigation state.

``` razor
<OutlanderTopMenu
    ShowNavToggle="true"
    OnNavToggle="HandleNavToggle" />
```

The parent component can wire this event to toggle:

-   `@bind-NavMenuCollapsed`
-   `@bind-MobileMenuOpen`

------------------------------------------------------------------------

# Content Slots

The component supports templated regions for customization.

``` razor
<OutlanderTopMenu>
    <LeftContent>
        <span>Sales Portal</span>
    </LeftContent>

    <RightContent>
        <button class="btn btn-outline-secondary btn-sm">Refresh</button>
        <OutlanderThemeSelector />
    </RightContent>
</OutlanderTopMenu>
```

------------------------------------------------------------------------

# Action Buttons

Header actions can be rendered as buttons with optional badges.

Typical usage includes:

-   Notifications
-   Tasks
-   Messages
-   User profile actions

------------------------------------------------------------------------

# Mobile Behavior

In smaller viewports, actions can be collapsed into an offcanvas area
to keep the top bar clean and accessible.

Recommended:

-   Keep primary actions visible
-   Move secondary actions into mobile actions area

------------------------------------------------------------------------

# Parameters

| Parameter         | Description |
|------------------|-------------|
| `ShowNavToggle`  | Shows the navigation toggle button |
| `OnNavToggle`    | Event callback fired when toggle is clicked |
| `LeftContent`    | Custom left header content |
| `RightContent`   | Custom right header content |
| `Class`          | Additional CSS classes for styling |

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

-   `OnNavToggle` is wired in the parent component
-   Parent state (`NavMenuCollapsed` / `MobileMenuOpen`) is updated correctly

## Actions are not visible on mobile

Verify:

-   Bootstrap 5.3+ is loaded
-   Offcanvas styles/scripts are available