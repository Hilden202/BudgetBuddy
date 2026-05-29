<script lang="ts">
	import { updateSavingsGoal } from '../stores/api';
	import { loadSavings, savings, saveSavingsGoal } from '../stores/savingStore';
	import { getCurrentMonth } from '../lib/utils/date';

	let month = $state(getCurrentMonth());
	let amount = $state(0);
	let goalAmount = $state(0);
	let error = $state('');
	let loading = $state(false);
	let success = $state('');

	let progress = $derived(($savings.totalAmount / $savings.savingsGoal) * 100);
	let remaining = $derived($savings.savingsGoal - $savings.totalAmount);

	async function handleLoad() {
		loading = true;
		error = '';
		success = '';
		try {
			await loadSavings(month);
		} catch (e) {
			error = 'Kunde inte ladda sparmål';
			success = '';
		} finally {
			loading = false;
		}
	}

	async function handleSaveGoal() {
		if (!month || goalAmount <= 0) {
			error = 'Ange en giltig månad och ett positivt målbelopp';
			success = '';
			return;
		}
		loading = true;
		error = '';
		success = '';
		try {
			await saveSavingsGoal(month, goalAmount);
			success = 'Sparmål uppdaterat';
		} catch (e) {
			error = 'Kunde inte uppdatera sparmål';
			success = '';
		} finally {
			loading = false;
		}
	}
</script>

<div class="savings-card">
	<h2>Sparande</h2>

    <div class="row">
        <label for="month">Månad</label>
        <input
            id="month"
            type="month"
            bind:value={month}
            onchange={handleLoad}
        />
    </div>

	{#if error}
		<p class="error">{error}</p>
	{/if}

    {#if success}
        <p class="success">{success}</p>
    {/if}

    <div class="summary">
        <div class="summary-row">
            <span>Sparat denna månad</span>
            <strong>{$savings.monthAmount} kr</strong>
        </div>

        <div class="summary-row">
            <span>Totalt sparat</span>
            <strong>{$savings.totalAmount} kr</strong>
        </div>

        <div class="summary-row">
            <span>Sparmål</span>
            <strong>{$savings.savingsGoal} kr</strong>
        </div>

        <div class="summary-row total">
            <span>Kvar till mål</span>
            <strong>{remaining} kr</strong>
        </div>
    </div>

    <div class="progress">
        <div class="progress-fill" style={`width: ${progress}%`}></div>
    </div>

    <div class="row">
        <label for="goal">Ändra sparmål</label>
        <input
            id="goal"
            type="number"
            min="0"
            bind:value={goalAmount}
            placeholder="0 kr"
        />
    </div>

    <button onclick={handleSaveGoal} disabled={loading || !month}>
        {loading ? 'Sparar...' : 'Spara sparmål'}
    </button>
</div>

<style>
    .savings-card {
        background: var(--color-surface);
        border-radius: var(--radius-card);
        padding: 1.5rem;
        border: 1px solid var(--color-border);
        display: flex;
        flex-direction: column;
        gap: 1rem;
        max-width: 520px;
    }

    h2 {
        font-size: 1rem;
        font-weight: 600;
        color: var(--color-text);
    }

    .row {
        display: flex;
        flex-direction: column;
        gap: 0.4rem;
    }

    label {
        font-size: 0.9rem;
        font-weight: 500;
        color: var(--color-muted);
    }

    input {
        padding: 0.65rem 0.9rem;
        border: 1px solid var(--color-border);
        border-radius: var(--radius-control);
        font-size: 0.95rem;
        outline: none;
        transition: border 0.15s;
    }

    input:focus {
        border-color: var(--color-primary);
    }

    .summary {
        border-top: 1px solid var(--color-border);
        padding-top: 1rem;
        display: flex;
        flex-direction: column;
        gap: 0.65rem;
    }

    .summary-row {
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 0.9rem;
        color: var(--color-muted);
    }

    .summary-row strong {
        color: var(--color-text);
        font-weight: 600;
    }

    .summary-row.total {
        font-size: 1rem;
        font-weight: 600;
        color: var(--color-text);
        border-top: 1px solid var(--color-border);
        padding-top: 0.75rem;
        margin-top: 0.25rem;
    }

    .progress {
        width: 100%;
        height: 10px;
        background: var(--color-border);
        border-radius: 999px;
        overflow: hidden;
    }

    .progress-fill {
        height: 100%;
        background: linear-gradient(90deg, #14532d 0%, #16a34a 55%, #07ca4f 100%);
        border-radius: 999px;
        transition: width 0.2s ease;
    }

    button {
        padding: 0.7rem;
        background: var(--color-primary);
        color: white;
        border: none;
        border-radius: var(--radius-control);
        font-size: 0.95rem;
        font-weight: 600;
        cursor: pointer;
        transition: background 0.15s;
    }

    button:hover:not(:disabled) {
        background: var(--color-primary-hover);
    }

    button:disabled {
        opacity: 0.6;
        cursor: not-allowed;
    }

    .error {
        color: var(--color-danger);
        font-size: 0.85rem;
    }

    .success {
        color: var(--color-success);
        font-size: 0.85rem;
    }
        
</style>