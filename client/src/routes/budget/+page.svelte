<script lang="ts">
	import BudgetForm from '../../components/BudgetForm.svelte';
	import ExpenseList from '../../components/ExpenseList.svelte';
	import SavingsCard from '../../components/SavingsCard.svelte';
</script>

<div class="page budget-page">
    <div class="budget-accent" aria-hidden="true"></div>
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

	.budget-page {
    position: relative;
    min-height: calc(100vh - 4rem);
}

.page-header,
.layout {
    position: relative;
    z-index: 1;
}

.budget-accent {
    position: absolute;
    top: 0;
    right: 1rem;
    width: 700px;
    height: 130px;
    background-image: url('/images/budget-accent-transparent.png');
    background-size: 700px auto;
    background-repeat: no-repeat;
    background-position: bottom right;
    opacity: 0.65;
    pointer-events: none;
    z-index: 0;
}
</style>
