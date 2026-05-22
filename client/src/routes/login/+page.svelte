<script lang="ts">
import { login} from '../../stores/authStore';
import { goto } from '$app/navigation';
import { validateEmail, validatePassword } from '$lib/utils/validation';

let email = $state('');
let password = $state('');
let emailTouched = $state(false);
let passwordTouched = $state(false);

let emailError = $derived(emailTouched ? validateEmail(email) : null);
let passwordError = $derived(passwordTouched ? validatePassword(password) : null);


async function handleLogin() {
    if(emailError || passwordError) return;

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
    <h1>BudgetBuddy</h1>

    {#if emailError}
        <p class="error">{emailError}</p>
    {/if}
    <input type="email" placeholder="Email" bind:value={email} onblur={() => emailTouched = true} />
    
    {#if passwordError}
        <p class="error">{passwordError}</p>
    {/if}
    <input type="password" placeholder="Lösenord" bind:value={password} onblur={() => passwordTouched = true} />

    <button onclick={handleLogin}>Logga in</button>
    <button onclick={goToRegister}>Gå till registrering</button>
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