using Microsoft.AspNetCore.Components;

namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem>
{
    private IEnumerable<int> VisiblePages
    {
        get
        {
            const int maxVisiblePages = 5;
            var start = Math.Max(1, CurrentPage - 2);
            var end = Math.Min(TotalPages, start + maxVisiblePages - 1);

            if ((end - start + 1) < maxVisiblePages)
            {
                start = Math.Max(1, end - maxVisiblePages + 1);
            }

            return Enumerable.Range(start, end - start + 1);
        }
    }

    private async Task HandlePageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newSize))
        {
            PageSize = newSize;
            CurrentPage = 1;
            await PageSizeChanged.InvokeAsync(newSize);
        }
    }

    private void PreviousPage()
    {
        if (!IsFirstPage)
        {
            CurrentPage--;
        }
    }

    private void NextPage()
    {
        if (!IsLastPage)
        {
            CurrentPage++;
        }
    }

    private void GoToPage(int page)
    {
        if (page >= 1 && page <= TotalPages)
        {
            CurrentPage = page;
        }
    }
}