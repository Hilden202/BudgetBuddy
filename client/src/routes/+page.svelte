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
		<div class="dashboard-accent" aria-hidden="true"></div>

		<header class="page-header">
			<h1 class="page-title">Dashboard</h1>
			<p class="page-subtitle">Översikt för {month}</p>
		</header>

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
		position: relative;
		min-height: calc(100vh - 4rem);
	}

	.dashboard-accent {
		position: absolute;
		right: -25rem;
		bottom: -1rem;
		width: 420px;
		height: 420px;
		background-image: url('/images/dashboard-accent-transparent.png');
		background-size: contain;
		background-repeat: no-repeat;
		background-position: bottom right;
		opacity: 0.28;
		pointer-events: none;
		z-index: 0;
	}

	.page-header,
	.cards,
	.bottom {
		position: relative;
		z-index: 1;
	}

	/* ── Cards ── */
	.cards {
		display: grid;
		grid-template-columns: repeat(4, 1fr);
		gap: 1rem;
		margin-bottom: 2rem;
	}

	.card {
		background: var(--color-surface);
		border-radius: var(--radius-card);
		padding: 1.25rem 1.5rem;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		border: 1px solid var(--color-border);
	}

	.card-label {
		font-size: 0.8rem;
		color: var(--color-muted);
		font-weight: 500;
	}

	.card-value {
		font-size: 1.6rem;
		font-weight: 700;
	}

	.card-sub {
		font-size: 0.8rem;
		color: var(--color-muted);
	}

	.green {
		color: var(--color-green);
	}
	.blue {
		color: var(--color-blue);
	}
	.purple {
		color: var(--color-purple);
	}
	.orange {
		color: var(--color-orange);
	}

	/* ── Bottom panels ── */
	.bottom {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 1rem;
	}

	.panel {
		background: var(--color-surface);
		border-radius: var(--radius-card);
		padding: 1.5rem;
		border: 1px solid var(--color-border);
	}

	.panel-title {
		font-size: 1rem;
		font-weight: 600;
		margin-bottom: 1rem;
		color: var(--color-text);
	}

	.empty {
		color: var(--color-muted);
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
		border-bottom: 1px solid var(--color-border);
		padding-bottom: 0.75rem;
	}

	.expense-row div {
		display: flex;
		flex-direction: column;
		gap: 0.2rem;
	}

	.expense-row span {
		color: var(--color-muted);
		font-size: 0.85rem;
	}

	.summary-row {
		display: flex;
		justify-content: space-between;
		color: var(--color-muted);
		font-size: 0.9rem;
		margin-bottom: 0.6rem;
	}

	.summary-row strong {
		color: var(--color-text);
	}

	.summary-row.total {
		border-top: 1px solid var(--color-border);
		padding-top: 0.75rem;
		font-weight: 700;
	}
	.error {
		color: var(--color-danger);
		font-size: 0.85rem;
	}
</style>
