(function () {
    let theme = localStorage.getItem("theme-mode");

    if (!["light", "dark", "system"].includes(theme)) {
        theme = "system";
    }

    const root = document.documentElement;

    if (theme === "system") {
        const systemDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

        root.setAttribute("data-bs-theme", systemDark ? "dark" : "light");

        root.setAttribute("data-theme-mode", "system");
    }
    else {
        root.setAttribute("data-bs-theme", theme);

        root.setAttribute("data-theme-mode", theme);
    }

    const media = window.matchMedia(
        "(prefers-color-scheme: dark)"
    );

    media.addEventListener("change", () => {

        const current =
            localStorage.getItem("theme-mode") || "system";

        if (current === "system") {
            window.outlander.applyTheme();
        }

    });
})();

window.outlander = {

    initTooltips: function () {
        if (!window.bootstrap) return;

        document.querySelectorAll('[data-bs-toggle="tooltip"], [data-bs-title]').forEach(el => {
            const existing = bootstrap.Tooltip.getInstance(el);

            if (existing) {
                existing.hide();
                existing.dispose();
            }

            bootstrap.Tooltip.getOrCreateInstance(el);
        });

        document.querySelectorAll('.tooltip.show').forEach(el => {
            const instance = bootstrap.Tooltip.getInstance(el);
            if (instance) {
                instance.hide();
            }
        });
    },
    clearTooltips: function () {
        if (!window.bootstrap) return;

        document.querySelectorAll('.tooltip.show').forEach(el => {
            const instance = bootstrap.Tooltip.getInstance(el);
            if (instance) {
                instance.hide();
            }
        });
    },
};