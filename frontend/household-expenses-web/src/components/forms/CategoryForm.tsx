import { useState, type FormEvent } from 'react'
import { CategoryPurpose, type CreateCategoryRequest } from '../../types/category'

type CategoryFormProps = {
  onSubmit: (data: CreateCategoryRequest) => void | Promise<void>
  onCancel: () => void
  isSubmitting?: boolean
}

/**
 * Cadastro de categoria: descrição + finalidade (enum numérico enviado à API).
 */
export default function CategoryForm({
  onSubmit,
  onCancel,
  isSubmitting = false,
}: CategoryFormProps) {
  const [description, setDescription] = useState('')
  const [purpose, setPurpose] = useState<CategoryPurpose>(CategoryPurpose.Expense)
  const [errors, setErrors] = useState<{ description?: string }>({})

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    const trimmed = description.trim()
    const next: typeof errors = {}
    if (!trimmed) next.description = 'Descrição é obrigatória.'
    else if (trimmed.length > 400) next.description = 'Máximo de 400 caracteres.'
    setErrors(next)
    if (Object.keys(next).length > 0) return
    await onSubmit({ description: trimmed, purpose })
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label
          htmlFor="category-desc"
          className="mb-1 block text-sm font-medium text-slate-700"
        >
          Descrição
        </label>
        <input
          id="category-desc"
          type="text"
          maxLength={400}
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        />
        {errors.description && (
          <p className="mt-1 text-xs text-red-600">{errors.description}</p>
        )}
      </div>
      <div>
        <label
          htmlFor="category-purpose"
          className="mb-1 block text-sm font-medium text-slate-700"
        >
          Finalidade
        </label>
        <select
          id="category-purpose"
          value={purpose}
          onChange={(e) => setPurpose(Number(e.target.value) as CategoryPurpose)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        >
          <option value={CategoryPurpose.Expense}>Despesa</option>
          <option value={CategoryPurpose.Income}>Receita</option>
          <option value={CategoryPurpose.Both}>Ambas</option>
        </select>
      </div>
      <div className="flex justify-end gap-2 pt-2">
        <button
          type="button"
          onClick={onCancel}
          disabled={isSubmitting}
          className="rounded-lg border border-slate-200 px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50 disabled:opacity-50"
        >
          Cancelar
        </button>
        <button
          type="submit"
          disabled={isSubmitting}
          className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700 disabled:opacity-50"
        >
          {isSubmitting ? 'Salvando…' : 'Salvar'}
        </button>
      </div>
    </form>
  )
}
