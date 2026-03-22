/**
 * Contratos da API de autenticação e chaves do localStorage (compartilhadas com o axios).
 */

/** Chave usada para persistir o JWT. */
export const AUTH_TOKEN_STORAGE_KEY = 'household_expenses_auth_token'

/** Chave usada para persistir o e-mail exibido na UI. */
export const AUTH_EMAIL_STORAGE_KEY = 'household_expenses_auth_email'

export interface AuthResponse {
  token: string
  email: string
}

export interface RegisterRequest {
  email: string
  password: string
}

export interface LoginRequest {
  email: string
  password: string
}
