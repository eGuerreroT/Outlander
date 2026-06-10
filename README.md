
<p align="center">
  <img src="Assets/Social_Media.png" width="80%" />
</p>

<h1 align="center">Outlander.Blazor</h1>

<h2 align="center">
  Build Business Applications Faster with Blazor
</h2>

Outlander.Blazor is a modern component library for Blazor applications focused on productivity, performance, and enterprise scenarios.

The project provides reusable UI components designed to simplify the development of business applications such as ERP, CRM, POS, reporting systems, dashboards, and internal management platforms.

---

## Features

### OutlanderGrid

The first component included in the library is a powerful data grid with support for:

- Server-side and client-side data binding
- Sorting
- Filtering
- Global search
- Row selection
- Column customization
- Footer summaries
- Excel export
- PDF export
- Responsive design
- Bootstrap 5 integration
- Blazor Server support
- Blazor WebAssembly support

---

## Installation

Install the package from NuGet:

```bash
dotnet add package Outlander.Blazor
```

---

## Registration

Add the namespace to your `_Imports.razor`:

```razor
@using Outlander.Blazor
```

If required by future components:

```csharp
builder.Services.AddOutlander();
```

---

## Basic Example

```razor
<OutlanderGrid Items="@Employees">

    <OutlanderGridDataColumn
        Field="@nameof(Employee.Name)"
        Title="Name" />

    <OutlanderGridDataColumn
        Field="@nameof(Employee.Email)"
        Title="Email" />

</OutlanderGrid>
```

---

## Example Model

```csharp
public class Employee
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
```

---

## Roadmap

### Version 0.x

- [x] OutlanderGrid
- [x] Excel Export
- [x] PDF Export
- [x] Search
- [x] Filtering
- [x] Selection
- [x] Focus
- [ ] Virtualization
- [ ] Column Reordering
- [ ] State Persistence

### Version 1.x

- [ ] OutlanderButton
- [ ] OutlanderDialog
- [ ] OutlanderToast
- [ ] OutlanderTextBox
- [ ] OutlanderSelect
- [ ] OutlanderDatePicker
- [ ] OutlanderTabs

---

## Browser Support

Outlander.Blazor supports all modern browsers:

- Microsoft Edge
- Google Chrome
- Mozilla Firefox
- Safari

---

## Compatibility

| Framework | Supported |
|-----------|-----------|
| .NET 8 | ✅ |
| .NET 9 | ✅ |
| .NET 10 | ✅ |

---

## Contributing

Contributions, bug reports, feature requests, and suggestions are welcome.

Please open an issue or submit a pull request.

---

## License

MIT License
