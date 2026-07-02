
export function getTheme () {
    return localStorage.getItem("theme-mode") || "system";
}

export function setTheme (theme) {
    localStorage.setItem("theme-mode", theme);
    applyTheme(theme);
}

function applyTheme (theme) {
    const root = document.documentElement;

    if (theme === "system") {
        const systemDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
        root.setAttribute("data-bs-theme", systemDark ? "dark" : "light");
        root.setAttribute("data-theme-mode", "system");
        return;
    }

    root.setAttribute("data-bs-theme", theme);
    root.setAttribute("data-theme-mode", theme);
}

export function initTheme () {
    const theme = getTheme();
    applyTheme(theme);

    const media = window.matchMedia("(prefers-color-scheme: dark)");
    media.addEventListener("change", () => {
        const currentTheme = getTheme();
        if (currentTheme === "system") {
            applyTheme("system");
        }
    });
}