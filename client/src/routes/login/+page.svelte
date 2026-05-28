<script lang="ts">
	import { login } from '../../stores/authStore';
	import { goto } from '$app/navigation';
	import { validateEmail, validatePassword } from '$lib/utils/validation';

	let email = $state('');
	let password = $state('');
	let emailTouched = $state(false);
	let passwordTouched = $state(false);

	let emailError = $derived(emailTouched ? validateEmail(email) : null);
	let passwordError = $derived(passwordTouched ? validatePassword(password) : null);

	async function handleLogin() {
		if (emailError || passwordError) return;

		try {
			await login(email, password);
			goto('/');
		} catch (e) {
			emailTouched = true;
			emailError = 'fel email eller lösenord';
		}
	}

	function goToRegister() {
		goto('/register');
	}
</script>

<div class="container">
	<h1>Logga in</h1>

	{#if emailError}
		<p class="error">{emailError}</p>
	{/if}
	<input type="email" placeholder="Email" bind:value={email} onblur={() => (emailTouched = true)} />

	{#if passwordError}
		<p class="error">{passwordError}</p>
	{/if}
	<input
		type="password"
		placeholder="Lösenord"
		bind:value={password}
		onblur={() => (passwordTouched = true)}
	/>

	<button onclick={handleLogin}>Logga in</button>
	<button class="secondary-btn" onclick={goToRegister}>Gå till registrering</button>
</div>

<style>
	.container {
		max-width: 400px;
		margin: 100px auto;
		display: flex;
		flex-direction: column;
		gap: 12px;
		background: var(--color-surface);
		border: 1px solid var(--color-border);
		border-radius: var(--radius-card);
		padding: 2rem;
	}

	h1 {
		color: var(--color-text);
		font-size: 1.8rem;
		margin-bottom: 0.5rem;
	}

	input {
		padding: 0.65rem 0.9rem;
		border: 1px solid var(--color-border);
		border-radius: var(--radius-control);
		font-size: 1rem;
		background: var(--color-input);
		color: var(--color-text);
		outline: none;
		transition: border 0.15s;
	}

	input:focus {
		border-color: var(--color-primary);
	}

	button {
		padding: 0.7rem;
		background: var(--color-primary);
		color: white;
		border: none;
		border-radius: var(--radius-control);
		font-size: 1rem;
		cursor: pointer;
		transition: background 0.15s;
	}

	button:hover {
		background: var(--color-primary-hover);
	}

	.error {
		color: var(--color-danger);
		font-size: 0.9rem;
	}

	.secondary-btn {
		background: var(--color-hover);
		color: var(--color-text);
	}

	.secondary-btn:hover {
		background: var(--color-active);
	}
</style>
