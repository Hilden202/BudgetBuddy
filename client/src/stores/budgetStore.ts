import { writable } from "svelte/store";
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
    const data = await getBudget(month);
    budget.set({
        id: data.id,
        month: data.month,
        income: data.income,
        expenses: data.expenses,
        remaining: data.income - data.expenses.reduce((sum: number, e: Expense) => sum + e.amount, 0)
    });
}

export async function addBudget(month: string, income: number) {
    const data = await createBudget(month, income);
    budget.set({
        id: data.id,
        month: data.month,
        income: data.income,
        expenses: [],
        remaining: data.income
    });
}
