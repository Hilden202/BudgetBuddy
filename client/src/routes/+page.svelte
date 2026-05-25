<script lang="ts">
import { goto } from '$app/navigation';
import { onMount } from 'svelte';

let loading = $state(true);

onMount(() => {
    const savedToken = localStorage.getItem('token');
    if (!savedToken) { goto('/login'); return; }
    loading = false;
});
</script>

{#if loading}
    <p>Laddar...</p>
{:else}
    <div class="dashboard">
        <h1 class="page-title">Dashboard</h1>
        <p class="subtitle">Översikt för maj 2025</p>

        <!-- Stat-kort -->
        <div class="cards">
            <div class="card">
                <span class="card-label">Tillgängligt belopp</span>
                <span class="card-value green">0 kr</span>
                <span class="card-sub">av 0 kr</span>
            </div>
            <div class="card">
                <span class="card-label">Totala utgifter</span>
                <span class="card-value blue">0 kr</span>
                <span class="card-sub">av 0 kr</span>
            </div>
            <div class="card">
                <span class="card-label">Sparat denna månad</span>
                <span class="card-value purple">0 kr</span>
                <span class="card-sub">av 0 kr mål</span>
            </div>
            <div class="card">
                <span class="card-label">Återstår av sparmål</span>
                <span class="card-value orange">0 kr</span>
                <span class="card-sub">av 0 kr</span>
            </div>
        </div>

        <!-- Nedre sektion -->
        <div class="bottom">
            <div class="panel">
                <h2 class="panel-title">Senaste utgifter</h2>
                <p class="empty">Inga utgifter ännu</p>
            </div>
            <div class="panel">
                <h2 class="panel-title">Sparmål</h2>
                <p class="empty">Inga sparmål ännu</p>
            </div>
        </div>
    </div>
{/if}

<style>
    .dashboard {
        max-width: 1100px;
    }

    .page-title {
        font-size: 1.8rem;
        font-weight: 700;
        margin-bottom: 0.25rem;
    }

    .subtitle {
        color: #6b7280;
        font-size: 0.9rem;
        margin-bottom: 2rem;
    }

    /* ── Cards ── */
    .cards {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 1rem;
        margin-bottom: 2rem;
    }

    .card {
        background: white;
        border-radius: 12px;
        padding: 1.25rem 1.5rem;
        display: flex;
        flex-direction: column;
        gap: 0.25rem;
        border: 1px solid #e5e7eb;
    }

    .card-label {
        font-size: 0.8rem;
        color: #6b7280;
        font-weight: 500;
    }

    .card-value {
        font-size: 1.6rem;
        font-weight: 700;
    }

    .card-sub {
        font-size: 0.8rem;
        color: #9ca3af;
    }

    .green  { color: #16a34a; }
    .blue   { color: #2563eb; }
    .purple { color: #7c3aed; }
    .orange { color: #ea580c; }

    /* ── Bottom panels ── */
    .bottom {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1rem;
    }

    .panel {
        background: white;
        border-radius: 12px;
        padding: 1.5rem;
        border: 1px solid #e5e7eb;
    }

    .panel-title {
        font-size: 1rem;
        font-weight: 600;
        margin-bottom: 1rem;
        color: #1f2937;
    }

    .empty {
        color: #9ca3af;
        font-size: 0.9rem;
    }
</style>