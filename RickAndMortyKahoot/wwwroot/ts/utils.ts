import { User } from "./models/User";
const LOCAL_STORAGE_KEYS = {
  USER: 'user',
};
export function getCurrentUser() {
  const json = localStorage.getItem(LOCAL_STORAGE_KEYS.USER);
  if (typeof json !== 'string') return null;
  return JSON.parse(json) as User;
}
export function saveCurrentUser(user: User) {
  localStorage.setItem(LOCAL_STORAGE_KEYS.USER, JSON.stringify(user));
}

export function navigate(url: string) {
  if (!url.includes('?userId')) {
    const user = getCurrentUser();
    if (user) url += `?userId=${user.id}`;
  }

  console.log(`Navigating to ${url}`);
  window.location.href = url;
}