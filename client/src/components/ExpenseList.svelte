<script lang="ts">
	import { budget } from '../stores/budgetStore';
	import { addExpense, removeExpense, editExpense } from '../stores/expenseStore';

	const PLACEHOLDER_CATEGORIES = ['🏠 Hyra', '💡 El', '🍕 Mat', '🚗 Transport', '🎉 Nöjen', '💎 Sparande'];

	let error = $state('');
	let deleteMode = $state(false);
	let addMode = $state(false);
	let newCategory = $state('');
	let newAmount = $state(0);

	let inputValues = $state<Record<string, number>>({});

	$effect(() => {
		const values: Record<string, number> = {};
		for (const expense of $budget.expenses) {
			values[expense.category] = expense.amount;
		}
		inputValues = values;
	});

	let totalExpenses = $derived($budget.expenses.reduce((sum, e) => sum + e.amount, 0));
	let budgetLoaded = $derived(!!$budget.id);

	function getExpense(category: string) {
		return $budget.expenses.find((e) => e.category === category) ?? null;
	}

	async function handleBlur(category: string) {
		if (!$budget.id) return;
		const value = Number(inputValues[category]) || 0;
		const existing = getExpense(category);
		error = '';
		try {
			if (existing) {
				if (existing.amount !== value) await editExpense(existing.id, value, category, existing.description);
			} else if (value > 0) {
				await addExpense($budget.id, category, value, '');
			}
		} catch {
			error = 'Något gick fel vid sparning';
		}
	}

	async function handleDelete(category: string) {
		const existing = getExpense(category);
		if (!existing) return;
		error = '';
		try {
			await removeExpense(existing.id);
		} catch {
			error = 'Kunde inte ta bort utgiften';
		}
		deleteMode = false;
	}

	async function handleAdd() {
		if (!newCategory.trim() || newAmount <= 0) {
			error = 'Fyll i kategori och belopp';
			return;
		}
		error = '';
		try {
			await addExpense($budget.id, newCategory.trim(), newAmount, '');
			newCategory = '';
			newAmount = 0;
			addMode = false;
		} catch {
			error = 'Kunde inte lägga till utgiften';
		}
	}
</script>

