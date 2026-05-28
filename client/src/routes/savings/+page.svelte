<script lang="ts">
	import SavingsCard from '../../components/SavingsCard.svelte';
</script>

<div class="page">
	<header class="page-header">
		<h1 class="page-title">Sparande</h1>
		<p class="page-subtitle">Följ ditt sparmål och se hur mycket som återstår.</p>
	</header>

	<div class="layout">
		<SavingsCard />
	</div>
</div>

<style>
	.layout {
		display: grid;
		grid-template-columns: 1fr 2fr;
		gap: 1.5rem;
		align-items: start;
	}
</style>
