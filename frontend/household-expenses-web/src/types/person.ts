/**
 * Modelos alinhados à API (camelCase) para pessoas e agregados de totais.
 */

export interface Person {
  id: string
  name: string
  age: number
}

export interface PersonTotals {
  id: string
  name: string
  totalIncome: number
  totalExpense: number
  balance: number
}

export interface SummaryTotals {
  totalIncome: number
  totalExpense: number
  balance: number
}

export interface PersonTotalsResult {
  persons: PersonTotals[]
  summary: SummaryTotals
}

export interface CreatePersonRequest {
  name: string
  age: number
}

export interface UpdatePersonRequest {
  name: string
  age: number
}
