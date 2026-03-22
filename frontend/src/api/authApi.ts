import type { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth'
import api from './axios'

export async function register(data: RegisterRequest): Promise<AuthResponse> {
  const { data: body } = await api.post<AuthResponse>('/api/auth/register', data)
  return body
}

export async function login(data: LoginRequest): Promise<AuthResponse> {
  const { data: body } = await api.post<AuthResponse>('/api/auth/login', data)
  return body
}
