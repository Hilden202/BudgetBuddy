import { getExpenses, createExpense, deleteExpense, updateExpense } from "./api";
import { budget } from "./budgetStore";


interface Expense {
    id: string;
    category: string;
    amount: number;
    description: string;
}

export async function loadExpenses(budgetId:string) {
    const data = await getExpenses(budgetId)
    budget.update(b => ({
        ...b,
        expenses: data,
        remaining: b.income - data.reduce((sum: number, e: Expense) => sum + e.amount, 0)

    }));
    
}

export async function addExpense(budgetId: string, category: string, amount: number, description: string ) {
    const newExpense = await createExpense(budgetId, category, amount, description);
    budget.update(b => ({
        ...b,
       expenses: [...b.expenses, newExpense],
       remaining: b.remaining - newExpense.amount
        
    }));
    
}

export async function removeExpense(id: string) {
    await deleteExpense(id);
    budget.update(b => {
        const expenses = b.expenses.filter(e => e.id !== id)
        const remaining = b.income - expenses.reduce((sum, e) => sum + e.amount, 0);
        return {...b, expenses, remaining}
    });

}

export async function editExpense(id: string, amount: number, category: string, description: string) {
    await updateExpense(id, amount, category, description);
    budget.update(b => ({
        ...b,
        expenses: b.expenses.map(e => e.id === id ? { ...e, amount, category, description} : e),
        remaining: b.income - b.expenses.reduce((sum, e) => sum + (e.id === id ? amount : e.amount), 0)
    }));
    
}
