# OutlanderThemeSelector

The `OutlanderThemeSelector` component provides an easy way to switch between the supported Bootstrap color modes in an Outlander application.

The component integrates with Bootstrap 5.3's `data-bs-theme` attribute and the `OutlanderTheme.js` helper script to persist the selected theme and automatically restore it when the application starts.

------------------------------------------------------------------------

# Features

- Light theme
- Dark theme
- System theme
- Bootstrap 5.3 color mode support
- Automatic persistence using `localStorage`
- Automatic restoration on application startup
- Two-way binding support
- Works with Blazor Server, WebAssembly and Blazor Web App

------------------------------------------------------------------------

# Requirements

The component requires the Outlander theme support script.

For Blazor Web App projects using `@Assets`:

```html
<script src="@Assets["_content/Outlander.Blazor/js/OutlanderTheme.js"]"></script>
```

For projects without `@Assets`:

```html
<script src="_content/Outlander.Blazor/js/OutlanderTheme.js"></script>
```

> The script should be loaded before the application starts.

------------------------------------------------------------------------

# Basic Usage

```razor
<OutlanderThemeSelector />
```

By default, the component starts using the **System** theme.

------------------------------------------------------------------------

# Two-way Binding

The current theme can be synchronized with the parent component.

```razor
<OutlanderThemeSelector
    @bind-CurrentTheme="CurrentTheme" />

<p>Current theme: @CurrentTheme</p>

@code
{
    private string CurrentTheme = "system";
}
```

Whenever the user selects another option, the bound value is automatically updated.

------------------------------------------------------------------------

# EventCallback

``` razor
<OutlanderThemeSelector
    CurrentTheme="@CurrentTheme"
    CurrentThemeChanged="OnThemeChanged" />
```

``` csharp
private Task OnThemeChanged(string theme)
{
    CurrentTheme = theme;
    return Task.CompletedTask;
}
```

------------------------------------------------------------------------

# Integration with OutlanderTopMenu

``` razor
<OutlanderTopMenu ShowThemeSelector="true" />
```

------------------------------------------------------------------------

# Available Themes

| Value | Description |
|-------|-------------|
| `light` | Forces the light theme. |
| `dark` | Forces the dark theme. |
| `system` | Uses the operating system preference. |

------------------------------------------------------------------------

# Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `CurrentTheme` | `string` | `"system"` | Gets or sets the currently selected theme. Valid values are `light`, `dark` and `system`. |
| `CurrentThemeChanged` | `EventCallback<string>` | — | Raised whenever the selected theme changes. |

------------------------------------------------------------------------

# Theme Persistence

The selected theme is automatically stored in the browser using `localStorage`.

The stored value is restored when the application starts, so users do not need to select their preferred theme every time they open the application.

If no theme exists in `localStorage`, the component defaults to `system`
and uses the operating system preference before the first render.

No additional configuration is required.

------------------------------------------------------------------------

# Bootstrap Integration

Outlander uses the Bootstrap 5.3 color mode system.

Internally the component updates the document attribute:

```html
<html data-bs-theme="dark">
```

Possible values are:

- `light`
- `dark`

When the selected mode is `system`, the helper script automatically determines the appropriate value based on the user's operating system preference.

------------------------------------------------------------------------

# Using the Theme in Your Application

Bootstrap automatically adapts its components according to the active color mode.

Custom components can also use the current theme through CSS variables.

Example:

```css
.my-card
{
    background: var(--bs-body-bg);
    color: var(--bs-body-color);
    border-color: var(--bs-border-color);
}
```

Using Bootstrap variables ensures your custom components automatically support both light and dark themes.

------------------------------------------------------------------------

# Notes

- Requires Bootstrap 5.3 or later.
- Requires `OutlanderTheme.js`.
- The selected theme is automatically persisted.
- Theme changes are applied immediately without reloading the page.
- Fully compatible with Bootstrap's native color mode support.

------------------------------------------------------------------------

# Troubleshooting

Verify:

-   Bootstrap 5.3+
-   `OutlanderTheme.js` loaded
-   Bootstrap Icons available
  - 
------------------------------------------------------------------------

# Related Documentation

- [Getting Started](../GettingStarted.md)
- [Themes](../Themes.md)
- [OutlanderTopMenu](OutlanderTopMenu.md)
