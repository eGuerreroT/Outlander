using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;

namespace Outlander.Blazor.Components;

public partial class OutlanderGrid<TItem> : IAsyncDisposable
{
    private readonly List<OutlanderGridColumnDefinition<TItem>> _columns = [];
    private readonly Dictionary<string, OutlanderGridFilterValue> _filters = [];

    private bool _renderPending;
    private bool _initialSortApplied;

    private string? _sortFieldName;
    private GridColumnSortOrder _sortOrder = GridColumnSortOrder.None;

    private ElementReference _selectAllItemsCheckboxRef;
    private ElementReference _selectPageCheckboxRef;
    private ElementReference _tableWrapperRef;

    private IJSObjectReference? _module;
    private CancellationTokenSource? _rowClickCts;

    private int CurrentPage { get; set; } = 1;

    private IReadOnlyList<OutlanderGridColumnDefinition<TItem>> AllColumnsDefinition => _columns;
    private IReadOnlyList<OutlanderGridColumnDefinition<TItem>> VisibleColumnsDefinition => [.. _columns.Where(c => c.Visible)];
    private int ResolvedColumnCount => Math.Max(1, VisibleColumnsDefinition.Count);

    private OutlanderGridColumnDefinition<TItem>? SelectionColumn => AllColumnsDefinition.FirstOrDefault(c => c.IsSelectionColumn);
    private bool ShowSelectAllItems => SelectionColumn?.AllowSelectAllItems == true;

    private bool IsRowInteractive => AllowFocusedRow || AllowHotTrackRow || RowClick.HasDelegate || RowDoubleClick.HasDelegate;

