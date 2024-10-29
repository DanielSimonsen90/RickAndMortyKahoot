import { LOCAL_STORAGE_KEYS } from "./constants.js";
import type { User } from "./models/User.js";

type SessionStorageMap = {
  [LOCAL_STORAGE_KEYS.USER]: User;
  [LOCAL_STORAGE_KEYS.IS_HOST]: boolean;
}

export function getFromSessionStorage<K extends keyof SessionStorageMap>(key: K): SessionStorageMap[K] | null {
  const json = sessionStorage.getItem(key);
  if (typeof json !== 'string') return null;
  return JSON.parse(json) as SessionStorageMap[K];
}
export function saveToSessionStorage<K extends keyof SessionStorageMap>(key: K, value: SessionStorageMap[K]) {
  sessionStorage.setItem(key, JSON.stringify(value));
}

export function getCurrentUser() {
  return getFromSessionStorage(LOCAL_STORAGE_KEYS.USER);
}
export function saveCurrentUser(user: User) {
  return saveToSessionStorage(LOCAL_STORAGE_KEYS.USER, user);
}

export function navigate(url: string) {
  if (!url.includes('?userId')) {
    const user = getCurrentUser();
    if (user) url += `?userId=${user.id}`;
  }

  console.log(`Navigating to ${url}`);
  window.location.href = url;
}