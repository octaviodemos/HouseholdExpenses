import { useCallback, useEffect, useState } from 'react'
import {
  createPerson,
  deletePerson,
  getPersons,
  updatePerson,
} from '../../api/personApi'
import ConfirmDialog from '../../components/shared/ConfirmDialog'
import LoadingSpinner from '../../components/shared/LoadingSpinner'
import Modal from '../../components/shared/Modal'
import PersonForm from '../../components/forms/PersonForm'
import type { CreatePersonRequest, Person } from '../../types/person'

/**
 * CRUD de pessoas: listagem em tabela e fluxos modais para criar, editar e excluir.
 */
export default function PersonsPage() {
  const [persons, setPersons] = useState<Person[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [formOpen, setFormOpen] = useState(false)
  const [editing, setEditing] = useState<Person | null>(null)
  const [formSubmitting, setFormSubmitting] = useState(false)
  const [deleteTarget, setDeleteTarget] = useState<Person | null>(null)
  const [deleteSubmitting, setDeleteSubmitting] = useState(false)

  const refresh = useCallback(async () => {
    setError(null)
    const list = await getPersons()
    setPersons(list)
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
          setError(e instanceof Error ? e.message : 'Falha ao carregar pessoas.')
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

  function openCreate() {
    setEditing(null)
    setFormOpen(true)
  }

  function openEdit(p: Person) {
    setEditing(p)
    setFormOpen(true)
  }

  async function handleFormSubmit(data: CreatePersonRequest) {
    setFormSubmitting(true)
    setError(null)
    try {
      if (editing) {
        await updatePerson(editing.id, data)
      } else {
        await createPerson(data)
      }
      setFormOpen(false)
      setEditing(null)
      await refresh()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Não foi possível salvar.')
    } finally {
      setFormSubmitting(false)
    }
  }

  async function handleConfirmDelete() {
    if (!deleteTarget) return
    setDeleteSubmitting(true)
    setError(null)
    try {
      await deletePerson(deleteTarget.id)
      setDeleteTarget(null)
      await refresh()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Não foi possível excluir.')
    } finally {
      setDeleteSubmitting(false)
    }
  }

  if (loading) return <LoadingSpinner label="Carregando pessoas…" />

  return (
    <div>
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-slate-900">Pessoas</h1>
          <p className="mt-1 text-sm text-slate-600">Cadastro de membros do lar.</p>
        </div>
        <button
          type="button"
          onClick={openCreate}
          className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
        >
          Nova pessoa
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
              <th className="px-4 py-3 font-semibold text-slate-700">Nome</th>
              <th className="px-4 py-3 font-semibold text-slate-700">Idade</th>
              <th className="px-4 py-3 font-semibold text-slate-700">Ações</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100">
            {persons.length === 0 ? (
              <tr>
                <td colSpan={3} className="px-4 py-8 text-center text-slate-500">
                  Nenhuma pessoa cadastrada.
                </td>
              </tr>
            ) : (
              persons.map((p) => (
                <tr key={p.id} className="hover:bg-slate-50/80">
                  <td className="px-4 py-3 font-medium text-slate-900">{p.name}</td>
                  <td className="px-4 py-3 text-slate-600">{p.age}</td>
                  <td className="px-4 py-3">
                    <div className="flex flex-wrap gap-2">
                      <button
                        type="button"
                        onClick={() => openEdit(p)}
                        className="rounded-md border border-slate-200 px-2 py-1 text-xs font-medium text-slate-700 hover:bg-slate-50"
                      >
                        Editar
                      </button>
                      <button
                        type="button"
                        onClick={() => setDeleteTarget(p)}
                        className="rounded-md border border-red-200 px-2 py-1 text-xs font-medium text-red-700 hover:bg-red-50"
                      >
                        Excluir
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <Modal
        open={formOpen}
        title={editing ? 'Editar pessoa' : 'Nova pessoa'}
        onClose={() => {
          if (!formSubmitting) {
            setFormOpen(false)
            setEditing(null)
          }
        }}
      >
        <PersonForm
          key={editing?.id ?? 'new'}
          initialData={editing}
          onSubmit={handleFormSubmit}
          onCancel={() => {
            if (!formSubmitting) {
              setFormOpen(false)
              setEditing(null)
            }
          }}
          isSubmitting={formSubmitting}
        />
      </Modal>

      <ConfirmDialog
        open={!!deleteTarget}
        title="Excluir pessoa"
        message={
          deleteTarget
            ? `Tem certeza que deseja excluir "${deleteTarget.name}"? Esta ação pode remover transações associadas.`
            : ''
        }
        onConfirm={handleConfirmDelete}
        onCancel={() => {
          if (!deleteSubmitting) setDeleteTarget(null)
        }}
        isSubmitting={deleteSubmitting}
        confirmLabel="Excluir"
      />
    </div>
  )
}
