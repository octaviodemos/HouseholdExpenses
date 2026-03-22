import { useCallback, useEffect, useState } from 'react'
import { getCategories } from '../../api/categoryApi'
import { getPersons } from '../../api/personApi'
import { createTransaction, getTransactions } from '../../api/transactionApi'
import TransactionForm from '../../components/forms/TransactionForm'
import LoadingSpinner from '../../components/shared/LoadingSpinner'
import Modal from '../../components/shared/Modal'
import type { Category } from '../../types/category'
import type { CreateTransactionRequest, Transaction } from '../../types/transaction'
import { transactionTypeLabel } from '../../types/transaction'
import type { Person } from '../../types/person'

function formatBrl(value: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(
    value,
  )
}

/**
 * Lista transações e abre modal para novo lançamento (categorias/pessoas vindas da API).
 */
export default function TransactionsPage() {
  const [transactions, setTransactions] = useState<Transaction[]>([])
  const [categories, setCategories] = useState<Category[]>([])
  const [persons, setPersons] = useState<Person[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [modalOpen, setModalOpen] = useState(false)
  const [submitting, setSubmitting] = useState(false)

  const refresh = useCallback(async () => {
    setError(null)
    const [tx, cats, people] = await Promise.all([
      getTransactions(),
      getCategories(),
      getPersons(),
    ])
    setTransactions(tx)
    setCategories(cats)
    setPersons(people)
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
          setError(e instanceof Error ? e.message : 'Falha ao carregar transações.')
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

  async function handleCreate(data: CreateTransactionRequest) {
    setSubmitting(true)
    setError(null)
    try {
      await createTransaction(data)
      setModalOpen(false)
      await refresh()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Não foi possível salvar.')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return <LoadingSpinner label="Carregando transações…" />

  return (
    <div>
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-slate-900">Transações</h1>
          <p className="mt-1 text-sm text-slate-600">Histórico de receitas e despesas.</p>
        </div>
        <button
          type="button"
          onClick={() => setModalOpen(true)}
          className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700"
        >
          Nova transação
        </button>
      </div>

      {error && (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-800">
          {error}
        </div>
      )}

      <div className="mt-6 overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-slate-200 text-left text-sm">
            <thead className="bg-slate-50">
              <tr>
                <th className="px-4 py-3 font-semibold text-slate-700">Descrição</th>
                <th className="px-4 py-3 font-semibold text-slate-700">Valor</th>
                <th className="px-4 py-3 font-semibold text-slate-700">Tipo</th>
                <th className="px-4 py-3 font-semibold text-slate-700">Categoria</th>
                <th className="px-4 py-3 font-semibold text-slate-700">Pessoa</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {transactions.length === 0 ? (
                <tr>
                  <td colSpan={5} className="px-4 py-8 text-center text-slate-500">
                    Nenhuma transação cadastrada.
                  </td>
                </tr>
              ) : (
                transactions.map((t) => (
                  <tr key={t.id} className="hover:bg-slate-50/80">
                    <td className="px-4 py-3 font-medium text-slate-900">{t.description}</td>
                    <td className="px-4 py-3 text-slate-700">{formatBrl(t.amount)}</td>
                    <td className="px-4 py-3 text-slate-600">{transactionTypeLabel(t.type)}</td>
                    <td className="px-4 py-3 text-slate-600">{t.categoryDescription}</td>
                    <td className="px-4 py-3 text-slate-600">{t.personName}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      <Modal
        open={modalOpen}
        title="Nova transação"
        onClose={() => {
          if (!submitting) setModalOpen(false)
        }}
      >
        <TransactionForm
          categories={categories}
          persons={persons.map((p) => ({ id: p.id, name: p.name }))}
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