    private IEnumerable<TItem> ProcessedItems => ApplySorting(ApplyGlobalSearch(ApplyFilters(Items)));
    private int TotalItems => ProcessedItems.Count();
    private IEnumerable<TItem> PagedItems => ProcessedItems.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
    private int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));
    private bool IsFirstPage => CurrentPage <= 1;
    private bool IsLastPage => CurrentPage >= TotalPages;
    private int StartRecord => TotalItems == 0 ? 0 : ((CurrentPage - 1) * PageSize) + 1;
    private int EndRecord => Math.Min(CurrentPage * PageSize, TotalItems);


    private bool _searchTextInitialized;
    private OutlanderGridSearchSettingsDefinition? _searchSettings;
    private bool ResolvedShowSearchBox => _searchSettings?.Show ?? ShowSearchBox;
    private string ResolvedSearchBoxNullText => _searchSettings?.NullText ?? SearchBoxNullText;
    private GridSearchTextParseMode ResolvedSearchTextParseMode => _searchSettings?.ParseMode ?? SearchBoxParseMode;


    private OutlanderGridFilterSettingsDefinition? _filterSettings;
    private bool ResolvedShowFilterRow => _filterSettings?.Show ?? ShowFilterRow;
    private string ResolvedFilterNullText => _filterSettings?.NullText ?? FilterNullText;
    private string ResolvedFilterAllText => _filterSettings?.AllText ?? FilterAllText;
    private string ResolvedFilterFromText => _filterSettings?.FromText ?? FilterFromText;
    private string ResolvedFilterToText => _filterSettings?.ToText ?? FilterToText;


    private OutlanderGridFooterSettingsDefinition? _footerSettings;
    private string ResolvedSummaryTextFormat => _footerSettings?.SummaryTextFormat ?? SummaryTextFormat;
    private string ResolvedPreviousPageText => _footerSettings?.PreviousPageText ?? PreviousPageText;
    private string ResolvedNextPageText => _footerSettings?.NextPageText ?? NextPageText;


    private OutlanderGridExportSettingsDefinition? _exportSettings;
    private bool ResolvedShowExportButtons => _exportSettings?.ShowButtons ?? ShowExportButtons;
    private bool ResolvedAllowExportPrint => _exportSettings?.AllowPrint ?? AllowExportPrint;
    private bool ResolvedAllowExportExcel => _exportSettings?.AllowExcel ?? AllowExportExcel;
    private bool ResolvedAllowExportPdf => _exportSettings?.AllowPdf ?? AllowExportPdf;
    private string ResolvedExportFileName => _exportSettings?.FileName ?? ExportFileName;
    private string ResolvedExportTitle => _exportSettings?.Title ?? ExportTitle;
    private OutlanderGridExportMode ResolvedExcelExportMode => _exportSettings?.ExcelMode ?? ExcelExportMode;
    private OutlanderGridExportMode ResolvedPdfExportMode => _exportSettings?.PdfMode ?? PdfExportMode;
    private OutlanderGridExportMode ResolvedPrintExportMode => _exportSettings?.PrintMode ?? PrintExportMode;
    private string ResolvedExportButtonText => _exportSettings?.ButtonText ?? ExportButtonText;
    private string ResolvedExportExcelText => _exportSettings?.ExcelText ?? ExportExcelText;
    private string ResolvedExportPdfText => _exportSettings?.PdfText ?? ExportPdfText;
    private string ResolvedExportPrintText => _exportSettings?.PrintText ?? ExportPrintText;


    private OutlanderGridSelectionSettingsDefinition? _selectionSettings;
    private string ResolvedSelectAllItemsText => _selectionSettings?.AllText ?? SelectAllItemsText;
    private string ResolvedSelectCurrentPageText => _selectionSettings?.CurrentPageText ?? SelectCurrentPageText;
    private string ResolvedSelectedItemsTextFormat => _selectionSettings?.SelectedItemsTextFormat ?? SelectedItemsTextFormat;
    private string ResolvedSelectedPageItemsTextFormat => _selectionSettings?.SelectedPageItemsTextFormat ?? SelectedPageItemsTextFormat;


    /// <summary>
    /// Gets or sets the collection of items rendered by the grid.
    /// </summary>
    [Parameter, EditorRequired]
    public IEnumerable<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the child column definitions declared inside the grid.
    /// </summary>
    [Parameter]
    public RenderFragment? Columns { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the toolbar area.
    /// </summary>
    [Parameter]
    public RenderFragment? ToolbarTemplate { get; set; }

    /// <summary>
    /// Defines the child settings components of the grid.
    /// </summary>
    [Parameter]
    public RenderFragment? Settings { get; set; }

    /// <summary>
    /// Gets or sets the text displayed when the grid has no rows to render.
    /// </summary>
    [Parameter]
    public string EmptyText { get; set; } = "No records to show.";

    /// <summary>
    /// Gets or sets a value indicating whether the column chooser is displayed.
    /// </summary>
    [Parameter]
    public bool ShowColumnChooser { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the page size selector is displayed in the toolbar.
    /// </summary>
    [Parameter]
    public bool ShowPageSizeSelector { get; set; } = false;

    /// <summary>
    /// Gets or sets the number of rows displayed per page.
    /// </summary>
    [Parameter]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the callback invoked when <see cref="PageSize"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<int> PageSizeChanged { get; set; }

    /// <summary>
    /// Gets or sets the available page size options shown in the selector.
    /// </summary>
    [Parameter]
    public List<int> PageSizeOptions { get; set; } = [5, 10, 20, 50];

    /// <summary>
    /// Gets or sets a value indicating whether column sorting is enabled.
    /// </summary>
    [Parameter]
    public bool AllowSort { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the global search box is displayed.
    /// </summary>
    [Parameter]
    public bool ShowSearchBox { get; set; } = false;

    /// <summary>
    /// Gets or sets the placeholder text displayed in the global search box.
    /// </summary>
    [Parameter]
    public string SearchBoxNullText { get; set; } = "Search...";

    /// <summary>
    /// Gets or sets the current global search text.
    /// </summary>
    [Parameter]
    public string SearchBoxText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets how the global search text is interpreted.
    /// </summary>
    [Parameter]
    public GridSearchTextParseMode SearchBoxParseMode { get; set; } = GridSearchTextParseMode.GroupWordsByAnd;

    /// <summary>
    /// Gets or sets a value indicating whether rows use hover highlighting.
    /// </summary>
    [Parameter]
    public bool AllowHotTrackRow { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether focused row behavior is enabled.
    /// </summary>
    [Parameter]
    public bool AllowFocusedRow { get; set; } = false;

    /// <summary>
    /// Gets or sets the currently focused row.
    /// </summary>
    [Parameter]
    public TItem? FocusedRow { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when <see cref="FocusedRow"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<TItem?> FocusedRowChanged { get; set; }

    /// <summary>
    /// Gets or sets the currently selected rows.
    /// </summary>
    [Parameter]
    public IList<TItem> SelectedItems { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback invoked when <see cref="SelectedItems"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<IList<TItem>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a row is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> RowClick { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a row is double-clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> RowDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets the delay, in milliseconds, used to distinguish a single click from a double-click.
    /// </summary>
    [Parameter]
    public int RowClickDelayMs { get; set; } = 250;

    /// <summary>
    /// Gets or sets a value indicating whether export buttons are displayed in the toolbar.
    /// </summary>
    [Parameter]
    public bool ShowExportButtons { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether print export is enabled.
    /// </summary>
    [Parameter]
    public bool AllowExportPrint { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether Excel export is enabled.
    /// </summary>
    [Parameter]
    public bool AllowExportExcel { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether PDF export is enabled.
    /// </summary>
    [Parameter]
    public bool AllowExportPdf { get; set; } = true;

    /// <summary>
    /// Gets or sets the base file name used for exported files.
    /// </summary>
    [Parameter]
    public string ExportFileName { get; set; } = "grid-export";

    /// <summary>
    /// Gets or sets the title used in exported documents.
    /// </summary>
    [Parameter]
    public string ExportTitle { get; set; } = "Grid export";

    /// <summary>
    /// Gets or sets the export mode used for Excel export.
    /// </summary>
    [Parameter]
    public OutlanderGridExportMode ExcelExportMode { get; set; } = OutlanderGridExportMode.Data;

    /// <summary>
    /// Gets or sets the export mode used for PDF export.
    /// </summary>
    [Parameter]
    public OutlanderGridExportMode PdfExportMode { get; set; } = OutlanderGridExportMode.Wysiwyg;

    /// <summary>
    /// Gets or sets the export mode used for print export.
    /// </summary>
    [Parameter]
    public OutlanderGridExportMode PrintExportMode { get; set; } = OutlanderGridExportMode.Wysiwyg;

    /// <summary>
    /// Gets or sets the text displayed on the export dropdown button.
    /// </summary>
    [Parameter]
    public string ExportButtonText { get; set; } = "Export";

    /// <summary>
    /// Gets or sets the text displayed for the Excel export action.
    /// </summary>
    [Parameter]
    public string ExportExcelText { get; set; } = "Excel";

    /// <summary>
    /// Gets or sets the text displayed for the PDF export action.
    /// </summary>
    [Parameter]
    public string ExportPdfText { get; set; } = "PDF";

    /// <summary>
    /// Gets or sets the text displayed for the print action.
    /// </summary>
    [Parameter]
    public string ExportPrintText { get; set; } = "Print";

    /// <summary>
    /// Gets or sets the label displayed before the page size selector.
    /// </summary>
    [Parameter]
    public string PageSizeLabelText { get; set; } = "Show";

    /// <summary>
    /// Gets or sets the text displayed after the page size selector.
    /// </summary>
    [Parameter]
    public string PageSizeSuffixText { get; set; } = "records";

    /// <summary>
    /// Gets or sets the text displayed on the column chooser button.
    /// </summary>
    [Parameter]
    public string ColumnChooserButtonText { get; set; } = "Columns";

    /// <summary>
    /// Gets or sets a value indicating whether the filter row is displayed below the header row.
    /// </summary>
    [Parameter]
    public bool ShowFilterRow { get; set; } = false;

    /// <summary>
    /// Gets or sets the placeholder text used by automatic text filters.
    /// </summary>
    [Parameter]
    public string FilterNullText { get; set; } = "Filter...";

    /// <summary>
    /// Gets or sets the text displayed for the default option in select filters.
    /// </summary>
    [Parameter]
    public string FilterAllText { get; set; } = "All";

    /// <summary>
    /// Gets or sets the placeholder text used for the lower bound of range filters.
    /// </summary>
    [Parameter]
    public string FilterFromText { get; set; } = "From";

    /// <summary>
    /// Gets or sets the placeholder text used for the upper bound of range filters.
    /// </summary>
    [Parameter]
    public string FilterToText { get; set; } = "To";

    /// <summary>
    /// Gets or sets the text used to display a <see langword="true" /> boolean value.
    /// </summary>
    [Parameter]
    public string BooleanTrueText { get; set; } = "Yes";

    /// <summary>
    /// Gets or sets the text used to display a <see langword="false" /> boolean value.
    /// </summary>
    [Parameter]
    public string BooleanFalseText { get; set; } = "No";

    /// <summary>
    /// Gets or sets the text displayed for the select-all-items action.
    /// </summary>
    [Parameter]
    public string SelectAllItemsText { get; set; } = "Select All";

    /// <summary>
    /// Gets or sets the text displayed for the select-current-page action.
    /// </summary>
    [Parameter]
    public string SelectCurrentPageText { get; set; } = "Select Current Page";

    /// <summary>
    /// Gets or sets the composite format string used to display the number of selected items.
    /// </summary>
    [Parameter]
    public string SelectedItemsTextFormat { get; set; } = "{0} Selected";

    /// <summary>
    /// Gets or sets the composite format string used to display the number of selected items in the current page.
    /// </summary>
    [Parameter]
    public string SelectedPageItemsTextFormat { get; set; } = "Selected in page: {0}";

    /// <summary>
    /// Gets or sets the composite format string used in the footer summary.
    /// </summary>
    [Parameter]
    public string SummaryTextFormat { get; set; } = "Showing {0} - {1} of {2} records";

    /// <summary>
    /// Gets or sets the text displayed on the previous page button.
    /// </summary>
    [Parameter]
    public string PreviousPageText { get; set; } = "Prev";

    /// <summary>
    /// Gets or sets the text displayed on the next page button.
    /// </summary>
    [Parameter]
    public string NextPageText { get; set; } = "Next";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "/_content/Outlander.Blazor/Components/Grid/OutlanderGrid.razor.js");

            await _module.InvokeVoidAsync("ensureBootStrapLibraries");
        }

        await UpdateSelectionCheckboxStatesAsync();
    }

    protected override void OnParametersSet()
    {
        if (CurrentPage > TotalPages)
        {
            CurrentPage = TotalPages;
        }
    }

    internal void RegisterColumn(OutlanderGridColumnDefinition<TItem> column)
    {
        if (_columns.All(c => c.FieldName != column.FieldName))
        {
            _columns.Add(column);

            if (!_renderPending)
            {
                _renderPending = true;

                _ = InvokeAsync(() =>
                {
                    _renderPending = false;
                    StateHasChanged();
                });
            }
        }
    }

    internal void RegisterSelectionColumn(OutlanderGridColumnDefinition<TItem> column)
    {
        if (_columns.Any(c => c.IsSelectionColumn))
            throw new InvalidOperationException("Solo se permite una OutlanderGridSelectionColumn por grid.");

        _columns.Insert(0, column);

        if (!_renderPending)
        {
            _renderPending = true;

            _ = InvokeAsync(() =>
            {
                _renderPending = false;
                StateHasChanged();
            });
        }
    }

    internal void RegisterSearchSettings(OutlanderGridSearchSettingsDefinition settings)
    {
        _searchSettings = settings;

        if (!_searchTextInitialized && string.IsNullOrWhiteSpace(SearchBoxText))
        {
            SearchBoxText = settings.Text ?? string.Empty;
            _searchTextInitialized = true;
        }
    }

    internal void RegisterFilterSettings(OutlanderGridFilterSettingsDefinition settings)
    {
        _filterSettings = settings;
    }

    internal void RegisterFooterSettings(OutlanderGridFooterSettingsDefinition settings)
    {
        _footerSettings = settings;
    }

    internal void RegisterExportSettings(OutlanderGridExportSettingsDefinition settings)
    {
        _exportSettings = settings;
    }

    internal void RegisterSelectionSettings(OutlanderGridSelectionSettingsDefinition settings)
    {
        _selectionSettings = settings;
    }

    private string GetWidthStyle(OutlanderGridColumnDefinition<TItem> column)
        => string.IsNullOrWhiteSpace(column.Width) ? string.Empty : $"width:{column.Width};";

    private static object? GetPropertyValue(TItem item, string fieldName)
    {
        var prop = typeof(TItem).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        return prop?.GetValue(item);
    }

    public async ValueTask DisposeAsync()
    {
        _rowClickCts?.Cancel();
        _rowClickCts?.Dispose();

        if (_module is null)
            return;

        try
        {
            await _module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }
}