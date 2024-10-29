import { CRITICAL_TIMEOUT_SECONDS, LOCAL_STORAGE_KEYS } from "./constants.js";
export function getFromSessionStorage(key) {
    const json = sessionStorage.getItem(key);
    if (typeof json !== 'string')
        return null;
    return JSON.parse(json);
}
export function saveToSessionStorage(key, value) {
    sessionStorage.setItem(key, JSON.stringify(value));
}
export function getCurrentUser() {
    return getFromSessionStorage(LOCAL_STORAGE_KEYS.USER);
}
export function saveCurrentUser(user) {
    return saveToSessionStorage(LOCAL_STORAGE_KEYS.USER, user);
}
export function navigate(url) {
    if (!url.includes('?userId')) {
        const user = getCurrentUser();
        if (user)
            url += `?userId=${user.id}`;
    }
    console.log(`Navigating to ${url}`);
    window.location.href = url;
}
export function timer({ timeoutSeconds, callback, showCritical, selector }) {
    const timer = $(selector);
    if (!timer)
        throw new Error('Timer element not found');
    timer.addClass('active');
    timer.css('--duration', `${timeoutSeconds}s`);
    const timeouts = {
        main: setTimeout(() => {
            window.stopTimer();
        }, timeoutSeconds * 1000),
        critical: showCritical ? setTimeout(() => {
            timer.addClass('critical');
            setTimeout(() => timer.removeClass('critical'), CRITICAL_TIMEOUT_SECONDS * 1000);
        }, (timeoutSeconds - CRITICAL_TIMEOUT_SECONDS) * 1000) : null,
    };
    window.stopTimer = () => {
        timer.css('--duration', '0s');
        timer.removeClass('active');
        callback?.();
        clearTimeout(timeouts.main);
        if (timeouts.critical)
            clearTimeout(timeouts.critical);
    };
}
