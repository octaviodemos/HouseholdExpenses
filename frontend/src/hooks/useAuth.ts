import { useContext } from 'react'
import { AuthContext, type AuthContextValue } from '../contexts/auth-context'

/** Acesso ao estado de autenticação (obrigatório dentro de AuthProvider). */
export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext)
  if (!ctx) {
    throw new Error('useAuth deve ser usado dentro de AuthProvider')
  }
  return ctx
}
