<script lang="ts">
import { updateSavingsGoal } from "../stores/api";
import { loadSavings, savings, saveSavingsGoal } from "../stores/savingStore";

let month = $state('');
let amount = $state(0);
let goalAmount = $state(0);
let error = $state('');
let loading = $state(false);
let success = $state('');

let progress = $derived($savings.monthAmount / $savings.savingsGoal * 100);
let remaining = $derived($savings.savingsGoal - $savings.monthAmount);

async function handleLoad() {
    loading = true;
    error = '';
    success = '';
    try {
        await loadSavings(month);
    } catch (e) {
        error = 'Kunde inte ladda sparmål';
        success = '';
    } finally {
        loading = false;
    }
}

async function handleSaveGoal() {
    if (!month || goalAmount <= 0) {
        error = 'Ange en giltig månad och ett positivt målbelopp';
        success = '';
        return;
    }
    loading = true;
    error = '';
    success = '';
    try {
        await saveSavingsGoal(month, goalAmount);
        success = 'Sparmål uppdaterat';
    } catch (e) {
        error = 'Kunde inte uppdatera sparmål';
        success = '';
    } finally {
        loading = false;
    }
}



</script>

<div class="savings-card">
    <h2>Sparande</h2>

    <div class="row">
        <label for="month">Månad</label>
        <input
            id="month"
            type="month"
            bind:value={month}
            onchange={handleLoad}
        />
    </div>

    {#if error}
        <p class="error">{error}</p>
    {/if}

    {#if success}
        <p class="success">{success}</p>
    {/if}

    <div class="summary">
        <div class="summary-row">
            <span>Sparat denna månad</span>
            <strong>{$savings.monthAmount} kr</strong>
        </div>

        <div class="summary-row">
            <span>Totalt sparat</span>
            <strong>{$savings.totalAmount} kr</strong>
        </div>

        <div class="summary-row">
            <span>Sparmål</span>
            <strong>{$savings.savingsGoal} kr</strong>
        </div>

        <div class="summary-row total">
            <span>Kvar till mål</span>
            <strong>{remaining} kr</strong>
        </div>
    </div>

    <div class="progress">
        <div class="progress-fill" style={`width: ${progress}%`}></div>
    </div>

    <div class="row">
        <label for="goal">Ändra sparmål</label>
        <input
            id="goal"
            type="number"
            min="0"
            bind:value={goalAmount}
            placeholder="0 kr"
        />
    </div>

    <button onclick={handleSaveGoal} disabled={loading || !month}>
        {loading ? 'Sparar...' : 'Spara sparmål'}
    </button>
</div>

<style>
    .savings-card {
        background: white;
        border-radius: 12px;
        padding: 1.5rem;
        border: 1px solid #e5e7eb;
        display: flex;
        flex-direction: column;
        gap: 1rem;
        max-width: 520px;
    }

    h2 {
        font-size: 1rem;
        font-weight: 600;
        color: #1f2937;
    }

    .row {
        display: flex;
        flex-direction: column;
        gap: 0.4rem;
    }

    label {
        font-size: 0.9rem;
        font-weight: 500;
        color: #374151;
    }

    input {
        padding: 0.65rem 0.9rem;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        font-size: 0.95rem;
        outline: none;
        transition: border 0.15s;
    }

    input:focus {
        border-color: #4f46e5;
    }

    .summary {
        border-top: 1px solid #f3f4f6;
        padding-top: 1rem;
        display: flex;
        flex-direction: column;
        gap: 0.65rem;
    }

    .summary-row {
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 0.9rem;
        color: #6b7280;
    }

    .summary-row strong {
        color: #1f2937;
        font-weight: 600;
    }

    .summary-row.total {
        font-size: 1rem;
        font-weight: 600;
        color: #1f2937;
        border-top: 1px solid #f3f4f6;
        padding-top: 0.75rem;
        margin-top: 0.25rem;
    }

    .progress {
        width: 100%;
        height: 10px;
        background: #e5e7eb;
        border-radius: 999px;
        overflow: hidden;
    }

    .progress-fill {
        height: 100%;
        background: #4f46e5;
        border-radius: 999px;
        transition: width 0.2s ease;
    }

    button {
        padding: 0.7rem;
        background: #4f46e5;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 0.95rem;
        font-weight: 600;
        cursor: pointer;
        transition: background 0.15s;
    }

    button:hover:not(:disabled) {
        background: #4338ca;
    }

    button:disabled {
        opacity: 0.6;
        cursor: not-allowed;
    }

    .error {
        color: #dc2626;
        font-size: 0.85rem;
    }

    .success {
        color: #16a34a;
        font-size: 0.85rem;
    }
</style>