<script lang="ts">
	import { budget, loadBudget, addBudget } from '../stores/budgetStore';
	import { getCurrentMonth } from '../lib/utils/date';

	let { month = $bindable() } = $props();
	let income = $state($budget.income);
	let loading = $state(false);
	let error = $state('');
	let success = $state('');

	const expenses = $derived($budget.income - $budget.remaining);

	$effect(() => {
		if (month) {
			loadBudget(month);
		}
	});

	async function handleSave() {
		if (!month || income <= 0) {
			error = 'Fyll i månad och inkomst';
			return;
		}
		loading = true;
		error = '';
		success = '';
		try {
			await addBudget(month, income);
			success = 'Budget sparad!';
		} catch (e) {
			error = 'Något gick fel';
			success = '';
		} finally {
			loading = false;
		}
	}
</script>

<div class="form-card">
	<h2>Månad</h2>

	<div class="row">
		<input type="month" bind:value={month} />
	</div>

	{#if error}<p class="error">{error}</p>{/if}
	{#if success}<p class="success">{success}</p>{/if}

	<div class="row">
		<h2>Månadsinkomst</h2>

		<input type="number" placeholder="0 kr" bind:value={income} />
	</div>

	<div class="summary">
		<div class="summary-row">
			<span>inkomst</span>
			<span class="green">{income} kr</span>
		</div>
		<div class="summary-row">
			<span>Utgifter</span>
			<span class="red">{expenses} kr</span>
		</div>
		<div class="summary-row total">
			<span>Kvar</span>
			<span>{$budget.remaining} kr</span>
		</div>
	</div>
	<button onclick={handleSave} disabled={loading}>
		{loading ? 'Sparar...' : 'Spara budget'}
	</button>
</div>

<style>
	.form-card {
		background: var(--color-surface);
		border-radius: var(--radius-card);
		padding: 1.5rem;
		border: 1px solid var(--color-border);
		display: flex;
		flex-direction: column;
		gap: 1rem;
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

	input {
		padding: 0.65rem 0.9rem;
		border: 1px solid var(--color-border);
		border-radius: var(--radius-control);
		font-size: 0.95rem;
		outline: none;
		transition: border 0.15s;
		background: var(--color-input);
		color: var(--color-text);
	}

	input:focus {
		border-color: var(--color-primary);
	}

	.summary {
		border-top: 1px solid var(--color-border);
		padding-top: 1rem;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.summary-row {
		display: flex;
		justify-content: space-between;
		font-size: 0.9rem;
		color: var(--color-muted);
	}

	.summary-row.total {
		font-weight: 600;
		color: var(--color-text);
		font-size: 1rem;
	}

	.green {
		color: var(--color-success);
		font-weight: 600;
	}
	.red {
		color: var(--color-danger);
		font-weight: 600;
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