<div class="expense-card">
	<h2>Utgifter</h2>

	{#if error}<p class="error">{error}</p>{/if}

	<ul class="list">
		{#if budgetLoaded}
			{#each $budget.expenses as expense}
				<li class="list-item">
					<span class="item-category">{expense.category}</span>
					<div class="item-right">
						{#if deleteMode}
							<button class="delete-confirm-btn" onclick={() => handleDelete(expense.category)}>
								Ta bort
							</button>
						{:else}
							<input
								type="number"
								class="amount-input"
								placeholder="0"
								min="0"
								bind:value={inputValues[expense.category]}
								onblur={() => handleBlur(expense.category)}
							/>
							<span class="kr-label">kr</span>
						{/if}
					</div>
				</li>
			{/each}
		{:else}
			{#each PLACEHOLDER_CATEGORIES as category}
				<li class="list-item">
					<span class="item-category">{category}</span>
					<div class="item-right">
						<input type="number" class="amount-input" placeholder="0" disabled />
						<span class="kr-label">kr</span>
					</div>
				</li>
			{/each}
		{/if}
	</ul>

	{#if addMode}
		<div class="add-form">
			<input
				type="text"
				class="add-input"
				placeholder="Kategori (t.ex. Gym)"
				bind:value={newCategory}
			/>
			<div class="add-row">
				<input
					type="number"
					class="add-input"
					placeholder="Belopp"
					min="0"
					bind:value={newAmount}
				/>
				<span class="kr-label">kr</span>
				<button class="save-btn" onclick={handleAdd}>Spara</button>
				<button class="cancel-btn" onclick={() => { addMode = false; error = ''; }}>Avbryt</button>
			</div>
		</div>
	{/if}

	{#if budgetLoaded && $budget.expenses.length > 0}
		<div class="summary">
			{#each $budget.expenses as expense}
				{#if expense.amount > 0}
					<div class="summary-row">
						<span>{expense.category}</span>
						<span class="summary-amount">-{expense.amount} kr</span>
					</div>
				{/if}
			{/each}
			<div class="summary-row total-row">
				<span>Totalt</span>
				<span class="red">{totalExpenses} kr</span>
			</div>
		</div>
	{/if}

	<div class="actions-row">
		<button
			class="action-btn"
			disabled={!budgetLoaded}
			onclick={() => { addMode = !addMode; deleteMode = false; }}
		>
			{addMode ? 'Avbryt' : '+ Lägg till utgift'}
		</button>
		<button
			class="action-btn danger"
			class:active={deleteMode}
			disabled={!budgetLoaded}
			onclick={() => { deleteMode = !deleteMode; addMode = false; }}
		>
			{deleteMode ? 'Avbryt' : 'Ta bort utgift'}
		</button>
	</div>
</div>

<style>
	.expense-card {
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

	.list {
		list-style: none;
		display: flex;
		flex-direction: column;
		gap: 0.4rem;
	}

	.list-item {
		display: flex;
		justify-content: space-between;
		align-items: center;
		padding: 0.6rem 0.75rem;
		background: var(--color-hover);
		border-radius: var(--radius-control);
	}

	.item-category {
		font-size: 0.9rem;
		font-weight: 500;
		color: var(--color-text);
	}

	.item-right {
		display: flex;
		align-items: center;
		gap: 0.4rem;
	}

	.amount-input {
		width: 90px;
		padding: 0.4rem 0.6rem;
		border: 1px solid var(--color-border);
		border-radius: var(--radius-control);
		font-size: 0.9rem;
		text-align: right;
		outline: none;
		transition: border 0.15s;
		background: var(--color-input);
		color: var(--color-text);
	}

	.amount-input:focus {
		border-color: var(--color-primary);
	}

	.amount-input:disabled {
		opacity: 0.4;
		cursor: not-allowed;
		background: var(--color-hover);
	}

	.kr-label {
		font-size: 0.85rem;
		color: var(--color-muted);
		min-width: 1.5rem;
	}

	.add-form {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		padding: 0.75rem;
		background: var(--color-hover);
		border-radius: var(--radius-control);
		border: 1px solid var(--color-border);
	}

	.add-row {
		display: flex;
		align-items: center;
		gap: 0.4rem;
	}

	.add-input {
		padding: 0.45rem 0.7rem;
		border: 1px solid var(--color-border);
		border-radius: var(--radius-control);
		font-size: 0.9rem;
		outline: none;
		transition: border 0.15s;
		background: var(--color-input);
		color: var(--color-text);
		width: 100%;
	}

	.add-input:focus {
		border-color: var(--color-primary);
	}

	.save-btn {
		padding: 0.45rem 0.85rem;
		background: var(--color-primary);
		color: white;
		border: none;
		border-radius: var(--radius-control);
		font-size: 0.85rem;
		font-weight: 600;
		cursor: pointer;
		white-space: nowrap;
		transition: background 0.15s;
	}

	.save-btn:hover {
		background: var(--color-primary-hover);
	}

	.cancel-btn {
		padding: 0.45rem 0.85rem;
		background: none;
		border: 1px solid var(--color-border);
		border-radius: var(--radius-control);
		font-size: 0.85rem;
		color: var(--color-muted);
		cursor: pointer;
		white-space: nowrap;
	}

	.cancel-btn:hover {
		background: var(--color-surface);
	}

	.summary {
		border-top: 1px solid var(--color-border);
		padding-top: 0.75rem;
		display: flex;
		flex-direction: column;
		gap: 0.35rem;
	}

	.summary-row {
		display: flex;
		justify-content: space-between;
		font-size: 0.88rem;
		color: var(--color-muted);
	}

	.summary-amount {
		font-weight: 500;
	}

	.total-row {
		font-weight: 600;
		font-size: 0.95rem;
		color: var(--color-text);
		border-top: 1px solid var(--color-border);
		padding-top: 0.5rem;
		margin-top: 0.15rem;
	}

	.red {
		color: var(--color-danger);
	}

	.actions-row {
		display: flex;
		gap: 0.5rem;
		justify-content: flex-end;
	}

	.action-btn {
		padding: 0.45rem 1rem;
		background: none;
		border: 1px solid var(--color-border);
		border-radius: var(--radius-control);
		font-size: 0.85rem;
		font-weight: 500;
		color: var(--color-muted);
		cursor: pointer;
		transition: background 0.15s, color 0.15s, border-color 0.15s;
	}

	.action-btn:disabled {
		opacity: 0.4;
		cursor: not-allowed;
	}

	.action-btn:hover:not(:disabled) {
		background: var(--color-hover);
		color: var(--color-primary);
		border-color: var(--color-primary);
	}

	.action-btn.danger:hover:not(:disabled),
	.action-btn.danger.active {
		color: var(--color-danger);
		border-color: var(--color-danger);
		background: var(--color-hover);
	}

	.delete-confirm-btn {
		padding: 0.35rem 0.75rem;
		background: none;
		border: 1px solid var(--color-danger);
		border-radius: var(--radius-control);
		font-size: 0.82rem;
		font-weight: 500;
		color: var(--color-danger);
		cursor: pointer;
		transition: background 0.15s;
	}

	.delete-confirm-btn:hover {
		background: #fef2f2;
	}

	.error {
		color: var(--color-danger);
		font-size: 0.85rem;
	}
</style>