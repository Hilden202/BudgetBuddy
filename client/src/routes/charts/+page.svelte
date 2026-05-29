<script lang="ts">
	import { Pie, Bar } from 'svelte-chartjs';
	import {
		Chart as ChartJS,
		ArcElement,
		Tooltip,
		Legend,
		CategoryScale,
		LinearScale,
		BarElement
	} from 'chart.js';

	import { budget, loadBudget } from '../../stores/budgetStore';
	import { getCurrentMonth } from '../../lib/utils/date';

	ChartJS.register(ArcElement, Tooltip, Legend, CategoryScale, LinearScale, BarElement);

	let month = $state(getCurrentMonth());
	let error = $state('');
	let loading = $state(false);

	let expenseItems = $derived($budget.expenses.filter((expense) => expense.amount > 0));

	const chartColors = ['#1f6f64', '#3fa58c', '#6f91a0', '#d7b98e', '#c98a4a', '#9a82b3'];

	const chartHoverColors = ['#18574f', '#338b76', '#5b7c89', '#c4a274', '#ad733d', '#846c9e'];

	let pieData = $derived({
		labels: expenseItems.map((expense) => expense.category),
		datasets: [
			{
				data: expenseItems.map((expense) => expense.amount),
				backgroundColor: chartColors,
				hoverBackgroundColor: chartHoverColors,
				hoverOffset: 6,
				borderWidth: 0
			}
		]
	});

	let barData = $derived({
		labels: ['Inkomst', 'Utgifter', 'Kvar'],
		datasets: [
			{
				label: 'Belopp',
				data: [$budget.income, $budget.income - $budget.remaining, $budget.remaining],
				backgroundColor: ['#3fa58c', '#c98a4a', '#1f6f64'],
				hoverBackgroundColor: ['#338b76', '#ad733d', '#18574f'],
				borderRadius: 8
			}
		]
	});

	const chartOptions = {
		responsive: true,
		maintainAspectRatio: false,
		plugins: {
			legend: {
				position: 'bottom' as const,
				labels: {
					boxWidth: 18,
					boxHeight: 18,
					padding: 18,
					font: {
						size: 16
					}
				}
			}
		}
	};

	async function handleLoad() {
		if (!month) return;

		loading = true;
		error = '';

		try {
			await loadBudget(month);

			if (!$budget.id) {
				error = 'Ingen budget hittades för vald månad';
			}
		} finally {
			loading = false;
		}
	}
</script>

<div class="page charts-page">
	<div class="charts-accent" aria-hidden="true"></div>
	<header class="page-header charts-header">
		<div>
			<h1 class="page-title">Diagram</h1>
			<p class="page-subtitle">Visuell översikt över utgifter och budgetläge.</p>
		</div>

		<div class="toolbar">
			<label for="chart-month">Välj månad:</label>
			<input type="month" id="chart-month" bind:value={month} onchange={handleLoad} />
		</div>
	</header>

	{#if error}
		<p class="error">{error}</p>
	{/if}

	{#if loading}
		<p>Laddar...</p>
	{:else if !month}
		<div class="empty-card">
			<p>Välj en månad för att visa diagram.</p>
		</div>
	{:else if !$budget.id}
		<div class="empty-card">
			<p>Ingen budget hittades för vald månad.</p>
		</div>
	{:else}
		<div class="summary-cards">
			<div class="summary-card">
				<span>Inkomst</span>
				<strong class="green">{$budget.income} kr</strong>
			</div>

			<div class="summary-card">
				<span>Utgifter</span>
				<strong class="orange">{$budget.income - $budget.remaining} kr</strong>
			</div>

			<div class="summary-card">
				<span>Kvar</span>
				<strong>{$budget.remaining} kr</strong>
			</div>
		</div>
		<div class="layout">
			<section class="chart-card">
				<h2>Utgifter per kategori</h2>

				{#if expenseItems.length === 0}
					<p class="empty">Inga utgifter att visa.</p>
				{:else}
					<div class="chart-box">
						<Pie data={pieData} options={chartOptions} />
					</div>
				{/if}
			</section>

			<section class="chart-card">
				<h2>Budgetöversikt</h2>

				<div class="chart-box">
					<Bar data={barData} options={chartOptions} />
				</div>
			</section>
		</div>
	{/if}
</div>

<style>
	.charts-header {
		display: flex;
		justify-content: space-between;
		align-items: flex-end;
		gap: 1.5rem;
	}

	.charts-page {
		position: relative;
		min-height: calc(100vh - 4rem);
	}

	.charts-accent {
		position: absolute;
		left: 0;
		right: -2rem;
		bottom: -1rem;
		height: 320px;
		background-image: url('/images/charts-accent-transparent.png');
		background-size: 1000px auto;
		background-repeat: no-repeat;
		background-position: bottom center;
		opacity: 0.18;
		pointer-events: none;
		z-index: 0;
	}

	.toolbar {
		min-width: 220px;
	}

	.toolbar label {
		display: block;
		margin-bottom: 0.4rem;
		color: var(--color-muted);
		font-size: 0.9rem;
	}

	input {
		width: 100%;
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

	.layout {
		display: grid;
		grid-template-columns: repeat(2, minmax(0, 1fr));
		gap: 1.5rem;
		align-items: start;
	}

	.chart-card,
	.empty-card {
		background: var(--color-surface);
		border-radius: var(--radius-card);
		padding: 1.5rem;
		border: 1px solid var(--color-border);
		min-width: 0;
	}

	h2 {
		font-size: 1rem;
		font-weight: 600;
		color: var(--color-text);
		margin-bottom: 1rem;
	}

	.chart-box {
		height: 320px;
		min-width: 0;
	}

	.empty {
		color: var(--color-muted);
		font-size: 0.9rem;
	}

	.error {
		color: var(--color-danger);
		font-size: 0.85rem;
		margin-bottom: 1rem;
	}

	/* Summary cards */
	.summary-cards {
		display: grid;
		grid-template-columns: repeat(3, minmax(0, 1fr));
		gap: 1rem;
		margin-bottom: 1.5rem;
	}

	.summary-card {
		background: var(--color-surface);
		border: 1px solid var(--color-border);
		border-radius: var(--radius-card);
		padding: 1rem 1.25rem;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.summary-card strong {
		color: var(--color-primary);
		font-size: 1.4rem;
		font-weight: 700;
	}

	.summary-card .green {
		color: var(--color-success);
	}

	.summary-card .orange {
		color: var(--color-warning);
	}

	.charts-header,
	.summary-cards,
	.layout,
	.error,
	.empty-card {
		position: relative;
		z-index: 1;
	}

	@media (max-width: 1200px) {
		.layout {
			grid-template-columns: 1fr;
		}

		.summary-cards {
			grid-template-columns: 1fr;
		}
	}
</style>
