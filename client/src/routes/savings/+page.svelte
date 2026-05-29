<script lang="ts">
	import SavingsCard from '../../components/SavingsCard.svelte';
</script>

<div class="page savings-page">
	<div class="savings-accent" aria-hidden="true"></div>
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
		grid-template-columns: minmax(320px, 520px) 1fr;
		gap: 2rem;
		align-items: start;
	}

	.savings-page {
		position: relative;
		min-height: calc(100vh - 4rem);
	}

	.page-header,
	.layout {
		position: relative;
		z-index: 1;
	}

	.savings-accent {
		position: absolute;
		right:-2rem;
		bottom: -2rem;
		width: 760px;
		height: 420px;
		background-image: url('/images/savings-accent-transparent.png');
		background-size: contain;
		background-repeat: no-repeat;
		background-position: bottom;
		opacity: 0.5;
		pointer-events: none;
		z-index: 0;
	}

	@media (max-width: 900px) {
		.layout {
			grid-template-columns: 1fr;
		}
	}
</style>
