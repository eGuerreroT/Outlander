const PACKAGE_ID = "AdminTemplate.Web.Components";

let exportLibrariesPromise = null;

function getAssetUrl(relativePath) {
    //return `./_content/${PACKAGE_ID}/${relativePath}`;
    return `/${relativePath}`;
}

function loadScript(src) {
    return new Promise((resolve, reject) => {
        const existing = document.querySelector(`script[src="${src}"]`);

        if (existing) {
            if (existing.dataset.loaded === "true") {
                resolve();
                return;
            }

            existing.addEventListener("load", () => resolve(), { once: true });
            existing.addEventListener("error", () => reject(new Error(`No se pudo cargar el script: ${src}`)), { once: true });
            return;
        }

        const script = document.createElement("script");
        script.src = src;
        script.async = true;

        script.addEventListener("load", () => {
            script.dataset.loaded = "true";
            resolve();
        }, { once: true });

        script.addEventListener("error", () => {
            reject(new Error(`No se pudo cargar el script: ${src}`));
        }, { once: true });

        document.head.appendChild(script);
    });
}

async function ensureExportLibraries() {
    if (!exportLibrariesPromise) {
        exportLibrariesPromise = (async () => {
            if (!window.XLSX) {
                await loadScript(getAssetUrl("lib/sheetjs/xlsx.full.min.js"));
            }

            if (!window.jspdf || !window.jspdf.jsPDF) {
                await loadScript(getAssetUrl("lib/jspdf/jspdf.umd.min.js"));
            }

            if (!window.html2pdf) {
                await loadScript(getAssetUrl("lib/html2pdf/html2pdf.bundle.min.js"));
            }

            if (!window.html2canvas) {
                await loadScript(getAssetUrl("lib/html2pdf/html2canvas.min.js"));
            }

            const hasAutoTable =
                window.jspdf &&
                window.jspdf.jsPDF &&
                typeof window.jspdf.jsPDF.API?.autoTable === "function";

            if (!hasAutoTable) {
                await loadScript(getAssetUrl("lib/jspdf-autotable/jspdf.plugin.autotable.min.js"));
            }
        })();
    }

    return exportLibrariesPromise;
}

export async function ensureBootStrapLibraries() {
    const bootstrapVersion = window.bootstrap?.Tooltip?.VERSION;

    console.log(`Bootstrap detectado: ${bootstrapVersion}`);

    if (!bootstrapVersion) {
        throw new Error("Bootstrap no está disponible.");
    }

    if (!isVersionGreaterOrEqual(bootstrapVersion, "5.3.0")) {
        throw new Error(`Bootstrap ${bootstrapVersion} no es compatible. Se requiere Bootstrap 5.3 o superior.`);
    }
}

function isVersionGreaterOrEqual(current, required) {

    const currentParts = current.split('.').map(Number);
    const requiredParts = required.split('.').map(Number);

    for (let i = 0; i < Math.max(currentParts.length, requiredParts.length); i++) {

        const currentValue = currentParts[i] || 0;
        const requiredValue = requiredParts[i] || 0;

        if (currentValue > requiredValue) {
            return true;
        }

        if (currentValue < requiredValue) {
            return false;
        }
    }

    return true;
}

function buildWorksheetData(title, headers, rows) {
    const data = [];

    if (title) {
        data.push([title]);
        data.push([]);
    }

    data.push(headers);

    for (const row of rows) {
        data.push(row);
    }

    return data;
}

