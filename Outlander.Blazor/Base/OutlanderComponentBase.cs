using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Outlander.Blazor.Base;

public abstract class OutlanderComponentBase : ComponentBase
{
    [Inject]
    protected IJSRuntime JS { get; set; } = default!;
}
