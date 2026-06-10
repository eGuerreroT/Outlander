namespace Outlander.Blazor.Components;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Defines the metadata and rendering behavior for a grid column.
/// </summary>
/// <typeparam name="TItem">Type of the row item rendered by the grid.</typeparam>
public class OutlanderGridColumnDefinition<TItem>
{
    /// <summary>
    /// Gets or sets the bound field name in <typeparamref name="TItem"/>.
    /// </summary>
    public string FieldName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text displayed in the column header.
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the column is visible in the grid.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the column can be toggled from the column chooser.
    /// </summary>
    public bool ShowInColumnChooser { get; set; } = true;

    /// <summary>
    /// Gets or sets whether sorting is enabled for the column.
    /// </summary>
    public bool AllowSort { get; set; }

    /// <summary>
    /// Gets or sets whether filtering is enabled for the column.
    /// </summary>
    public bool AllowFilter { get; set; }

    /// <summary>
    /// Gets or sets whether the column participates in global search.
    /// </summary>
    public bool SearchEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the column is the selection checkbox column.
    /// </summary>
    public bool IsSelectionColumn { get; set; }

    /// <summary>
    /// Gets or sets whether the grid should allow selecting all processed items from the toolbar.
    /// </summary>
    public bool AllowSelectAllItems { get; set; }

    /// <summary>
    /// Gets or sets whether the column is included in export operations.
    /// </summary>
    public bool AllowExport { get; set; } = true;

    /// <summary>
    /// Gets or sets the caption used when exporting the column.
    /// </summary>
    public string? ExportCaption { get; set; }

    /// <summary>
    /// Gets or sets a custom selector used to extract the exported value from a row item.
    /// </summary>
    public Func<TItem, object?>? ExportValueSelector { get; set; }

    /// <summary>
    /// Gets or sets the CSS width value for the column.
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the filter mode used by the column.
    /// </summary>
    public GridFilterMode FilterMode { get; set; } = GridFilterMode.Auto;

    /// <summary>
    /// Gets or sets how sorting is resolved for the column.
    /// </summary>
    public GridColumnSortMode SortMode { get; set; } = GridColumnSortMode.Value;

    /// <summary>
    /// Gets or sets the initial sort order of the column.
    /// </summary>
    public GridColumnSortOrder SortOrder { get; set; } = GridColumnSortOrder.None;

    /// <summary>
    /// Gets or sets the initial sort priority when multiple columns define a default sort.
    /// </summary>
    public int? SortIndex { get; set; }

    /// <summary>
    /// Gets or sets a custom selector used to obtain the display text used for sorting.
    /// </summary>
    public Func<TItem, string?>? SortTextSelector { get; set; }

    /// <summary>
    /// Gets or sets a custom selector used to obtain the display text used for filtering and search.
    /// </summary>
    public Func<TItem, string?>? FilterTextSelector { get; set; }

    /// <summary>
    /// Gets or sets a custom header template for the column.
    /// </summary>
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets a custom filter template for the column.
    /// </summary>
    public RenderFragment<OutlanderGridFilterContext>? FilterTemplate { get; set; }

    /// <summary>
    /// Gets or sets a custom cell template for the column.
    /// </summary>
    public RenderFragment<OutlanderGridCellContext<TItem>>? CellTemplate { get; set; }
}

/// <summary>
/// Provides context information for a custom filter template.
/// </summary>
public class OutlanderGridFilterContext
{
    /// <summary>
    /// Gets or sets the field name associated with the filter.
    /// </summary>
    public string FieldName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current filter value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback used to update the filter value.
    /// </summary>
    public Action<string?> SetValue { get; set; } = _ => { };
}

/// <summary>
/// Defines how a column resolves its sort value.
/// </summary>
public enum GridColumnSortMode
{
    /// <summary>
    /// Sorts using the raw property value.
    /// </summary>
    Value,

    /// <summary>
    /// Sorts using display text resolved by a text selector.
    /// </summary>
    DisplayText
}

/// <summary>
/// Defines the sort direction of a column.
/// </summary>
public enum GridColumnSortOrder
{
    /// <summary>
    /// No sort is applied.
    /// </summary>
    None,