function escapeHtml(value) {
    return String(value ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#39;");
}

export function setIndeterminate(element, value) {
    if (!element) return;
    element.indeterminate = !!value;
}

function getTextWidth(value) {
    if (value === null || value === undefined) return 0;
    return String(value).length;
}

function computeColumnWidths(headers, rows) {
    return headers.map((header, colIndex) => {
        let maxLength = getTextWidth(header);

        for (const row of rows) {
            const cell = row[colIndex];
            const cellLength = getTextWidth(cell);
            if (cellLength > maxLength) {
                maxLength = cellLength;
            }
        }

        // ancho mínimo 14, máximo 80
        return { wch: Math.min(Math.max(maxLength + 4, 14), 80) };
    });
}

function tryApplyHeaderStyle(worksheet, headersRowIndex, headersLength) {
    if (!window.XLSX?.utils?.encode_cell) return;

    for (let col = 0; col < headersLength; col++) {
        const cellAddress = window.XLSX.utils.encode_cell({ r: headersRowIndex, c: col });
        const cell = worksheet[cellAddress];
        if (!cell) continue;

        // Puede que algunas builds CE ignoren estilos visuales.
        cell.s = {
            font: { bold: true, color: { rgb: "FFFFFF" } },
            fill: { fgColor: { rgb: "343A40" } },
            alignment: {
                horizontal: "center",
                vertical: "center",
                wrapText: true
            }
        };
    }
}

export async function exportExcel(fileName, title, headers, rows) {
    await ensureExportLibraries();

    if (!window.XLSX) {
        throw new Error("SheetJS (XLSX) no está disponible.");
    }

    const data = [];
    let headerRowIndex = 0;

    if (title) {
        data.push([title]);
        data.push([]);
        headerRowIndex = 2;
    }

    data.push(headers);

    for (const row of rows) {
        data.push(row);
    }

    const worksheet = window.XLSX.utils.aoa_to_sheet(data);

    // Merge del título
    if (title && headers.length > 1) {
        worksheet["!merges"] = [
            {
                s: { r: 0, c: 0 },
                e: { r: 0, c: headers.length - 1 }
            }
        ];
    }

    // Ancho automático por contenido
    worksheet["!cols"] = computeColumnWidths(headers, rows);

    // Autofilter sobre la fila de encabezado
    const lastRowIndex = data.length - 1;
    const lastColIndex = headers.length - 1;

    worksheet["!autofilter"] = {
        ref: window.XLSX.utils.encode_range({
            s: { r: headerRowIndex, c: 0 },
            e: { r: lastRowIndex, c: lastColIndex }
        })
    };

    // Intentar congelar encabezado
    // Nota: según versión/build de XLSX, esta parte puede o no reflejarse igual.
    worksheet["!freeze"] = {
        xSplit: 0,
        ySplit: headerRowIndex + 1,
        topLeftCell: window.XLSX.utils.encode_cell({ r: headerRowIndex + 1, c: 0 }),
        activePane: "bottomLeft",
        state: "frozen"
    };

    // Intentar estilo visual del header
    try {
        tryApplyHeaderStyle(worksheet, headerRowIndex, headers.length);
    } catch {
        // Si la build no soporta estilos, no rompemos exportación
    }

    const workbook = window.XLSX.utils.book_new();
    window.XLSX.utils.book_append_sheet(workbook, worksheet, "Data");

    workbook.Props = {
        Title: title || fileName,
        Subject: "Grid Export",
        Author: "OutlanderGrid",
        CreatedDate: new Date()
    };

    window.XLSX.writeFile(workbook, `${fileName}.xlsx`);
}

export async function exportPdf(fileName, title, headers, rows) {
    await ensureExportLibraries();

    if (!window.jspdf || !window.jspdf.jsPDF) {
        throw new Error("jsPDF no está disponible.");
    }

    const doc = new window.jspdf.jsPDF({
        orientation: "landscape",
        unit: "pt",
        format: "a4"
    });

    if (title) {
        doc.setFontSize(14);
        doc.text(title, 40, 40);
    }

    if (typeof doc.autoTable !== "function") {
        throw new Error("jsPDF-AutoTable no está disponible.");
    }

    doc.autoTable({
        head: [headers],
        body: rows,
        startY: title ? 60 : 40,
        styles: {
            fontSize: 8,
            cellPadding: 4
        },
        headStyles: {
            fillColor: [52, 58, 64]
        },
        theme: "grid"
    });

    doc.save(`${fileName}.pdf`);
}

export function printGrid(title, headers, rows) {
    const printWindow = window.open("", "_blank");

    if (!printWindow) {
        throw new Error("No se pudo abrir la ventana de impresión.");
    }

    const headerHtml = headers
        .map(h => `<th>${escapeHtml(h)}</th>`)
        .join("");

    const rowsHtml = rows
        .map(row => `
            <tr>
                ${row.map(cell => `<td>${escapeHtml(cell)}</td>`).join("")}
            </tr>
        `)
        .join("");

    const html = `
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="utf-8" />
            <title>${escapeHtml(title || "Impresión")}</title>
            <style>
                body {
                    font-family: Arial, Helvetica, sans-serif;
                    margin: 24px;
                    color: #222;
                }

                h1 {
                    font-size: 18px;
                    margin-bottom: 16px;
                }

                table {
                    width: 100%;
                    border-collapse: collapse;
                    font-size: 12px;
                }

                th, td {
                    border: 1px solid #ccc;
                    padding: 8px;
                    text-align: left;
                    vertical-align: top;
                }

                th {
                    background: #f3f3f3;
                    font-weight: bold;
                }

                @media print {
                    body {
                        margin: 0;
                    }
                }
            </style>
        </head>
        <body>
            <h1>${escapeHtml(title || "Impresión")}</h1>
            <table>
                <thead>
                    <tr>${headerHtml}</tr>
                </thead>
                <tbody>
                    ${rowsHtml}
                </tbody>
            </table>
        </body>
        </html>`;

    printWindow.document.open();
    printWindow.document.write(html);
    printWindow.document.close();

    printWindow.focus();
    printWindow.print();
}

function removeNodes(root, selectors) {
    for (const selector of selectors) {
        root.querySelectorAll(selector).forEach(node => node.remove());
    }
}

function removeColumnAt(table, columnIndex) {
    table.querySelectorAll("tr").forEach(row => {
        const cells = row.children;
        if (columnIndex >= 0 && columnIndex < cells.length) {
            cells[columnIndex].remove();
        }
    });
}

function removeEmptyColumns(table) {
    const headerRow = table.querySelector("thead tr");
    if (!headerRow) return;

    const headerCells = Array.from(headerRow.children);

    for (let i = headerCells.length - 1; i >= 0; i--) {
        const headerText = (headerCells[i].innerText || headerCells[i].textContent || "").trim();
        const bodyCells = Array.from(table.querySelectorAll(`tbody tr`))
            .map(tr => tr.children[i])
            .filter(Boolean);

        const allBodyCellsEmpty = bodyCells.every(td => {
            const text = (td.innerText || td.textContent || "").trim();
            return text === "";
        });

        if (headerText === "" && allBodyCellsEmpty) {
            removeColumnAt(table, i);
        }
    }
}

function removeIgnoredColumns(table) {
    const headerRow = table.querySelector("thead tr");
    if (!headerRow) return;

    const headerCells = Array.from(headerRow.children);
    const ignoredIndexes = [];

    headerCells.forEach((cell, index) => {
        if (cell.classList.contains("app-grid-export-ignore") || cell.matches("[data-export-ignore='true']")) {
            ignoredIndexes.push(index);
        }
    });

    for (let i = ignoredIndexes.length - 1; i >= 0; i--) {
        removeColumnAt(table, ignoredIndexes[i]);
    }
}

function cleanupClonedGridTable(container) {
    const table = container.querySelector("table");
    if (!table) return;

    // 1. Quitar filter row: dejamos solo la primera fila del thead
    const thead = table.querySelector("thead");
    if (thead) {
        const theadRows = thead.querySelectorAll("tr");
        for (let i = theadRows.length - 1; i >= 1; i--) {
            theadRows[i].remove();
        }
    }

    // 2. Eliminar columnas completas marcadas como no exportables
    removeIgnoredColumns(table);

    // 3. Eliminar elementos internos marcados explícitamente
    removeNodes(container, [
        ".app-grid-export-ignore",
        "[data-export-ignore='true']"
    ]);

    // 4. Quitar elementos interactivos residuales no deseados
    removeNodes(container, [
        ".app-grid-footer",
        ".app-grid-pager",
        ".app-grid-toolbar",
        ".app-grid-column-sort",
        ".app-grid-column-filter",
        ".app-grid-column-menu",
        ".app-grid-header-icon",
        ".dropdown-menu"
    ]);

    // 5. Quitar controles residuales
    removeNodes(container, [
        "input[type='checkbox']",
        "input[type='radio']",
        "input[type='search']",
        ".app-grid-sort-button i"
    ]);

    // 6. Limpiar atributos interactivos
    container.querySelectorAll("*").forEach(el => {
        el.removeAttribute("onclick");
        el.removeAttribute("ondblclick");
        el.removeAttribute("onchange");
        el.removeAttribute("oninput");
        el.removeAttribute("tabindex");
        el.removeAttribute("aria-expanded");
        el.removeAttribute("aria-controls");
        el.removeAttribute("data-bs-toggle");
        el.removeAttribute("data-bs-target");
    });

    // 7. Quitar columnas vacías residuales
    removeEmptyColumns(table);
}

export function printGridWysiwyg(tableWrapperElement, title) {
    if (!tableWrapperElement) {
        throw new Error("No se encontró el contenedor de la tabla del grid.");
    }

    const table = tableWrapperElement.querySelector("table");

    if (!table) {
        throw new Error("No se encontró la tabla del grid.");
    }

    const clonedTable = table.cloneNode(true);
    clonedTable.classList.add("app-grid-card");

    const tempContainer = document.createElement("div");
    tempContainer.style.padding = "10px";
    tempContainer.appendChild(clonedTable);

    cleanupClonedGridTable(tempContainer);

    const printWindow = window.open("", "_blank");

    if (!printWindow) {
        throw new Error("No se pudo abrir la ventana de impresión.");
    }

    const stylesHtml = collectDocumentStylesHtml();
    const themeBootstrap = document.documentElement.getAttribute("data-bs-theme");

    const html = `
        <html data-bs-theme="${escapeHtml(themeBootstrap || "light")}">
            <head>
                <meta charset="utf-8" />
                <title>${escapeHtml(title || "Impresión")}</title>
                ${stylesHtml}
                <style>
                    body {
                        font-family: Arial, Helvetica, sans-serif;
                        padding: 24px;
                    }

                    .app-grid-print-title {
                        font-size: 22px;
                        font-weight: 700;
                        margin-bottom: 16px;
                    }

                    @media print {
                        body {
                            padding: 12mm;
                            -webkit-print-color-adjust: exact;
                            print-color-adjust: exact;
                        }
                    }
                </style>
            </head>
            <body>
                ${title ? `<div class="app-grid-print-title">${escapeHtml(title)}</div>` : ""}
                ${tempContainer.outerHTML}
            </body>
        </html>
    `;

    printWindow.document.open();
    printWindow.document.write(html);
    printWindow.document.close();

    printWindow.focus();

    setTimeout(() => {
        printWindow.print();
    }, 250);
}

function getCleanInnerText(element) {
    if (!element) return "";
    return (element.innerText || element.textContent || "")
        .replace(/\s+/g, " ")
        .trim();
}

function extractTableDataFromClonedTable(tableElement) {
    const headers = [];
    const rows = [];

    const headerCells = tableElement.querySelectorAll("thead th");
    headerCells.forEach(th => {
        headers.push(getCleanInnerText(th));
    });

    const bodyRows = tableElement.querySelectorAll("tbody tr");
    bodyRows.forEach(tr => {
        const row = [];
        const cells = tr.querySelectorAll("td");

        cells.forEach(td => {
            row.push(getCleanInnerText(td));
        });

        // solo agrega filas con al menos una celda
        if (row.length > 0) {
            rows.push(row);
        }
    });

    return { headers, rows };
}

export async function exportPdfWysiwyg(tableWrapperElement, fileName, title) {
    await ensureExportLibraries();

    if (!tableWrapperElement) {
        throw new Error("No se encontró el contenedor de la tabla del grid.");
    }

    if (!window.jspdf || !window.jspdf.jsPDF) {
        throw new Error("jsPDF no está disponible.");
    }

    const originalTable = tableWrapperElement.querySelector("table");

    if (!originalTable) {
        throw new Error("No se encontró la tabla del grid.");
    }

    const clonedTable = originalTable.cloneNode(true);

    const tempContainer = document.createElement("div");
    tempContainer.style.backgroundColor = window.getComputedStyle(document.body).backgroundColor;
    tempContainer.style.padding = "10px";

    tempContainer.appendChild(clonedTable);

    cleanupClonedGridTable(tempContainer);

    const cleanedTable = tempContainer.querySelector("table");

    if (!cleanedTable) {
        throw new Error("No se pudo preparar la tabla limpia para exportación.");
    }

    const { headers } = extractTableDataFromClonedTable(cleanedTable);

    if (!headers.length) {
        throw new Error("La tabla no contiene encabezados exportables.");
    }

    if (title) {
        const tituloPDF = document.createElement("h5");
        tituloPDF.textContent = title;
        tituloPDF.style.textAlign = "center";
        tituloPDF.style.marginBottom = "15px";

        tempContainer.prepend(tituloPDF);
    }

    const { jsPDF } = window.jspdf;

    const backgroundPage = window.getComputedStyle(document.body).backgroundColor;

    // Detectar automáticamente orientación y tamaño
    const tableWidth = cleanedTable.scrollWidth;

    let orientation = "p";
    let format = "a4";

    if (tableWidth > 1200) {
        orientation = "l";
    }

    if (tableWidth > 2200) {
        orientation = "l";
        format = "a3";
    }

    console.log(
        `Exportando PDF: ${format.toUpperCase()} ${orientation === "l" ? "Landscape" : "Portrait"
        } (ancho tabla: ${tableWidth}px)`
    );

    const pdf = new jsPDF(
        orientation,
        "mm",
        format
    );

    // Obtener dimensiones reales según formato/orientación
    const pageWidth = pdf.internal.pageSize.getWidth();
    const pageHeight = pdf.internal.pageSize.getHeight();

    const margin = 10;

    // Ancho utilizable dentro de márgenes
    const imgWidth = pageWidth - (margin * 2);

    cleanedTable.classList.add('app-grid-card');
    document.body.appendChild(tempContainer);

    const canvas = await html2canvas(tempContainer, {
        scale: 2,
        useCORS: true,
        backgroundColor: backgroundPage,
        logging: false,
        removeContainer: true,
        scrollX: 0,
        scrollY: 0,
        windowWidth: tempContainer.scrollWidth,
        windowHeight: tempContainer.scrollHeight + 100
    });

    tempContainer.remove();

    const imgData = canvas.toDataURL("image/png", 1.0);

    const imgHeight = (canvas.height * imgWidth) / canvas.width;

    let position = 0;
    let heightLeft = imgHeight;

    pdf.setFillColor(backgroundPage);
    pdf.rect(0, 0, pageWidth, pageHeight, "F");

    pdf.addImage(
        imgData,
        "PNG",
        margin,
        position,
        imgWidth,
        imgHeight,
        "",
        "FAST"
    );

    heightLeft -= pageHeight;

    while (heightLeft > 0) {

        position = heightLeft - imgHeight;

        pdf.addPage();

        pdf.setFillColor(backgroundPage);
        pdf.rect(0, 0, pageWidth, pageHeight, "F");

        pdf.addImage(
            imgData,
            "PNG",
            margin,
            position,
            imgWidth,
            imgHeight,
            "",
            "FAST"
        );

        heightLeft -= pageHeight;
    }

    // Descarga
    pdf.save(`${fileName}.pdf`);

    // Vista previa en pestaña nueva
    window.open(pdf.output("bloburl"), "_blank");
}

function collectDocumentStylesHtml() {
    const nodes = Array.from(document.querySelectorAll("link[rel='stylesheet'], style"));

    return nodes
        .map(node => node.outerHTML)
        .join("\n");

}