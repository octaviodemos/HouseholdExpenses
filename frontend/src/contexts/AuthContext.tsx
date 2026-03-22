import { useCallback, useMemo, useState, type ReactNode } from 'react'
import * as authApi from '../api/authApi'
import {
  AUTH_EMAIL_STORAGE_KEY,
  AUTH_TOKEN_STORAGE_KEY,
  type AuthResponse,
} from '../types/auth'
import { AuthContext, type AuthContextValue } from './auth-context'

function persistSession(data: AuthResponse): void {
  localStorage.setItem(AUTH_TOKEN_STORAGE_KEY, data.token)
  localStorage.setItem(AUTH_EMAIL_STORAGE_KEY, data.email)
}

function clearSessionStorage(): void {
  localStorage.removeItem(AUTH_TOKEN_STORAGE_KEY)
  localStorage.removeItem(AUTH_EMAIL_STORAGE_KEY)
}

/**
 * Fornece estado de sessão JWT, persistência no localStorage e métodos login/register/logout.
 */
export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() =>
    localStorage.getItem(AUTH_TOKEN_STORAGE_KEY),
  )
  const [email, setEmail] = useState<string | null>(() =>
    localStorage.getItem(AUTH_EMAIL_STORAGE_KEY),
  )

  const login = useCallback(async (loginEmail: string, password: string) => {
    const data = await authApi.login({ email: loginEmail.trim(), password })
    persistSession(data)
    setToken(data.token)
    setEmail(data.email)
  }, [])

  const register = useCallback(async (registerEmail: string, password: string) => {
    const data = await authApi.register({ email: registerEmail.trim(), password })
    persistSession(data)
    setToken(data.token)
    setEmail(data.email)
  }, [])

  const logout = useCallback(() => {
    clearSessionStorage()
    setToken(null)
    setEmail(null)
  }, [])

  const value = useMemo<AuthContextValue>(
    () => ({
      token,
      email,
      isAuthenticated: Boolean(token),
      login,
      register,
      logout,
    }),
    [token, email, login, register, logout],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
