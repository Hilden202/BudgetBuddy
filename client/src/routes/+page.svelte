<script lang="ts">
	import { goto } from '$app/navigation';
	import { onMount } from 'svelte';
	import { budget, loadBudget } from '../stores/budgetStore';
	import { savings, loadSavings } from '../stores/savingStore';
	import { getCurrentMonth } from '../lib/utils/date';

	let loading = $state(true);
	let error = $state('');
	let month = $state(getCurrentMonth());

	let totalExpenses = $derived($budget.expenses.reduce((sum, expense) => sum + expense.amount, 0));

	let latestExpenses = $derived($budget.expenses.slice(-5).reverse());

	let savingsRemaining = $derived($savings.savingsGoal - $savings.monthAmount);

	onMount(async () => {
		const savedToken = localStorage.getItem('token');
		if (!savedToken) {
			goto('/login');
			return;
		}

		try {
			await Promise.all([loadBudget(month), loadSavings(month)]);
			if (!$budget.id) {
				error = `Ingen budget hittades för måndaden.`;
			}
		} catch {
			error = 'Kunde inte ladda dashboarden';
		} finally {
			loading = false;
		}
	});
</script>

{#if loading}
	<p>Laddar...</p>
{:else}
	<div class="dashboard">
		<h1 class="page-title">Dashboard</h1>
		<p class="subtitle">Översikt för {month}</p>

		{#if error}
			<p class="error">{error}</p>
		{/if}
		<!-- Stat-kort -->
		<div class="cards">
			<div class="card">
				<span class="card-label">Tillgängligt belopp</span>
				<span class="card-value green">{$budget.remaining} kr</span>
				<span class="card-sub">av {$budget.income} kr</span>
			</div>
			<div class="card">
				<span class="card-label">Totala utgifter</span>
				<span class="card-value blue">{totalExpenses} kr</span>
				<span class="card-sub">av {$budget.income} kr</span>
			</div>
			<div class="card">
				<span class="card-label">Sparat denna månad</span>
				<span class="card-value purple">{$savings.monthAmount} kr</span>
				<span class="card-sub">av {$savings.savingsGoal} kr mål</span>
			</div>
			<div class="card">
				<span class="card-label">Återstår av sparmål</span>
				<span class="card-value orange">{savingsRemaining} kr</span>
				<span class="card-sub">av {$savings.savingsGoal} kr</span>
			</div>
		</div>

		<!-- Nedre sektion -->
		<div class="bottom">
			<div class="panel">
				<h2 class="panel-title">Senaste utgifter</h2>

				{#if latestExpenses.length === 0}
					<p class="empty">Inga utgifter ännu</p>
				{:else}
					<ul class="expense-list">
						{#each latestExpenses as expense (expense.id)}
							<li class="expense-row">
								<div>
									<strong>{expense.category}</strong>
									{#if expense.description}
										<span>{expense.description}</span>
									{/if}
								</div>

								<strong class="red">-{expense.amount} kr</strong>
							</li>
						{/each}
					</ul>
				{/if}
			</div>
			<div class="panel">
				<h2 class="panel-title">Sparmål</h2>

				{#if $savings.savingsGoal <= 0}
					<p class="empty">Inga sparmål ännu</p>
				{:else}
					<div class="summary-row">
						<span>Sparat denna månad</span>
						<strong>{$savings.monthAmount} kr</strong>
					</div>

					<div class="summary-row">
						<span>Sparmål</span>
						<strong>{$savings.savingsGoal} kr</strong>
					</div>

					<div class="summary-row total">
						<span>Återstår</span>
						<strong>{savingsRemaining} kr</strong>
					</div>
				{/if}
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

	.green {
		color: #16a34a;
	}
	.blue {
		color: #2563eb;
	}
	.purple {
		color: #7c3aed;
	}
	.orange {
		color: #ea580c;
	}

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

	.expense-list {
		list-style: none;
		padding: 0;
		margin: 0;
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
	}

	.expense-row {
		display: flex;
		justify-content: space-between;
		align-items: center;
		border-bottom: 1px solid #f3f4f6;
		padding-bottom: 0.75rem;
	}

	.expense-row div {
		display: flex;
		flex-direction: column;
		gap: 0.2rem;
	}

	.expense-row span {
		color: #9ca3af;
		font-size: 0.85rem;
	}

	.summary-row {
		display: flex;
		justify-content: space-between;
		color: #6b7280;
		font-size: 0.9rem;
		margin-bottom: 0.6rem;
	}

	.summary-row strong {
		color: #1f2937;
	}

	.summary-row.total {
		border-top: 1px solid #f3f4f6;
		padding-top: 0.75rem;
		font-weight: 700;
	}
	.error {
		color: #dc2626;
		font-size: 0.85rem;
	}
</style>
