import { writable, get } from "svelte/store";
import { getBudget, createBudget, updateBudget, deleteBudget } from "./api";

interface Expense {
    id: string;
    category: string;
    amount: number;
    description: string;
}

interface BudgetState {
    id: string;
    month: string;
    income: number;
    expenses: Expense[];
    remaining: number;
}

const initialState: BudgetState = {
    id: '',
    month: '',
    income: 0,
    expenses: [],
    remaining: 0
};

export const budget = writable<BudgetState>(initialState);

export async function loadBudget(month: string) {
    // Rensa alltid state när man byter månad
    budget.set({ ...initialState, month });

    try {
        const data = await getBudget(month);
        budget.set({
            id: data.id,
            month: data.month,
            income: data.income,
            expenses: data.expenses ?? [],
            remaining: data.income - (data.expenses ?? []).reduce((sum: number, e: Expense) => sum + e.amount, 0)
        });
    } catch {
        // Ingen budget finns för månaden — state är redan nollställd
    }
}

export async function addBudget(month: string, income: number) {
    const current = get(budget);

    if (current.id && current.month === month) {
        // Budget finns redan → uppdatera
        await updateBudget(current.month, income);
        budget.update(b => ({
            ...b,
            income,
            remaining: income - b.expenses.reduce((sum, e) => sum + e.amount, 0)
        }));
    } else {
        // Ingen budget → skapa ny
        const data = await createBudget(month, income);
        budget.set({
            id: data.id,
            month: data.month,
            income: data.income,
            expenses: [],
            remaining: data.income
        });
    }
}
