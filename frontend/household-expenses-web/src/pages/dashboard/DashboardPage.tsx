import { useEffect, useState } from 'react'
import { getCategories } from '../../api/categoryApi'
import { getPersons, getPersonTotals } from '../../api/personApi'
import { getTransactions } from '../../api/transactionApi'
import LoadingSpinner from '../../components/shared/LoadingSpinner'

function formatBrl(value: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(
    value,
  )
}

/**
 * Visão geral: contagens principais e saldo consolidado (soma dos saldos por pessoa).
 */
export default function DashboardPage() {
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [counts, setCounts] = useState({ persons: 0, categories: 0, transactions: 0 })
  const [balance, setBalance] = useState(0)

  useEffect(() => {
    let cancelled = false
    async function load() {
      setLoading(true)
      setError(null)
      try {
        const [persons, categories, transactions, totals] = await Promise.all([
          getPersons(),
          getCategories(),
          getTransactions(),
          getPersonTotals(),
        ])
        if (cancelled) return
        setCounts({
          persons: persons.length,
          categories: categories.length,
          transactions: transactions.length,
        })
        setBalance(totals.summary.balance)
      } catch (e) {
        if (!cancelled) {
          setError(e instanceof Error ? e.message : 'Não foi possível carregar o dashboard.')
        }
      } finally {
        if (!cancelled) setLoading(false)
      }
    }
    void load()
    return () => {
      cancelled = true
    }
  }, [])

  if (loading) return <LoadingSpinner label="Carregando dashboard…" />
  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-800">
        {error}
      </div>
    )
  }

  const balancePositive = balance >= 0

  return (
    <div>
      <h1 className="text-2xl font-bold tracking-tight text-slate-900">Dashboard</h1>
      <p className="mt-1 text-sm text-slate-600">Resumo dos dados cadastrados na aplicação.</p>

      <div className="mt-8 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <div className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm">
          <p className="text-sm font-medium text-slate-500">Total de pessoas</p>
          <p className="mt-2 text-3xl font-semibold text-slate-900">{counts.persons}</p>
        </div>
        <div className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm">
          <p className="text-sm font-medium text-slate-500">Total de categorias</p>
          <p className="mt-2 text-3xl font-semibold text-slate-900">{counts.categories}</p>
        </div>
        <div className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm">
          <p className="text-sm font-medium text-slate-500">Total de transações</p>
          <p className="mt-2 text-3xl font-semibold text-slate-900">{counts.transactions}</p>
        </div>
        <div
          className={`rounded-xl border p-5 shadow-sm ${
            balancePositive
              ? 'border-emerald-200 bg-emerald-50'
              : 'border-red-200 bg-red-50'
          }`}
        >
          <p
            className={`text-sm font-medium ${balancePositive ? 'text-emerald-800' : 'text-red-800'}`}
          >
            Saldo geral
          </p>
          <p
            className={`mt-2 text-3xl font-semibold ${balancePositive ? 'text-emerald-700' : 'text-red-700'}`}
          >
            {formatBrl(balance)}
          </p>
        </div>
      </div>
    </div>
  )
}
