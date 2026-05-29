<script lang="ts">
	import { register } from '../../stores/authStore';
	import { goto } from '$app/navigation';
	import {
		validateEmail,
		validateConfirmedPassword,
		validatePassword
	} from '$lib/utils/validation';

	let email = $state('');
	let password = $state('');
	let confirmPassword = $state('');
	let emailTouched = $state(false);
	let passwordTouched = $state(false);
	let confirmTouched = $state(false);

	let emailError = $derived(emailTouched ? validateEmail(email) : null);
	let passwordError = $derived(passwordTouched ? validatePassword(password) : null);
	let confirmError = $derived(
		confirmTouched ? validateConfirmedPassword(password, confirmPassword) : null
	);

	async function handleRegister() {
		if (emailError || passwordError || confirmError) return;

		try {
			await register(email, password);
			goto('/');
		} catch (e) {
			emailError = 'registrering misslyckades';
		}
	}

	function goToLogin() {
		goto('/login');
	}
</script>

<div class="auth-page">
	<section class="auth-intro">
		<h1>Bygg en tryggare ekonomi från början.</h1>
		<p>Skapa ett konto och följ budget, utgifter och sparande med en tydlig överblick.</p>
	</section>

	<section class="auth-card">
		<h2>Skapa konto</h2>

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

		{#if confirmError}
			<p class="error">{confirmError}</p>
		{/if}
		<input
			type="password"
			placeholder="Bekräfta lösenord"
			bind:value={confirmPassword}
			onblur={() => (confirmTouched = true)}
		/>

		<button onclick={handleRegister}>Registrera</button>
		<button class="secondary-btn" onclick={goToLogin}>Gå till inloggning</button>
	</section>
</div>
