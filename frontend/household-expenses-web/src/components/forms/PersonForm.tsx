import { useState, type FormEvent } from 'react'
import type { CreatePersonRequest, Person } from '../../types/person'

type PersonFormProps = {
  initialData?: Person | null
  onSubmit: (data: CreatePersonRequest) => void | Promise<void>
  onCancel: () => void
  isSubmitting?: boolean
}

/**
 * Formulário de criação/edição de pessoa com validação básica no cliente
 * (espelha limites usados no backend).
 */
export default function PersonForm({
  initialData,
  onSubmit,
  onCancel,
  isSubmitting = false,
}: PersonFormProps) {
  const [name, setName] = useState(initialData?.name ?? '')
  const [age, setAge] = useState(initialData?.age !== undefined ? String(initialData.age) : '')
  const [errors, setErrors] = useState<{ name?: string; age?: string }>({})

  function validate(): boolean {
    const next: typeof errors = {}
    const trimmed = name.trim()
    if (!trimmed) next.name = 'Nome é obrigatório.'
    else if (trimmed.length > 200) next.name = 'Máximo de 200 caracteres.'
    const ageNum = Number(age)
    if (age === '' || Number.isNaN(ageNum)) next.age = 'Idade é obrigatória.'
    else if (ageNum < 1) next.age = 'Idade mínima é 1.'
    setErrors(next)
    return Object.keys(next).length === 0
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    if (!validate()) return
    await onSubmit({
      name: name.trim(),
      age: Number(age),
    })
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label htmlFor="person-name" className="mb-1 block text-sm font-medium text-slate-700">
          Nome
        </label>
        <input
          id="person-name"
          type="text"
          maxLength={200}
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        />
        {errors.name && <p className="mt-1 text-xs text-red-600">{errors.name}</p>}
      </div>
      <div>
        <label htmlFor="person-age" className="mb-1 block text-sm font-medium text-slate-700">
          Idade
        </label>
        <input
          id="person-age"
          type="number"
          min={1}
          step={1}
          value={age}
          onChange={(e) => setAge(e.target.value)}
          className="w-full rounded-lg border border-slate-200 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        />
        {errors.age && <p className="mt-1 text-xs text-red-600">{errors.age}</p>}
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
