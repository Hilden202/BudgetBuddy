<script lang="ts">
	import { budget, loadBudget, addBudget } from '../stores/budgetStore';
	import { getCurrentMonth } from '../lib/utils/date';
	let month = $state(getCurrentMonth());
	let income = $state(0);
	let loading = $state(false);
	let error = $state('');
	let success = $state('');

	async function handleLoad() {
		if (!month) return;
		loading = true;
		error = '';
		success = '';
		try {
			await loadBudget(month);
			income = $budget.income;
			success = `Budget för ${month} laddad!`;
		} catch (e) {
			error = 'Ingen budget hittades för den månaden';
			success = '';
		} finally {
			loading = false;
		}
	}

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
		<input type="month" bind:value={month} onchange={handleLoad} />
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
			<span class="green">{$budget.income} kr</span>
		</div>
		<div class="summary-row">
			<span>Utgifter</span>
			<span class="red">{$budget.income - $budget.remaining} kr</span>
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
		background: white;
		border-radius: 12px;
		padding: 1.5rem;
		border: 1px solid #e5e7eb;
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	h2 {
		font-size: 1rem;
		font-weight: 600;
		color: #1f2937;
	}

	.row {
		display: flex;
		flex-direction: column;
		gap: 0.4rem;
	}

	input {
		padding: 0.65rem 0.9rem;
		border: 1px solid #e5e7eb;
		border-radius: 8px;
		font-size: 0.95rem;
		outline: none;
		transition: border 0.15s;
	}

	input:focus {
		border-color: #4f46e5;
	}

	.summary {
		border-top: 1px solid #f3f4f6;
		padding-top: 1rem;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.summary-row {
		display: flex;
		justify-content: space-between;
		font-size: 0.9rem;
		color: #6b7280;
	}

	.summary-row.total {
		font-weight: 600;
		color: #1f2937;
		font-size: 1rem;
	}

	.green {
		color: #16a34a;
		font-weight: 600;
	}
	.red {
		color: #dc2626;
		font-weight: 600;
	}

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

	button:hover:not(:disabled) {
		background: #4338ca;
	}
	button:disabled {
		opacity: 0.6;
		cursor: not-allowed;
	}

	.error {
		color: #dc2626;
		font-size: 0.85rem;
	}
	.success {
		color: #16a34a;
		font-size: 0.85rem;
	}
</style>