    /// <summary>
    /// Sorts in ascending order.
    /// </summary>
    Ascending,

    /// <summary>
    /// Sorts in descending order.
    /// </summary>
    Descending
}

/// <summary>
/// Defines how the global search text is interpreted.
/// </summary>
public enum GridSearchTextParseMode
{
    /// <summary>
    /// All search terms must match.
    /// </summary>
    GroupWordsByAnd = 0,

    /// <summary>
    /// Any search term can match.
    /// </summary>
    GroupWordsByOr = 1,

    /// <summary>
    /// The full search text is treated as a single expression.
    /// </summary>
    ExactMatch = 2
}

/// <summary>
/// Provides context information for a custom cell template.
/// </summary>
/// <typeparam name="TItem">Type of the row item rendered by the grid.</typeparam>
public class OutlanderGridCellContext<TItem>
{
    /// <summary>
    /// Gets or sets the current row item.
    /// </summary>
    public TItem Item { get; set; } = default!;

    /// <summary>
    /// Gets or sets the field name associated with the cell.
    /// </summary>
    public string FieldName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current global search text.
    /// </summary>
    public string SearchText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a helper function that highlights matching search text.
    /// </summary>
    public Func<string?, MarkupString> Highlight { get; set; } = _ => new MarkupString(string.Empty);
}

/// <summary>
/// Defines the filter UI and matching behavior used by a column.
/// </summary>
public enum GridFilterMode
{
    /// <summary>
    /// Automatically selects the most appropriate filter mode based on the property type.
    /// </summary>
    Auto = 0,

    /// <summary>
    /// Uses a text input filter.
    /// </summary>
    Text = 1,

    /// <summary>
    /// Uses a select/dropdown filter.
    /// </summary>
    Select = 2,

    /// <summary>
    /// Uses a date filter.
    /// </summary>
    Date = 3,

    /// <summary>
    /// Uses a range filter.
    /// </summary>
    Range = 4
}

/// <summary>
/// Stores the value state for a column filter.
/// </summary>
public class OutlanderGridFilterValue
{
    /// <summary>
    /// Gets or sets the primary filter value.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the upper bound value used by range filters.
    /// </summary>
    public string? ValueTo { get; set; }
}

/// <summary>
/// Defines how export operations should render grid data.
/// </summary>
public enum OutlanderGridExportMode
{
    /// <summary>
    /// Exports normalized data values.
    /// </summary>
    Data = 0,

    /// <summary>
    /// Exports the grid using a WYSIWYG approach.
    /// </summary>
    Wysiwyg = 1
}

/// <summary>
/// Defines search-related settings for <see cref="OutlanderGrid{TItem}" />.
/// </summary>
public class OutlanderGridSearchSettingsDefinition
{
    /// <summary>
    /// Gets or sets whether the global search box is displayed.
    /// </summary>
    public bool Show { get; set; } = false;

    /// <summary>
    /// Gets or sets the placeholder text displayed in the global search box.
    /// </summary>
    public string NullText { get; set; } = "Type to search...";

    /// <summary>
    /// Gets or sets the init value for the global search box.
    /// </summary>
    public string Text { get; set; } = "";

    /// <summary>
    /// Gets or sets how the global search text is interpreted.
    /// </summary>
    public GridSearchTextParseMode ParseMode { get; set; } = GridSearchTextParseMode.GroupWordsByAnd;
}

/// <summary>
/// Defines filter-related settings for <see cref="OutlanderGrid{TItem}" />.
/// </summary>
public class OutlanderGridFilterSettingsDefinition
{
    /// <summary>
    /// Gets or sets whether the filter row is displayed.
    /// </summary>
    public bool Show { get; set; } = false;

    /// <summary>
    /// Gets or sets the placeholder text used by automatic text filters.
    /// </summary>
    public string NullText { get; set; } = "Filter...";

    /// <summary>
    /// Gets or sets the text displayed for the default option in select filters.
    /// </summary>
    public string AllText { get; set; } = "All";

