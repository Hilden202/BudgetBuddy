<script lang="ts">
import { register } from '../../stores/authStore';
import { goto } from '$app/navigation';

let email = '';
let password = '';
let confirmPassword = '';
let error = '';

async function handleRegister() {
    console.log("Registrera klick");
    if (password !== confirmPassword) {
        error = 'lösenorden matchar inte';
        return;
    }

    try {
        await register(email, password);
        goto('/');
    } catch (e) {
        error = 'registrering misslyckades';
    }
}

function goToLogin() {
    goto('/login');
}

</script>

<div class="container">
    <h1>BudgetBuddy - Skapa konto</h1>

    {#if error}
        <p class="error">{error}</p>
    {/if}

    <input type="email" placeholder="Email" bind:value={email} />
    <input type="password" placeholder="Lösenord" bind:value={password} />
    <input type="password" placeholder="Bekräfta lösenord" bind:value={confirmPassword} />
    <button onclick={handleRegister}>Skapa konto</button>
    <button onclick={goToLogin}>Gå till inloggning</button>
</div>

<style>
    .container {
        max-width: 400px;
        margin: 100px auto;
        display: flex;
        flex-direction: column;
        gap: 12px;
    }

    input {
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 6px;
        font-size: 1rem;
    }

    button {
        padding: 10px;
        background: #4f46e5;
        color: white;
        border: none;
        border-radius: 6px;
        font-size: 1rem;
        cursor: pointer;
    }

    .error {
        color: red;
        font-size: 0.9rem;
    }
</style>