<script lang="ts">
	import favicon from '$lib/assets/favicon.svg';
	import { page } from '$app/stores';
	import { token } from '../stores/authStore';
	import { goto } from '$app/navigation';
	import { browser } from '$app/environment';
	import '../app.css';

	let { children } = $props();

	const navItems = [
		{ href: '/', label: 'Dashboard', icon: '▦' },
		{ href: '/budget', label: 'Budget', icon: '◈' },
		{ href: '/savings', label: 'Sparande', icon: '◇' },
		{ href: '/charts', label: 'Diagram', icon: '◫' }
	];

	$effect(() => {
		if (browser) {
			const isAuthPage = $page.url.pathname === '/login' || $page.url.pathname === '/register';
			if (!$token && !isAuthPage) goto('/login');
		}
	});

	function logout() {
		localStorage.removeItem('token');
		token.set(null);
		goto('/login');
	}

	let isAuthPage = $derived($page.url.pathname === '/login' || $page.url.pathname === '/register');
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

{#if isAuthPage}
	{@render children()}
{:else}
	<div class="app">
		<aside class="sidebar">
			<div class="logo">
				<span class="logo-icon">◈</span>
				<span class="logo-text">BudgetBuddy</span>
			</div>

			<nav>
				{#each navItems as item}
					<a href={item.href} class="nav-item" class:active={$page.url.pathname === item.href}>
						<span class="nav-icon">{item.icon}</span>
						<span>{item.label}</span>
					</a>
				{/each}
			</nav>

			<button class="logout-btn" onclick={logout}>
				<span>⇦</span>
				<span>Logga ut</span>
			</button>
		</aside>

		<main class="main">
			{@render children()}
		</main>
	</div>
{/if}

<style>
	:global(*, *::before, *::after) {
		box-sizing: border-box;
		margin: 0;
		padding: 0;
	}

	:global(body) {
		font-family: 'Inter', Arial, sans-serif;
		background: var(--color-bg);
		color: var(--color-text);
	}

	.app {
		display: flex;
		min-height: 100vh;
	}

	/* ── Sidebar ── */
	.sidebar {
		width: 220px;
		background: var(--color-surface);
		border-right: 1px solid var(--color-border);
		display: flex;
		flex-direction: column;
		padding: 1.5rem 1rem;
		position: fixed;
		top: 0;
		left: 0;
		height: 100vh;
	}

	.logo {
		display: flex;
		align-items: center;
		gap: 0.6rem;
		padding: 0.5rem 0.75rem;
		margin-bottom: 2rem;
	}

	.logo-icon {
		font-size: 1.4rem;
		color: var(--color-primary);
	}

	.logo-text {
		font-size: 1.1rem;
		font-weight: 700;
		color: var(--color-text);
	}

	nav {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
		flex: 1;
	}

	.nav-item {
		display: flex;
		align-items: center;
		gap: 0.75rem;
		padding: 0.65rem 0.75rem;
		border-radius: 8px;
		text-decoration: none;
		color: var(--color-muted);
		font-size: 0.9rem;
		font-weight: 500;
		transition:
			background 0.15s,
			color 0.15s;
	}

	.nav-item:hover {
		background: var(--color-hover);
		color: var(--color-text);
	}

	.nav-item.active {
		background: var(--color-active);
		color: var(--color-primary);
		font-weight: 600;
	}

	.nav-icon {
		font-size: 1rem;
		width: 20px;
		text-align: center;
	}

	.logout-btn {
		display: flex;
		align-items: center;
		gap: 0.75rem;
		padding: 0.65rem 0.75rem;
		border-radius: 8px;
		border: none;
		background: none;
		color: var(--color-muted);
		font-size: 0.9rem;
		font-weight: 500;
		cursor: pointer;
		width: 100%;
		transition:
			background 0.15s,
			color 0.15s;
	}

	.logout-btn:hover {
		background: var(--color-surface);
		color: var(--color-danger);
	}

	/* ── Main content ── */
	.main {
		margin-left: 220px;
		flex: 1;
		padding: 2rem 2.5rem;
	}
</style>
