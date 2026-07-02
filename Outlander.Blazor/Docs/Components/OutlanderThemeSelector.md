# OutlanderThemeSelector

`OutlanderThemeSelector` is a ready-to-use component that allows users
to switch between the supported Bootstrap color modes.

The component integrates with the Outlander theme system and
automatically persists the selected theme in the browser.

------------------------------------------------------------------------

# Features

-   Bootstrap 5.3 color mode support
-   Light theme
-   Dark theme
-   System theme
-   Theme persistence using `localStorage`
-   Two-way binding support
-   Ready to integrate with `OutlanderTopMenu`

------------------------------------------------------------------------

# Requirements

Before using this component, make sure your application includes:

-   Bootstrap 5.3 or later
-   Outlander.Blazor stylesheet
-   OutlanderTheme.js

------------------------------------------------------------------------

# Theme Script

For Blazor Web App projects using `@Assets`:

``` html
<script src="@Assets["_content/Outlander.Blazor/js/OutlanderTheme.js"]"></script>
```

For projects without `@Assets`:

``` html
<script src="_content/Outlander.Blazor/js/OutlanderTheme.js"></script>
```

> The script should be loaded before the Blazor application starts
> rendering.

------------------------------------------------------------------------

# Basic Usage

``` razor
<OutlanderThemeSelector />
```

The component automatically reads the saved theme, defaults to `system`,
and updates the Bootstrap color mode.

------------------------------------------------------------------------

# Two-Way Binding

``` razor
<OutlanderThemeSelector @bind-CurrentTheme="CurrentTheme" />
```

Supported values are `light`, `dark`, and `system`.

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

# First Application Launch

If no theme exists in `localStorage`, the component defaults to `system`
and uses the operating system preference before the first render.

------------------------------------------------------------------------

# Styling Recommendations

Prefer Bootstrap CSS variables:

``` css
background-color: var(--bs-body-bg);
color: var(--bs-body-color);
```

------------------------------------------------------------------------

# Troubleshooting

Verify:

-   Bootstrap 5.3+
-   `OutlanderTheme.js` loaded
-   Bootstrap Icons available
