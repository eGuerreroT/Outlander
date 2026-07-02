# Themes

Outlander.Blazor includes Bootstrap 5.3 theme support through a
lightweight JavaScript initialization script.

The theme system is based on the Bootstrap color mode mechanism using
the `data-bs-theme` attribute.

## Supported themes

Outlander.Blazor supports three theme modes:

  Mode       Description
  ---------- -----------------------------------------------
  `light`    Forces the application to use the light theme
  `dark`     Forces the application to use the dark theme
  `system`   Uses the operating system preference

The selected value is persisted in the browser using `localStorage`.

## Theme initialization

Add the Outlander theme script before the application starts rendering:

For Blazor Web App projects using `@Assets`:

``` html
<script src="@Assets["_content/Outlander.Blazor/js/OutlanderTheme.js"]"></script>
```

For projects without `@Assets`:

``` html
<link href="_content/Outlander.Blazor/js/OutlanderTheme.js" rel="stylesheet" />
```

The script executes immediately and:

1.  Reads the saved theme preference.
2.  Detects the system preference when required.
3.  Applies the Bootstrap theme attribute:

``` html
<html data-bs-theme="dark">
```

or:

``` html
<html data-bs-theme="light">
```

This avoids visual changes after the first render.

## Default behavior

If the application has never been opened before, no theme preference
exists in `localStorage`.

In this case, Outlander.Blazor uses:

    system

The browser preference is detected using:

``` javascript
window.matchMedia("(prefers-color-scheme: dark)")
```

The selected value is also stored as:

``` html
data-theme-mode="system"
```

on the root document element.

## OutlanderThemeSelector

The `OutlanderThemeSelector` component provides a ready-to-use UI for
changing themes.

Example:

``` razor
<OutlanderThemeSelector />
```

The component includes:

-   Light theme option
-   Dark theme option
-   System preference option
-   Current selection indicator
-   Browser persistence

## Using the current theme value

The component supports two-way binding:

``` razor
<OutlanderThemeSelector @bind-CurrentTheme="CurrentTheme" />

<p>Current theme: @CurrentTheme</p>

@code {
    private string CurrentTheme = "system";
}
```

Possible values:

``` text
light
dark
system
```

For component-specific configuration, see:

-   [OutlanderThemeSelector](Components/OutlanderThemeSelector.md)

## Bootstrap compatibility

The theme system requires Bootstrap 5.3 or later because dark mode
support is based on Bootstrap's native color mode implementation.

Make sure Bootstrap styles are loaded before using theme-dependent
components.

## Custom components support

Custom components should respect Bootstrap theme variables instead of
hardcoded colors.

Recommended:

``` css
.my-component {
    background-color: var(--bs-body-bg);
    color: var(--bs-body-color);
    border-color: var(--bs-border-color);
}
```

Avoid:

``` css
.my-component {
    background-color: white;
    color: black;
}
```

Using Bootstrap variables ensures compatibility with both light and dark
themes.

## Theme change events

When the theme changes, `OutlanderThemeSelector` notifies the parent
component through:

``` razor
@bind-CurrentTheme
```

Example:

``` razor
<OutlanderThemeSelector 
    CurrentTheme="@Theme"
    CurrentThemeChanged="OnThemeChanged" />
```

``` csharp
private Task OnThemeChanged(string theme)
{
    Theme = theme;

    return Task.CompletedTask;
}
```

## Recommended application layout

A common configuration is:

``` razor
<OutlanderTopMenu ShowThemeSelector="true" />

<OutlanderNavMenu />
```

The theme selector can be placed in the application header while the
theme initialization script handles the initial page load.

## Troubleshooting

### Theme flashes during startup

If the application briefly displays the wrong theme:

-   Verify that `OutlanderTheme.js` is loaded in the document head.
-   Ensure Bootstrap CSS is loaded before the application renders.
-   Avoid loading the theme script after Blazor initialization.

### Dark theme colors are not applied

Verify:

-   Bootstrap version is 5.3 or later.
-   Components use Bootstrap CSS variables.
-   The document contains:

``` html
<html data-bs-theme="dark">
```
