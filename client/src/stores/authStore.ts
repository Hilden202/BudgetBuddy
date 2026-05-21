import { login as loginAPI } from "./api";
import { writable } from "svelte/store";
import { browser } from "$app/environment";

export const token = writable<string | null>(browser ? localStorage.getItem('token') : null);

export async function login(email: string, password: string) {
    const data = await loginAPI(email, password);
    localStorage.setItem('token', data.token);
    token.set(data.token);
}

export function logout() {
    localStorage.removeItem('token');
    token.set(null);
}
