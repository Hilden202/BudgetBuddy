<script lang="ts">
import { register } from '../../stores/authStore';
import { goto } from '$app/navigation';
import { validateEmail, validateConfirmedPassword, validatePassword } from '$lib/utils/validation';

let email = $state('');
let password = $state('');
let confirmPassword = $state('');
let emailTouched = $state(false);
let passwordTouched = $state(false);
let confirmTouched = $state(false);

let emailError = $derived(emailTouched ? validateEmail(email): null);
let passwordError = $derived(passwordTouched ? validatePassword(password): null);
let confirmError = $derived(confirmTouched ? validateConfirmedPassword(password, confirmPassword): null)


async function handleRegister() {

    if(emailError || passwordError  || confirmError) return;

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

<div class="container">
    <h1>BudgetBuddy - Skapa konto</h1>
    {#if emailError}
        <p class="error">{emailError}</p>
    {/if}
    <input type="email" placeholder="Email" bind:value={email} onblur={() => emailTouched = true} />
    {#if passwordError}
        <p class="error">{passwordError}</p>
    {/if}
    <input type="password" placeholder="Lösenord" bind:value={password} onblur={() => passwordTouched = true} />
    {#if confirmError}
        <p class="error">{confirmError}</p>
    {/if}
    <input type="password" placeholder="Bekräfta lösenord" bind:value={confirmPassword} onblur={() => confirmTouched = true} />
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