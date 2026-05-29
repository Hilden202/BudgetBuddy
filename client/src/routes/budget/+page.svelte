<script lang="ts">
	import BudgetForm from '../../components/BudgetForm.svelte';
	import ExpenseList from '../../components/ExpenseList.svelte';
	import SavingsCard from '../../components/SavingsCard.svelte';
</script>

<div class="page">
	<header class="page-header">
		<h1 class="page-title">Budget</h1>
		<p class="page-subtitle">Hantera månadens inkomster, utgifter och sparande.</p>
	</header>

	<div class="layout">
		<!-- Vänsterkolumn: BudgetForm + SavingsCard staplade -->
		<div class="left-col">
			<BudgetForm />
			<SavingsCard />
		</div>

		<!-- Högerkolumn: Utgifter -->
		<ExpenseList />
	</div>
</div>

<style>
	.layout {
		display: grid;
		grid-template-columns: 1fr 2fr;
		gap: 1.5rem;
		align-items: start;
	}

	.left-col {
		display: flex;
		flex-direction: column;
		gap: 1.5rem;
	}
</style>