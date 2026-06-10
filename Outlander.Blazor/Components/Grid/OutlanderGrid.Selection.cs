using Microsoft.JSInterop;

namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem>
{
    private bool IsSelected(TItem item)
    {
        return SelectedItems?.Contains(item) == true;
    }

    private async Task SetItemSelectedAsync(TItem item, bool selected)
    {
        var current = SelectedItems?.ToList() ?? new List<TItem>();

        if (selected)
        {
            if (!current.Contains(item))
                current.Add(item);
        }
        else
        {
            current.Remove(item);
        }

        SelectedItems = current;
        await SelectedItemsChanged.InvokeAsync(current);
    }

    private bool AreAllPageItemsSelected()
    {
        var pageItems = PagedItems.ToList();
        if (pageItems.Count == 0)
            return false;

        return pageItems.All(item => SelectedItems?.Contains(item) == true);
    }

    private async Task SetAllPageItemsSelectedAsync(bool selected)
    {
        var current = SelectedItems?.ToList() ?? new List<TItem>();
        var pageItems = PagedItems.ToList();

        if (selected)
        {
            foreach (var item in pageItems)
            {
                if (!current.Contains(item))
                    current.Add(item);
            }
        }
        else
        {
            foreach (var item in pageItems)
            {
                current.Remove(item);
            }
        }

        SelectedItems = current;
        await SelectedItemsChanged.InvokeAsync(current);
    }

    private bool AreAllProcessedItemsSelected()
    {
        var items = ProcessedItems.ToList();
        if (items.Count == 0)
            return false;

        return items.All(item => SelectedItems?.Contains(item) == true);
    }

    private async Task SetAllProcessedItemsSelectedAsync(bool selected)
    {
        var current = SelectedItems?.ToList() ?? new List<TItem>();
        var items = ProcessedItems.ToList();

        if (selected)
        {
            foreach (var item in items)
            {
                if (!current.Contains(item))
                    current.Add(item);
            }
        }
        else
        {
            foreach (var item in items)
            {
                current.Remove(item);
            }
        }

        SelectedItems = current;
        await SelectedItemsChanged.InvokeAsync(current);
    }

    private int GetSelectedPageItemsCount()
    {
        var pageItems = PagedItems.ToList();
        if (pageItems.Count == 0 || SelectedItems is null || SelectedItems.Count == 0)
            return 0;

        return pageItems.Count(item => SelectedItems.Contains(item));
    }

    private int GetSelectedProcessedItemsCount()
    {
        var items = ProcessedItems.ToList();
        if (items.Count == 0 || SelectedItems is null || SelectedItems.Count == 0)
            return 0;

        return items.Count(item => SelectedItems.Contains(item));
    }

    private string GetSelectAllItemsLabel()
    {
        var count = GetSelectedProcessedItemsCount();
        return count > 0
            ? string.Format(ResolvedSelectedItemsTextFormat, count)
            : ResolvedSelectAllItemsText;
    }

    private string GetSelectCurrentPageTitle()
    {
        var count = GetSelectedPageItemsCount();
        return count > 0
            ? string.Format(ResolvedSelectedPageItemsTextFormat, count)
            : ResolvedSelectCurrentPageText;
    }

    private bool IsPageSelectionIndeterminate()
    {
        var pageItems = PagedItems.ToList();
        if (pageItems.Count == 0)
            return false;

        var selectedCount = GetSelectedPageItemsCount();
        return selectedCount > 0 && selectedCount < pageItems.Count;
    }

    private bool IsProcessedSelectionIndeterminate()
    {
        var items = ProcessedItems.ToList();
        if (items.Count == 0)
            return false;

        var selectedCount = GetSelectedProcessedItemsCount();
        return selectedCount > 0 && selectedCount < items.Count;
    }

    private async Task UpdateSelectionCheckboxStatesAsync()
    {
        if (_module is null)
            return;

        if (ShowSelectAllItems)
        {
            await _module.InvokeVoidAsync("setIndeterminate", _selectAllItemsCheckboxRef, IsProcessedSelectionIndeterminate());
        }

        if (SelectionColumn is not null && SelectionColumn.Visible)
        {
            await _module.InvokeVoidAsync("setIndeterminate", _selectPageCheckboxRef, IsPageSelectionIndeterminate());
        }
    }

}