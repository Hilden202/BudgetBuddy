import { login as loginAPI, register as registerAPI } from "./api";
import { writable } from "svelte/store";
import { browser } from "$app/environment";

export const token = writable<string | null>(browser ? localStorage.getItem('token') : null);

export const currentUser = writable<string | null>(null);

export async function login(email: string, password: string) {
    const data = await loginAPI(email, password);
    localStorage.setItem('token', data.token);
    token.set(data.token);
    currentUser.set(email);
}

export function logout() {
    localStorage.removeItem('token');
    token.set(null);
    currentUser.set(null);
}

export async function register(email: string, password: string) {
    const data = await registerAPI(email, password);
}