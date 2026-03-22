import type {
  CreateTransactionRequest,
  Transaction,
} from '../types/transaction'
import api from './axios'

export async function getTransactions(): Promise<Transaction[]> {
  const { data } = await api.get<Transaction[]>('/api/transactions')
  return data
}

export async function createTransaction(
  body: CreateTransactionRequest,
): Promise<Transaction> {
  const { data } = await api.post<Transaction>('/api/transactions', body)
  return data
}
