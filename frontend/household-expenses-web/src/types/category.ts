import type { SummaryTotals } from './person'

/**
 * Finalidade da categoria (valores numéricos iguais ao backend C#).
 * Objeto const em vez de enum por compatibilidade com erasableSyntaxOnly no TS.
 */
export const CategoryPurpose = {
  Expense: 1,
  Income: 2,
  Both: 3,
} as const

export type CategoryPurpose = (typeof CategoryPurpose)[keyof typeof CategoryPurpose]

export interface Category {
  id: string
  description: string
  purpose: CategoryPurpose
}

export interface CreateCategoryRequest {
  description: string
  purpose: CategoryPurpose
}

export interface CategoryTotals {
  id: string
  description: string
  totalIncome: number
  totalExpense: number
  balance: number
}

export interface CategoryTotalsResult {
  categories: CategoryTotals[]
  summary: SummaryTotals
}

/** Rótulo em português para exibição em tabelas e selects. */
export function categoryPurposeLabel(purpose: CategoryPurpose): string {
  switch (purpose) {
    case CategoryPurpose.Expense:
      return 'Despesa'
    case CategoryPurpose.Income:
      return 'Receita'
    case CategoryPurpose.Both:
      return 'Ambas'
    default:
      return String(purpose)
  }
}
