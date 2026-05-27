<script lang="ts">
	import SavingsCard from '../../components/SavingsCard.svelte';
</script>

<h1>Sparande</h1>
<div class="page">
	<h2 class="page-title">Budget</h2>

	<div class="layout">
		<SavingsCard />
	</div>
</div>

<style>
	.page-title {
		font-size: 1.8rem;
		font-weight: 700;
		margin-bottom: 2rem;
	}

	.layout {
		display: grid;
		grid-template-columns: 1fr 2fr;
		gap: 1.5rem;
		align-items: start;
	}
</style>
