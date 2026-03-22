import { useCallback, useEffect, useState } from 'react'
import { createCategory, getCategories } from '../../api/categoryApi'
import CategoryForm from '../../components/forms/CategoryForm'
import LoadingSpinner from '../../components/shared/LoadingSpinner'
import Modal from '../../components/shared/Modal'
import type { Category, CreateCategoryRequest } from '../../types/category'
import { categoryPurposeLabel } from '../../types/category'

/**
 * Lista categorias e permite cadastro via modal (sem edição na API).
 */
export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [modalOpen, setModalOpen] = useState(false)
  const [submitting, setSubmitting] = useState(false)

  const refresh = useCallback(async () => {
    setError(null)
    const list = await getCategories()
    setCategories(list)
  }, [])

  useEffect(() => {
    let cancelled = false
    async function load() {
      setLoading(true)
      setError(null)
      try {
        await refresh()
      } catch (e) {
        if (!cancelled) {
          setError(e instanceof Error ? e.message : 'Falha ao carregar categorias.')
        }
      } finally {
        if (!cancelled) setLoading(false)
      }
    }
    void load()
    return () => {
      cancelled = true
    }
  }, [refresh])

  async function handleCreate(data: CreateCategoryRequest) {
    setSubmitting(true)
    setError(null)
    try {
      await createCategory(data)
      setModalOpen(false)
      await refresh()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Não foi possível salvar.')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return <LoadingSpinner label="Carregando categorias…" />

  return (
    <div>
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-slate-900">Categorias</h1>
          <p className="mt-1 text-sm text-slate-600">Classificação das transações.</p>
        </div>
        <button
          type="button"
          onClick={() => setModalOpen(true)}
          className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
        >
          Nova categoria
        </button>
      </div>

      {error && (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-800">
          {error}
        </div>
      )}

      <div className="mt-6 overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
        <table className="min-w-full divide-y divide-slate-200 text-left text-sm">
          <thead className="bg-slate-50">
            <tr>
              <th className="px-4 py-3 font-semibold text-slate-700">Descrição</th>
              <th className="px-4 py-3 font-semibold text-slate-700">Finalidade</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100">
            {categories.length === 0 ? (
              <tr>
                <td colSpan={2} className="px-4 py-8 text-center text-slate-500">
                  Nenhuma categoria cadastrada.
                </td>
              </tr>
            ) : (
              categories.map((c) => (
                <tr key={c.id} className="hover:bg-slate-50/80">
                  <td className="px-4 py-3 font-medium text-slate-900">{c.description}</td>
                  <td className="px-4 py-3 text-slate-600">{categoryPurposeLabel(c.purpose)}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <Modal
        open={modalOpen}
        title="Nova categoria"
        onClose={() => {
          if (!submitting) setModalOpen(false)
        }}
      >
        <CategoryForm
          onSubmit={handleCreate}
          onCancel={() => {
            if (!submitting) setModalOpen(false)
          }}
          isSubmitting={submitting}
        />
      </Modal>
    </div>
  )
}
