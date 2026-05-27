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

	ChartJS.register(ArcElement, Tooltip, Legend, CategoryScale, LinearScale, BarElement);

	let month = $state('');
	let error = $state('');
	let loading = $state(false);

	let expenseItems = $derived($budget.expenses.filter((expense) => expense.amount > 0));

	let pieData = $derived({
		labels: expenseItems.map((expense) => expense.category),
		datasets: [
			{
				data: expenseItems.map((expense) => expense.amount),
				backgroundColor: ['#4f46e5', '#16a34a', '#dc2626', '#f59e0b', '#2563eb', '#9333ea'],
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
				backgroundColor: ['#16a34a', '#dc2626', '#4f46e5'],
				borderRadius: 8
			}
		]
	});

	const chartOptions = {
		responsive: true,
		maintainAspectRatio: false,
		plugins: {
			legend: {
				position: 'bottom'
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

<div class="page">
	<h1 class="page-title">Diagram</h1>

	<div class="toolbar">
		<input type="month" bind:value={month} onchange={handleLoad} />
	</div>

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
	.page-title {
		font-size: 1.8rem;
		font-weight: 700;
		margin-bottom: 2rem;
	}

	.toolbar {
		background: white;
		border-radius: 12px;
		padding: 1.5rem;
		border: 1px solid #e5e7eb;
		margin-bottom: 1.5rem;
		max-width: 420px;
	}

	input {
		width: 100%;
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

	.layout {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 1.5rem;
		align-items: start;
	}

	.chart-card,
	.empty-card {
		background: white;
		border-radius: 12px;
		padding: 1.5rem;
		border: 1px solid #e5e7eb;
	}

	h2 {
		font-size: 1rem;
		font-weight: 600;
		color: #1f2937;
		margin-bottom: 1rem;
	}

	.chart-box {
		height: 320px;
	}

	.empty {
		color: #9ca3af;
		font-size: 0.9rem;
	}

	.error {
		color: #dc2626;
		font-size: 0.85rem;
		margin-bottom: 1rem;
	}

	@media (max-width: 900px) {
		.layout {
			grid-template-columns: 1fr;
		}
	}
</style>
