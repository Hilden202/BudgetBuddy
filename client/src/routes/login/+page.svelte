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
			emailError = 'fel e-post eller lösenord';
		}
	}

	function goToRegister() {
		goto('/register');
	}
</script>

<div class="auth-page">
	<section class="auth-intro">
		<span class="brand">BudgetBuddy</span>
		<h1>Låt din ekonomi växa i lugn takt.</h1>
		<p>Få överblick över budget, utgifter och sparande i en tydlig och trygg vy.</p>
	</section>

	<section class="auth-card">
		<h2>Logga in</h2>

		{#if emailError}
			<p class="error">{emailError}</p>
		{/if}
		<input
			type="email"
			placeholder="E-post"
			bind:value={email}
			onblur={() => (emailTouched = true)}
		/>

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
	</section>
</div>
