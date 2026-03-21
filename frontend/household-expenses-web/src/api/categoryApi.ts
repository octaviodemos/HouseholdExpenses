import type {
  Category,
  CategoryTotalsResult,
  CreateCategoryRequest,
} from '../types/category'
import api from './axios'

export async function getCategories(): Promise<Category[]> {
  const { data } = await api.get<Category[]>('/api/categories')
  return data
}

export async function createCategory(
  body: CreateCategoryRequest,
): Promise<Category> {
  const { data } = await api.post<Category>('/api/categories', body)
  return data
}

export async function getCategoryTotals(): Promise<CategoryTotalsResult> {
  const { data } = await api.get<CategoryTotalsResult>('/api/categories/totals')
  return data
}
