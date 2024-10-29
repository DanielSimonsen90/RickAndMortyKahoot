import { CRITICAL_TIMEOUT_SECONDS, SESSION_STORAGE_KEYS } from "./constants.js";
/**
 * Get the value stored in the session storage.
 * @param key Storage key
 * @returns The value stored in the session storage
 */
export function getFromSessionStorage(key) {
    const json = sessionStorage.getItem(key);
    if (typeof json !== 'string')
        return null;
    return JSON.parse(json);
}
/**
 * Save the value to the session storage.
 * @param key Storage key
 * @param value Value to save
 */
export function saveToSessionStorage(key, value) {
    if (value === undefined)
        return sessionStorage.removeItem(key);
    sessionStorage.setItem(key, JSON.stringify(value));
}
/**
 * Get the current user from the session storage.
 * @returns The current user
 */
export function getCurrentUser() {
    return getFromSessionStorage(SESSION_STORAGE_KEYS.USER);
}
/**
 * Save the current user to the session storage.
 * @param user User to save
 */
export function saveCurrentUser(user) {
    return saveToSessionStorage(SESSION_STORAGE_KEYS.USER, user);
}
/**
 * Navigate to the specified URL. This also auto-appends the current userId to the URL if it doesn't already exist.
 * @param url URL to navigate to
 */
export function navigate(url) {
    if (!url.includes('?userId')) {
        const user = getCurrentUser();
        if (user)
            url += `?userId=${user.id}`;
    }
    window.location.href = url;
}
/**
 * Start a timer with the specified options.
 * @param props Arguments for the timer
 */
export function timer({ timeoutSeconds, callback, showCritical, selector }) {
    const timer = $(selector);
    if (!timer)
        throw new Error('Timer element not found');
    timer.addClass('active');
    timer.css('--duration', `${timeoutSeconds}s`);
    const timeouts = {
        /**
         * The main timer that will stop the timer when it runs out.
         */
        main: setTimeout(() => {
            window.stopTimer();
        }, timeoutSeconds * 1000),
        /**
         * The critical timer, if enabled, that will add a critical class to the timer when it is nearing the end.
         */
        critical: showCritical ? setTimeout(() => {
            timer.addClass('critical');
            setTimeout(() => timer.removeClass('critical'), CRITICAL_TIMEOUT_SECONDS * 1000);
        }, (timeoutSeconds - CRITICAL_TIMEOUT_SECONDS) * 1000) : null,
    };
    /**
     * Store global stopTimer function to stop the timer anywhere
     */
    window.stopTimer = () => {
        timer.css('--duration', '0s');
        timer.removeClass('active');
        callback?.();
        clearTimeout(timeouts.main);
        if (timeouts.critical)
            clearTimeout(timeouts.critical);
    };
}
