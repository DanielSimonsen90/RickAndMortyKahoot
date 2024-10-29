import { LOCAL_STORAGE_KEYS } from "./constants.js";
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
