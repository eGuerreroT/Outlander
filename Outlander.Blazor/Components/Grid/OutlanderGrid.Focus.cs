namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem>
{
    private bool IsFocusedRow(TItem item)
    {
        if (FocusedRow is null || item is null)
            return false;

        return EqualityComparer<TItem>.Default.Equals(FocusedRow, item);
    }

    private async Task SetFocusedRowAsync(TItem item)
    {
        if (!AllowFocusedRow)
            return;

        if (EqualityComparer<TItem>.Default.Equals(FocusedRow, item))
            return;

        FocusedRow = item;
        await FocusedRowChanged.InvokeAsync(item);
    }

    private string GetRowCssClass(TItem item)
    {
        if (!AllowFocusedRow)
            return string.Empty;

        return IsFocusedRow(item) ? "outlander-grid-row-focused" : string.Empty;
    }

    private async Task HandleRowClickAsync(TItem item)
    {
        if (AllowFocusedRow)
        {
            await SetFocusedRowAsync(item);
        }

        if (!RowDoubleClick.HasDelegate)
        {
            if (RowClick.HasDelegate)
            {
                await RowClick.InvokeAsync(item);
            }

            return;
        }

        _rowClickCts?.Cancel();
        _rowClickCts?.Dispose();

        var cts = new CancellationTokenSource();
        _rowClickCts = cts;

        try
        {
            await Task.Delay(RowClickDelayMs, cts.Token);

            if (!cts.Token.IsCancellationRequested && RowClick.HasDelegate)
            {
                await RowClick.InvokeAsync(item);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private async Task HandleRowDoubleClickAsync(TItem item)
    {
        _rowClickCts?.Cancel();

        if (AllowFocusedRow)
        {
            await SetFocusedRowAsync(item);
        }

        if (RowDoubleClick.HasDelegate)
        {
            await RowDoubleClick.InvokeAsync(item);
        }
    }

}