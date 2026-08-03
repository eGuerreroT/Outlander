using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem>
{
    // Drag state
    private string? _dragSourceFieldName;

    /// <summary>
    /// Enables drag-and-drop column reordering.
    /// </summary>
    [Parameter] public bool AllowColumnReorder { get; set; } = false;

    /// <summary>
    /// Fired when the visible column order changes.
    /// Emits FieldName list in final visible order.
    /// </summary>
    [Parameter] public EventCallback<IReadOnlyList<string>> ColumnOrderChanged { get; set; }

    private void EnsureColumnOrderInitialized()
    {
        for (var i = 0; i < _columns.Count; i++)
        {
            var c = _columns[i];
            if (c.OrderIndex < 0)
                c.OrderIndex = i;
        }

        NormalizeColumnOrderIndexes();
    }

    private void NormalizeColumnOrderIndexes()
    {
        var ordered = _columns
            .OrderBy(c => c.OrderIndex)
            .ThenBy(c => c.FieldName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        for (var i = 0; i < ordered.Count; i++)
            ordered[i].OrderIndex = i;
    }

    private async Task OnHeaderDropAsync(string targetFieldName)
    {
        if (!AllowColumnReorder || string.IsNullOrWhiteSpace(_dragSourceFieldName))
            return;

        var sourceFieldName = _dragSourceFieldName!;
        _dragSourceFieldName = null;

        var changed = MoveColumnRelative(sourceFieldName, targetFieldName, _dropBeforeTarget);
        ClearDragVisuals();

        if (!changed) return;

        CurrentPage = 1;
        await NotifyColumnOrderChangedAsync();
        StateHasChanged();
    }

    private bool MoveColumnRelative(string sourceFieldName, string targetFieldName, bool beforeTarget)
    {
        EnsureColumnOrderInitialized();

        var ordered = _columns.OrderBy(c => c.OrderIndex).ToList();
        var source = ordered.FirstOrDefault(c => c.FieldName == sourceFieldName);
        var target = ordered.FirstOrDefault(c => c.FieldName == targetFieldName);

        if (source is null || target is null || source == target) return false;

        var sourceIndex = ordered.IndexOf(source);
        var targetIndex = ordered.IndexOf(target);

        ordered.RemoveAt(sourceIndex);

        if (sourceIndex < targetIndex) targetIndex--; // ajuste por remove previo
        var insertIndex = beforeTarget ? targetIndex : targetIndex + 1;
        insertIndex = Math.Clamp(insertIndex, 0, ordered.Count);

        ordered.Insert(insertIndex, source);

        for (int i = 0; i < ordered.Count; i++)
            ordered[i].OrderIndex = i;

        return true;
    }

    private async Task NotifyColumnOrderChangedAsync()
    {
        if (!ColumnOrderChanged.HasDelegate)
            return;

        var visibleOrder = VisibleColumnsDefinition
            .Select(c => c.FieldName)
            .ToList();

        await ColumnOrderChanged.InvokeAsync(visibleOrder);
    }

    private string GetHeaderDragCssClass()
        => AllowColumnReorder ? "outlander-grid-col-reorder-enabled" : string.Empty;

    private bool IsColumnReorderDraggable(OutlanderGridColumnDefinition<TItem> column)
        => AllowColumnReorder && !column.IsSelectionColumn;

    private string? _dragOverFieldName;
    private bool _dropBeforeTarget = true; // true = cae antes, false = cae después
    private bool IsDraggingColumn => !string.IsNullOrWhiteSpace(_dragSourceFieldName);

    private void OnHeaderDragStart(DragEventArgs e, string sourceFieldName)
    {
        if (!AllowColumnReorder) return;

        _dragSourceFieldName = sourceFieldName;

        if (e.DataTransfer is not null)
        {
            e.DataTransfer.DropEffect = "move";
            e.DataTransfer.EffectAllowed = "move";
        }

        StateHasChanged(); // activa dropzones
    }

    private void OnDropZoneDragEnter(DragEventArgs e, string targetFieldName, bool before)
    {
        if (!AllowColumnReorder || string.IsNullOrWhiteSpace(_dragSourceFieldName))
            return;

        if (e.DataTransfer is not null) e.DataTransfer.DropEffect = "move";

        _dragOverFieldName = targetFieldName;
        _dropBeforeTarget = before;
    }

    private void OnDropZoneDragOver(DragEventArgs e, string targetFieldName, bool before)
    {
        if (!AllowColumnReorder || string.IsNullOrWhiteSpace(_dragSourceFieldName))
            return;

        if (e.DataTransfer is not null) e.DataTransfer.DropEffect = "move";

        _dragOverFieldName = targetFieldName;
        _dropBeforeTarget = before;
    }

    private async Task OnDropZoneDropAsync(DragEventArgs e, string targetFieldName, bool before)
    {
        if (!AllowColumnReorder || string.IsNullOrWhiteSpace(_dragSourceFieldName))
            return;

        _dropBeforeTarget = before;
        await OnHeaderDropAsync(targetFieldName);
    }

    private void ClearDragVisuals()
    {
        _dragSourceFieldName = null;
        _dragOverFieldName = null;
        _dropBeforeTarget = true;

        StateHasChanged(); // desactiva dropzones
    }

    private string GetDropIndicatorCss(OutlanderGridColumnDefinition<TItem> column)
    {
        if (column.FieldName != _dragOverFieldName) return string.Empty;
        return $"""outlander-grid-drop-over {(_dropBeforeTarget 
            ? "outlander-grid-drop-before"
            : "outlander-grid-drop-after")}""";
    }
}