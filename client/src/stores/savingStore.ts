import { writable } from "svelte/store";
import { getSavings, updateSavingsGoal } from "./api";

interface SavingState {
    month: string;
    monthAmount: number;
    totalAmount: number;
    savingsGoal: number;
}

export const savings = writable<SavingState>({
    month: '',
    monthAmount: 0,
    totalAmount: 0,
    savingsGoal: 0
});

export async function loadSavings(month: string) {
    const data = await getSavings(month);

    savings.set({
        month,
        monthAmount: data.monthAmount ?? 0,
        totalAmount: data.totalAmount ?? 0,
        savingsGoal: data.savingsGoal ?? 0
    });
}

export async function saveSavingsGoal(month: string, savingsGoal: number) {
    await updateSavingsGoal(savingsGoal);
    await loadSavings(month);
}