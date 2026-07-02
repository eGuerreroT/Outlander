# Getting Started

This guide explains how to configure **Outlander.Blazor** in a Blazor
application.

## Requirements

Before installing Outlander.Blazor, make sure your project has:

-   .NET 8, .NET 9, or .NET 10
-   Bootstrap 5.3 or later
-   A supported Blazor hosting model:
    -   Blazor Server
    -   Blazor WebAssembly
    -   Blazor Web App

## Installation

Install the NuGet package:

``` bash
dotnet add package Outlander.Blazor
```

## Register namespaces

Add the following namespaces to `_Imports.razor`:

``` razor
@using Outlander.Blazor
@using Outlander.Blazor.Components
@using Outlander.Blazor.Components.Layout
```

## Add required styles

Reference the Outlander stylesheet in your application.

For Blazor Web App projects using `@Assets`:

``` html
<link href="@Assets["_content/Outlander.Blazor/css/Outlander.Blazor.styles.css"]" rel="stylesheet" />
```

For projects without `@Assets`:

``` html
<link href="_content/Outlander.Blazor/css/Outlander.Blazor.styles.css" rel="stylesheet" />
```

## Bootstrap configuration

Outlander.Blazor requires Bootstrap Bundle 5.3 or later.

The Bootstrap bundle must be loaded before using components that depend
on Bootstrap JavaScript features.

Example:

For Blazor Web App projects using `@Assets`:

``` html
<script src="@Assets["lib/bootstrap/dist/js/bootstrap.bundle.min.js"]"></script>
```

For projects without `@Assets`:

``` html
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
```

> [!IMPORTANT]
> Outlander.Blazor requires **Bootstrap Bundle 5.3 or later**.
>
> Make sure the Bootstrap bundle script is loaded before using Outlander components:
>
> If Bootstrap is missing or an unsupported version is loaded, a runtime exception similar to the following will be thrown:
>
> ```text
> Bootstrap {bootstrapVersion} is not supported. Bootstrap 5.3 or later is required.
> ```
>
> The Bootstrap bundle is required because Outlander.Blazor relies on Bootstrap JavaScript components and Popper functionality.


> [!IMPORTANT]
> Outlander.Blazor currently uses Bootstrap Icons internally.
>
> The library automatically loads Bootstrap Icons through its stylesheet using:
>
> ```css
> @import url('https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css');
> ```
>
> Internet access to the CDN is required for icons to be displayed correctly.
>
> A self-contained icon system is planned for a future release.

The bundle includes Popper support required by Bootstrap components.

## Theme support

To enable Bootstrap dark mode support, include the Outlander theme
script:

For Blazor Web App projects using `@Assets`:

``` html
<script src="@Assets["_content/Outlander.Blazor/js/OutlanderTheme.js"]"></script>
```

For projects without `@Assets`:

``` html
<script src="_content/Outlander.Blazor/js/OutlanderTheme.js"></script>
```

The script:

-   Reads the user's selected theme from `localStorage`
-   Supports light, dark, and system preferences
-   Applies the Bootstrap `data-bs-theme` attribute
-   Allows components to initialize with the correct theme

For more information:

[Themes](Themes.md)

## Service registration

If your application uses services provided by Outlander.Blazor, register
them during startup:

``` csharp
builder.Services.AddOutlander();
```

## First component

After configuration, components can be used directly:

``` razor
<OutlanderThemeSelector />

<OutlanderNavMenu />

<OutlanderTopMenu />
```

For component-specific configuration, see:

-   [OutlanderGrid](Components/OutlanderGrid.md)
-   [OutlanderNavMenu](Components/OutlanderNavMenu.md)
-   [OutlanderTopMenu](Components/OutlanderTopMenu.md)
-   [OutlanderThemeSelector](Components/OutlanderThemeSelector.md)

## Recommended project structure

A recommended documentation structure for applications using
Outlander.Blazor:

    Docs/
     ├── GettingStarted.md
     ├── Themes.md
     ├── Layout.md
     └── Components/
         ├── OutlanderGrid.md
         ├── OutlanderNavMenu.md
         ├── OutlanderTopMenu.md
         └── OutlanderThemeSelector.md

## Troubleshooting

### Bootstrap error

If you see an error indicating that Bootstrap is not supported:

    Bootstrap {bootstrapVersion} is not supported. Bootstrap 5.3 or later is required.

Verify that:

-   Bootstrap Bundle 5.3+ is loaded
-   The script is loaded before Outlander components are initialized

### Icons are not displayed

Outlander.Blazor uses Bootstrap Icons.

Verify that your application can access the Bootstrap Icons CDN or
provide your own icon configuration in future versions.
