import { useMemo, useState, type FormEvent } from 'react'
import type { Category } from '../../types/category'
import { CategoryPurpose } from '../../types/category'
import type { CreateTransactionRequest } from '../../types/transaction'
import { TransactionType } from '../../types/transaction'

type PersonOption = { id: string; name: string }

type TransactionFormProps = {
  categories: Category[]
  persons: PersonOption[]
  onSubmit: (data: CreateTransactionRequest) => void | Promise<void>
  onCancel: () => void
  isSubmitting?: boolean
}

/**
 * Retorna categorias compatíveis com o tipo de transação selecionado
 * (regra igual à do domínio: Expense/Income ou Both).
 */
function categoriesForType(categories: Category[], type: TransactionType): Category[] {
  return categories.filter((c) => {
    if (c.purpose === CategoryPurpose.Both) return true
    if (type === TransactionType.Expense) return c.purpose === CategoryPurpose.Expense
    return c.purpose === CategoryPurpose.Income
  })
}

/**
 * Nova transação: selects de pessoa e categoria; categorias filtradas pelo tipo.
 */
export default function TransactionForm({
  categories,
  persons,
  onSubmit,
  onCancel,
  isSubmitting = false,
}: TransactionFormProps) {
  const [description, setDescription] = useState('')
  const [amount, setAmount] = useState('')
  const [type, setType] = useState<TransactionType>(TransactionType.Expense)
  const [categoryId, setCategoryId] = useState('')
  const [personId, setPersonId] = useState('')
  const [errors, setErrors] = useState<{
    description?: string
    amount?: string
    categoryId?: string
    personId?: string
  }>({})

  const filteredCategories = useMemo(
    () => categoriesForType(categories, type),
    [categories, type],
  )

  function handleTypeChange(newType: TransactionType) {
    setType(newType)
    const allowed = categoriesForType(categories, newType)
    if (categoryId && !allowed.some((c) => c.id === categoryId)) {
      setCategoryId('')
    }
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    const next: typeof errors = {}
    const trimmed = description.trim()
    if (!trimmed) next.description = 'Descrição é obrigatória.'
    else if (trimmed.length > 400) next.description = 'Máximo de 400 caracteres.'
    const amountNum = Number(amount.replace(',', '.'))
    if (amount === '' || Number.isNaN(amountNum)) next.amount = 'Valor é obrigatório.'
    else if (amountNum < 0.01) next.amount = 'Valor mínimo é 0,01.'
    if (!categoryId) next.categoryId = 'Selecione uma categoria.'
    if (!personId) next.personId = 'Selecione uma pessoa.'
    setErrors(next)
    if (Object.keys(next).length > 0) return
    await onSubmit({
      description: trimmed,
      amount: amountNum,
      type,
      categoryId,
      personId,
    })
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label
          htmlFor="tx-desc"
          className="mb-1 block text-sm font-medium text-slate-700"
        >
          Descrição
        </label>
        <input
          id="tx-desc"
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
          htmlFor="tx-amount"
          className="mb-1 block text-sm font-medium text-slate-700"
        >
          Valor
        </label>
        <input
          id="tx-amount"
          type="number"
          min={0.01}
          step={0.01}
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        />
        {errors.amount && <p className="mt-1 text-xs text-red-600">{errors.amount}</p>}
      </div>
      <div>
        <label htmlFor="tx-type" className="mb-1 block text-sm font-medium text-slate-700">
          Tipo
        </label>
        <select
          id="tx-type"
          value={type}
          onChange={(e) =>
            handleTypeChange(Number(e.target.value) as TransactionType)
          }
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        >
          <option value={TransactionType.Expense}>Despesa</option>
          <option value={TransactionType.Income}>Receita</option>
        </select>
      </div>
      <div>
        <label
          htmlFor="tx-category"
          className="mb-1 block text-sm font-medium text-slate-700"
        >
          Categoria
        </label>
        <select
          id="tx-category"
          value={categoryId}
          onChange={(e) => setCategoryId(e.target.value)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        >
          <option value="">Selecione…</option>
          {filteredCategories.map((c) => (
            <option key={c.id} value={c.id}>
              {c.description}
            </option>
          ))}
        </select>
        {filteredCategories.length === 0 && (
          <p className="mt-1 text-xs text-amber-700">
            Nenhuma categoria compatível com este tipo. Cadastre uma categoria adequada.
          </p>
        )}
        {errors.categoryId && (
          <p className="mt-1 text-xs text-red-600">{errors.categoryId}</p>
        )}
      </div>
      <div>
        <label htmlFor="tx-person" className="mb-1 block text-sm font-medium text-slate-700">
          Pessoa
        </label>
        <select
          id="tx-person"
          value={personId}
          onChange={(e) => setPersonId(e.target.value)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        >
          <option value="">Selecione…</option>
          {persons.map((p) => (
            <option key={p.id} value={p.id}>
              {p.name}
            </option>
          ))}
        </select>
        {errors.personId && <p className="mt-1 text-xs text-red-600">{errors.personId}</p>}
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
