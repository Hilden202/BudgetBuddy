export function validateEmail(email:string): string | null {
    if (!email) return 'Email är obligatorisk';
    
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) return "Ange en giltigt email";

    return null;
}

export function validatePassword(password: string): string | null {
    if(!password) return 'Lösenord är obligatoriskt';
    if(password.length < 6) return 'Lösenordet måste vara minst 6 tecken';
    if(!/[A-Z]/.test(password)) return 'Lösenordet måste innehålla minst en stor bokstav';
    if (!/[0-9]/.test(password)) return 'Lösenordet måste innehålla minst en siffra';
    return null;
 }

export function validateConfirmedPassword(password:string, confirmPassowrd: string): string | null {
    if(password !== confirmPassowrd) return 'Lösenorden matchar inte';
    return null;
}
