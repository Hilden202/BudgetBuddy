<script lang="ts">
import { goto } from '$app/navigation';
import { onMount } from 'svelte';

let loading = $state(true);

let income = $state(0);

// Standardkategorier
let categories = $state([
    {
        name: 'Hyra',
        amount: 0
    },
    {
        name: 'Mat',
        amount: 0
    },
    {
        name: 'Transport',
        amount: 0
    },
    {
        name: 'Prenumerationer',
        amount: 0
    },
    {
        name: 'Underhållning',
        amount: 0
    },
    {
        name: 'Sparande',
        amount: 0
    }
]);

let totalExpenses = $derived(
    categories.reduce((sum, category) => sum + category.amount, 0)
);

onMount(async () => {
    const saveToken = localStorage.getItem('token');

    if (!saveToken) {
        goto('/login');
        return;
    }

    loading = false;
});

function logout() {
        localStorage.removeItem('token');
        goto('/login');
    }

</script>

{#if loading}

<p>laddar...</p>

{:else }

<div class="container">
    <h1>BudgetBuddy</h1>

    <button onclick={logout}>Logout</button>

    <h2>Månadsinkomst</h2>

    <input
    type="number"
    placeholder="Månadsinkomst"
    bind:value={income}
    />

    <hr>

    <h2>Utgifter</h2>
    
{#each categories as category}

    <div class="expense-row">

        <label>
            {category.name}
        </label>

        <input
            type="number"
            placeholder={category.name}
            bind:value={category.amount}
        />

    </div>

{/each}

<h2>
    Totala utgifter: {totalExpenses} kr
</h2>

</div>
{/if}

<style>

    :global(body) {
        margin: 0;
        font-family: Arial, Helvetica, sans-serif;
        background: #f5f5f5;
        color: #1f2937;
    }

    .container {
        max-width: 900px;
        margin: 0 auto;
        padding: 2rem;
    }

    h1 {
        font-size: 3rem;
        margin-bottom: 2rem;
    }

    h2 {
        margin-bottom: 1rem;
    }

    hr {
        margin: 2rem 0;
        border: none;
        border-top: 1px solid #ddd;
    }

    .expense-row {
        margin-bottom: 1.5rem;
    }

    label {
        display: block;
        margin-bottom: 0.5rem;
        font-weight: bold;
    }

    input {
        width: 100%;
        padding: 1rem;
        margin-bottom: 1rem;
        border: none;
        border-radius: 12px;
        background: white;
        box-shadow: 0 2px 8px rgba(0,0,0,0.05);
        font-size: 1rem;
        box-sizing: border-box;
    }

    input:focus {
        outline: 2px solid #4f46e5;
    }

    button {
        border: none;
        border-radius: 999px;
        background: #4f46e5;
        color: white;
        padding: 0.9rem 1.5rem;
        cursor: pointer;
        font-size: 1rem;
        font-weight: bold;
        margin-bottom: 2rem;
        transition: 0.2s;
    }

    button:hover {
        background: #6366f1;
    }

</style>