    /// <summary>
    /// Gets or sets the placeholder text used for the lower bound of range filters.
    /// </summary>
    public string FromText { get; set; } = "From";

    /// <summary>
    /// Gets or sets the placeholder text used for the upper bound of range filters.
    /// </summary>
    public string ToText { get; set; } = "To";
}

/// <summary>
/// Defines footer-related settings for <see cref="OutlanderGrid{TItem}" />.
/// </summary>
public class OutlanderGridFooterSettingsDefinition
{
    /// <summary>
    /// Gets or sets the composite format string used in the footer summary.
    /// </summary>
    public string SummaryTextFormat { get; set; } = "Showing {0} - {1} of {2} records";

    /// <summary>
    /// Gets or sets the text displayed on the previous page button.
    /// </summary>
    public string PreviousPageText { get; set; } = "Prev";

    /// <summary>
    /// Gets or sets the text displayed on the next page button.
    /// </summary>
    public string NextPageText { get; set; } = "Next";
}

/// <summary>
/// Defines export-related settings for <see cref="OutlanderGrid{TItem}" />.
/// </summary>
public class OutlanderGridExportSettingsDefinition
{
    /// <summary>
    /// Gets or sets whether export buttons are displayed in the toolbar.
    /// </summary>
    public bool ShowButtons { get; set; } = false;

    /// <summary>
    /// Gets or sets whether print export is enabled.
    /// </summary>
    public bool AllowPrint { get; set; } = true;

    /// <summary>
    /// Gets or sets whether Excel export is enabled.
    /// </summary>
    public bool AllowExcel { get; set; } = true;

    /// <summary>
    /// Gets or sets whether PDF export is enabled.
    /// </summary>
    public bool AllowPdf { get; set; } = true;

    /// <summary>
    /// Gets or sets the base file name used for exported files.
    /// </summary>
    public string FileName { get; set; } = "grid-export";

    /// <summary>
    /// Gets or sets the title used in exported documents.
    /// </summary>
    public string Title { get; set; } = "Grid export";

    /// <summary>
    /// Gets or sets the export mode used for Excel export.
    /// </summary>
    public OutlanderGridExportMode ExcelMode { get; set; } = OutlanderGridExportMode.Data;

    /// <summary>
    /// Gets or sets the export mode used for PDF export.
    /// </summary>
    public OutlanderGridExportMode PdfMode { get; set; } = OutlanderGridExportMode.Wysiwyg;

    /// <summary>
    /// Gets or sets the export mode used for print export.
    /// </summary>
    public OutlanderGridExportMode PrintMode { get; set; } = OutlanderGridExportMode.Wysiwyg;

    /// <summary>
    /// Gets or sets the text displayed on the export dropdown button.
    /// </summary>
    public string ButtonText { get; set; } = "Export";

    /// <summary>
    /// Gets or sets the text displayed for the Excel export action.
    /// </summary>
    public string ExcelText { get; set; } = "Excel";

    /// <summary>
    /// Gets or sets the text displayed for the PDF export action.
    /// </summary>
    public string PdfText { get; set; } = "PDF";

    /// <summary>
    /// Gets or sets the text displayed for the print export action.
    /// </summary>
    public string PrintText { get; set; } = "Print";
}

/// <summary>
/// Defines selection-related settings for <see cref="OutlanderGrid{TItem}" />.
/// </summary>
public class OutlanderGridSelectionSettingsDefinition
{
    /// <summary>
    /// Gets or sets the text displayed for the select-all-items action.
    /// </summary>
    public string AllText { get; set; } = "Select All";

    /// <summary>
    /// Gets or sets the text displayed for the select-current-page action.
    /// </summary>
    public string CurrentPageText { get; set; } = "Select Current Page";

    /// <summary>
    /// Gets or sets the composite format string used to display the number of selected items.
    /// </summary>
    public string SelectedItemsTextFormat { get; set; } = "{0} Selected";

    /// <summary>
    /// Gets or sets the composite format string used to display the number of selected items in the current page.
    /// </summary>
    public string SelectedPageItemsTextFormat { get; set; } = "Selected in page: {0}";

}