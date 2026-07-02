# OutlanderLayout

`OutlanderLayout` is the main layout component provided by Outlander.Blazor for building modern business applications.

It provides a complete application shell composed of a navigation area, a top bar, a content area, and an optional footer.

The component is designed to work seamlessly with `OutlanderNavMenu`, `OutlanderTopMenu`, and `OutlanderThemeSelector`, but any custom Blazor content can also be used.

------------------------------------------------------------------------

# Features

- Complete application shell
- Navigation area
- Top navigation bar
- Main content area
- Optional footer
- Responsive-ready
- Works with any Blazor component
- Fully compatible with OutlanderNavMenu
- Fully compatible with OutlanderTopMenu
- Fully compatible with OutlanderThemeSelector
- Supports Blazor Server, WebAssembly and Blazor Web App

------------------------------------------------------------------------

# Basic Usage

```razor
<OutlanderLayout>

    <NavMenu>
        <OutlanderNavMenu
            @bind-MobileMenuOpen="mobileMenuOpen"
            @bind-NavMenuCollapsed="navMenuCollapsed"
            Groups="NavMenuGroups" />
    </NavMenu>

    <TopMenu>
        <OutlanderTopMenu
            @bind-MobileMenuOpen="mobileMenuOpen"
            @bind-NavMenuCollapsed="navMenuCollapsed"
            ShowNavMenuToggle="true"
            ShowThemeSelector="true" />
    </TopMenu>

    <Body>
        @Body
    </Body>

</OutlanderLayout>
```

------------------------------------------------------------------------

# Complete Layout Example

```razor
<OutlanderLayout>

    <NavMenu>
        <OutlanderNavMenu
            @bind-MobileMenuOpen="mobileMenuOpen"
            @bind-NavMenuCollapsed="navMenuCollapsed"
            BrandText="Admin Template"
            Groups="NavMenuGroups"
            FooterItems="NavMenuFooterItems" />
    </NavMenu>

    <TopMenu>
        <OutlanderTopMenu
            @bind-MobileMenuOpen="mobileMenuOpen"
            @bind-NavMenuCollapsed="navMenuCollapsed"
            ShowNavMenuToggle="true"
            ShowThemeSelector="true"
            CollapseActionsToOffcanvasOnMobile="true"
            ActionItems="TopActions" />
    </TopMenu>

    <Body>
        @Body
    </Body>

    <Footer>
        <span>© 2026 My Company</span>
        <span>Outlander.Blazor v1.0.1</span>
    </Footer>

</OutlanderLayout>
```

------------------------------------------------------------------------

# Layout Structure

The component is composed of four independent regions.

```
+------------------------------------------------------+
|                     Top Menu                         |
+-----------+------------------------------------------+
|           |                                          |
|           |                                          |
| Nav Menu  |              Body                        |
|           |                                          |
|           |                                          |
+-----------+------------------------------------------+
|                    Footer (Optional)                 |
+------------------------------------------------------+
```

Each section accepts any Blazor content through `RenderFragment`.

------------------------------------------------------------------------

# Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `NavMenu` | `RenderFragment` | — | Content rendered in the navigation area. Typically an `OutlanderNavMenu`. |
| `TopMenu` | `RenderFragment` | — | Content rendered in the application header. Typically an `OutlanderTopMenu`. |
| `Body` | `RenderFragment` | — | Main application content. Usually the routed page content. |
| `Footer` | `RenderFragment` | — | Optional footer displayed at the bottom of the layout. |

------------------------------------------------------------------------

# Using Custom Components

Although OutlanderLayout is optimized for Outlander components, any Blazor component can be used.

```razor
<OutlanderLayout>

    <NavMenu>
        <MyCustomMenu />
    </NavMenu>

    <TopMenu>
        <MyToolbar />
    </TopMenu>

    <Body>
        <Dashboard />
    </Body>

</OutlanderLayout>
```

------------------------------------------------------------------------

# Application Shell

Unlike the traditional Blazor layout model based on `LayoutComponentBase`, `OutlanderLayout` is implemented as a reusable shell component.

Each layout section is exposed through a `RenderFragment`, allowing it to be used not only as an application layout, but also as a reusable container inside any page or component.

This approach provides greater flexibility when building dashboards, portals, administration systems, or applications with multiple layout variations.

------------------------------------------------------------------------

# Responsive Behavior

`OutlanderLayout` itself does not impose responsive behavior.

Instead, it delegates responsiveness to the components hosted inside it, such as:

- `OutlanderNavMenu`
- `OutlanderTopMenu`

This separation keeps the layout simple while allowing each component to manage its own responsive logic.

------------------------------------------------------------------------

# Recommended Composition

The recommended application structure is:

```
OutlanderLayout
│
├── OutlanderNavMenu
├── OutlanderTopMenu
│      └── OutlanderThemeSelector
│
├── Routed Page
│
└── Footer (optional)
```

This composition provides a complete enterprise application shell with minimal configuration.

------------------------------------------------------------------------

# Notes

- Works with any Blazor hosting model.
- Supports Blazor Server.
- Supports Blazor WebAssembly.
- Supports Blazor Web App.
- Footer is optional.
- Any Blazor component can be hosted inside each section.

------------------------------------------------------------------------

# Related Documentation

- [Getting Started](../GettingStarted.md)
- [Layout](../Layout.md)
- [OutlanderNavMenu](OutlanderNavMenu.md)
- [OutlanderTopMenu](OutlanderTopMenu.md)
- [OutlanderThemeSelector](OutlanderThemeSelector.md)