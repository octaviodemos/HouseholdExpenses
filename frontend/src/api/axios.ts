import axios, { type AxiosError } from 'axios'
import { AUTH_TOKEN_STORAGE_KEY } from '../types/auth'

/** Corpo JSON padrão da API em caso de falha (validação, não encontrado, etc.). */
interface ApiErrorPayload {
  errors?: string[]
}

const baseURL =
  import.meta.env.VITE_API_URL?.toString().trim() || 'http://localhost:5049'

/**
 * Cliente HTTP centralizado: baseURL configurável e tratamento uniforme de erros
 * retornados pela API (`errors` como lista de mensagens).
 */
const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  const stored = localStorage.getItem(AUTH_TOKEN_STORAGE_KEY)
  if (stored) {
    config.headers.Authorization = `Bearer ${stored}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiErrorPayload>) => {
    const payload = error.response?.data
    if (payload?.errors && Array.isArray(payload.errors) && payload.errors.length > 0) {
      const message = payload.errors.join('; ')
      return Promise.reject(new Error(message))
    }
    if (error.message) {
      return Promise.reject(error)
    }
    return Promise.reject(new Error('Falha na comunicação com o servidor.'))
  },
)

export default api
