const BASE_URL = import.meta.env.VITE_API_URL;

// Hjälpfunktion som lägger till JWT-token automatiskt
function headers() {
    const token = localStorage.getItem('token');
    return {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
    };
}

// ─── AUTH ────────────────────────────────────────────
export async function login(email: string, password: string) {
    const res = await fetch(`${BASE_URL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    return res.json(); // { token: "..." }
}

export async function register(email: string, password: string) {
    const res = await fetch(`${BASE_URL}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    return res.json();
}

// ─── BUDGET ──────────────────────────────────────────
export async function getBudget(month: string) {
    const res = await fetch(`${BASE_URL}/budget/${month}`, {
        headers: headers()
    });
    return res.json(); // { id, month, income, expenses[] }
}

export async function createBudget(month: string, income: number) {
    const res = await fetch(`${BASE_URL}/budget`, {
        method: 'POST',
        headers: headers(),
        body: JSON.stringify({ month, income })
    });
    return res.json();
}

// ─── EXPENSES ─────────────────────────────────────────
export async function getExpenses(budgetId: string) {
    const res = await fetch(`${BASE_URL}/expenses/${budgetId}`, {
        headers: headers()
    });
    return res.json(); // [{ id, category, amount, description }]
}

export async function createExpense(budgetId: string, category: string, amount: number, description: string) {
    const res = await fetch(`${BASE_URL}/expenses`, {
        method: 'POST',
        headers: headers(),
        body: JSON.stringify({ budgetId, category, amount, description })
    });
    return res.json();
}

export async function deleteExpense(id: string) {
    await fetch(`${BASE_URL}/expenses/${id}`, {
        method: 'DELETE',
        headers: headers()
    });
}

export async function updateExpense(id: string, amount: number, category: string, description: string) {
    await fetch(`${BASE_URL}/expenses/${id}`, {
        method: 'PUT',
        headers: headers(),
        body: JSON.stringify({ amount, category, description })
    });
}

// ─── SAVINGS ──────────────────────────────────────────
export async function getSavings(userId: string) {
    const res = await fetch(`${BASE_URL}/savings/${userId}`, {
        headers: headers()
    });
    return res.json(); // [{ id, month, amount, goalAmount }]
}

export async function getTotalSavings(userId: string) {
    const res = await fetch(`${BASE_URL}/savings/${userId}/total`, {
        headers: headers()
    });
    return res.json(); // { totalAmount }
}

export async function createSavings(userId: string, month: string, amount: number, goalAmount: number) {
    const res = await fetch(`${BASE_URL}/savings`, {
        method: 'POST',
        headers: headers(),
        body: JSON.stringify({ userId, month, amount, goalAmount })
    });
    return res.json();
}

export async function deleteSavings(id: string) {
    await fetch(`${BASE_URL}/savings/${id}`, {
        method: 'DELETE',
        headers: headers()
    });
}

export async function updateSavings(id: string, amount: number, goalAmount: number) {
    await fetch(`${BASE_URL}/savings/${id}`, {
        method: 'PUT',
        headers: headers(),
        body: JSON.stringify({ amount, goalAmount })
    });
}