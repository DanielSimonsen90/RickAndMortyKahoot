export function getCurrentUser() {
    const json = localStorage.getItem('user');
    if (typeof json !== 'string')
        return null;
    return JSON.parse(json);
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
