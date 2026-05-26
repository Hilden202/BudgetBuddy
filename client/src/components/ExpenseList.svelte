<script lang="ts">
import { budget } from "../stores/budgetStore";
import { addExpense,removeExpense, editExpense } from "../stores/expenseStore";

let category = $state('');
let amount = $state(0);
let description = $state('');
let loading = $state(false);
let error = $state('');

//Håller koll på vilken utgift som redigeras
let editingId = $state<string | null>(null);
let editCategory = $state('');
let editAmount = $state(0);
let editDescription = $state('');

let totalExpenses = $derived(
    $budget.expenses.reduce((sum, e) => sum + e.amount, 0)
);

async function handleAdd() {
    if(!category || amount <= 0) { error = 'Fyll i kategori och belopp'; return; }
    if(!$budget.id) { error = 'Välj en månad först'; return; }

    loading = true;
    error = '';
    try {
        await addExpense($budget.id, category, amount, description);
        category = '';
        amount = 0;
        description = '';
    } catch (e) {
        error = 'Något gick fel';
    } finally {
        loading = false;
    }
}

async function handleDelete(id:string) {
    try {
        await removeExpense(id)
    } catch (e) {
        error = 'Kunde inte ta bort utgiften'
    }
    
}

function startEdit(expense: { id: string, category: string; amount: number; description: string; }) {
    editingId = expense.id;
    editCategory = expense.category;
    editAmount = expense.amount;
    editDescription = expense.description;
}

async function handleEdit() {
    if(!editCategory || editAmount <= 0) { error = 'Fyll i kategori och belopp'; return; }
    loading = true;
    error = '';
    try {
        await editExpense(editingId!, editAmount, editCategory, editDescription);
        editingId = null;
    } catch (e) {
        error = 'Kunde inte uppdatera utgiften';
    } finally {
        loading = false;
    }
    
}

function cancelEdit() {
    editingId = null;
}
</script>

<div class = "expense-card">
    <h2>Utgifter</h2>

    {#if error}<p class="error">{error}</p>{/if}

    <!-- Lägg till formulär -->
     <div class="form">
        <input type="text"
        placeholder="Kategori (t.ex. Mat)"
        bind:value={category} />

        <input type="number"
        placeholder="Belopp"
        bind:value={amount} />

        <input type="text"
        placeholder="Beskrivning (valfri)"
        bind:value={description} /> 

        <button onclick={handleAdd} disabled={loading}>
        {loading ? "Läger till..." : "+ lägg till utgift"}
        </button>
     </div>

     {#if $budget.expenses.length === 0}
     <p class="empty">Inga utgifter än</p>
     {:else}
        <ul class="list">
        {#each $budget.expenses as expense}
            <li class="list-item">
                {#if editingId === expense.id}
                <!--Redigeringsläge-->
                <div class="edit-form">
                    <input type="text"
                    bind:value={editCategory} />
                    
                    <input type="number"
                    bind:value={editAmount} />

                    <input type="text"
                    placeholder="beskrivning"
                    bind:value={editDescription} />

                    <div class="edit-actions">
                        <button onclick={handleEdit} disabled={loading}>Spara</button>
                        <button class="cancel-btn" onclick={cancelEdit}>Avbryt</button>
                    </div>
                </div>
                {:else}
                <!--Visningsläge-->
                <div class="item-info">
                    <span class="item-category">{expense.category}</span>
                    {#if expense.description}
                    <span class="item-desc">{expense.description}</span>
                    {/if}
                </div>
                <div class="item-right">
                    <span class="item-amount">-{expense.amount} kr</span>
                    <button class="edit-btn" onclick={() => startEdit(expense)}>Redigera</button>
                    <button class="delete-btn" onclick={() => handleDelete(expense.id)}>Ta Bort</button>
                </div>
                {/if}
            </li>
            {/each}
        </ul>
        
        <div class="total-row">
            <span>Totalt</span>
            <span class="red">{totalExpenses}</span>
        </div>
    {/if}
</div>

<style>
    .expense-card {
        background: white;
        border-radius: 12px;
        padding: 1.5rem;
        border: 1px solid #e5e7eb;
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    h2 { font-size: 1rem; font-weight: 600; color: #1f2937; }

    .form, .edit-form {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
        width: 100%;
    }

    input {
        padding: 0.65rem 0.9rem;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        font-size: 0.95rem;
        outline: none;
        transition: border 0.15s;
    }

    input:focus { border-color: #4f46e5; }

    button {
        padding: 0.7rem;
        background: #4f46e5;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 0.95rem;
        font-weight: 600;
        cursor: pointer;
        transition: background 0.15s;
    }

    button:hover:not(:disabled) { background: #4338ca; }
    button:disabled { opacity: 0.6; cursor: not-allowed; }

    .edit-actions {
        display: flex;
        gap: 0.5rem;
    }

    .cancel-btn {
        background: #f3f4f6;
        color: #6b7280;
    }

    .cancel-btn:hover { background: #e5e7eb; }

    .list { list-style: none; display: flex; flex-direction: column; gap: 0.5rem; }

    .list-item {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 0.75rem 1rem;
        background: #f9fafb;
        border-radius: 8px;
    }

    .item-info { display: flex; flex-direction: column; gap: 0.15rem; }
    .item-category { font-size: 0.9rem; font-weight: 600; color: #1f2937; }
    .item-desc { font-size: 0.8rem; color: #9ca3af; }
    .item-right { display: flex; align-items: center; gap: 0.75rem; }
    .item-amount { font-weight: 600; color: #dc2626; font-size: 0.9rem; }

    .edit-btn {
        background: none;
        color: #9ca3af;
        font-size: 0.9rem;
        padding: 0.25rem 0.5rem;
        border-radius: 4px;
    }

    .edit-btn:hover { background: #ede9fe; color: #4f46e5; }

    .delete-btn {
        background: none;
        color: #9ca3af;
        font-size: 0.8rem;
        padding: 0.25rem 0.5rem;
        border-radius: 4px;
    }

    .delete-btn:hover { background: #fee2e2; color: #dc2626; }

    .total-row {
        display: flex;
        justify-content: space-between;
        font-weight: 600;
        font-size: 0.95rem;
        border-top: 1px solid #f3f4f6;
        padding-top: 0.75rem;
    }

    .red { color: #dc2626; }
    .empty { color: #9ca3af; font-size: 0.9rem; }
    .error { color: #dc2626; font-size: 0.85rem; }
</style>