export function getNavMenuCollapsed() {
    return localStorage.getItem("outlander-nav-menu-collapsed") === "true";
}

export function setNavMenuCollapsed(value) {
    localStorage.setItem("outlander-nav-menu-collapsed", value ? "true" : "false");
}

export function getWindowWidth() {
    return window.innerWidth;
}

export function registerResizeCallback(dotNetRef) {
    let timeoutId;

    window.addEventListener("resize", function () {
        clearTimeout(timeoutId);

        timeoutId = setTimeout(() => {
            dotNetRef.invokeMethodAsync("OnBrowserResize", window.innerWidth);
        }, 120);
    });
}