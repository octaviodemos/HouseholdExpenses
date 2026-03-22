import { useEffect, useState } from 'react'
import { getPersonTotals } from '../../api/personApi'
import LoadingSpinner from '../../components/shared/LoadingSpinner'
import type { PersonTotalsResult } from '../../types/person'

function formatBrl(value: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(
    value,
  )
}

function balanceClass(value: number): string {
  if (value > 0) return 'font-semibold text-emerald-600'
  if (value < 0) return 'font-semibold text-red-600'
  return 'font-medium text-slate-700'
}

/**
 * Totais por pessoa e linha de resumo geral, com valores em BRL e cores por saldo.
 */
export default function PersonTotalsPage() {
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [data, setData] = useState<PersonTotalsResult | null>(null)

  useEffect(() => {
    let cancelled = false
    async function load() {
      setLoading(true)
      setError(null)
      try {
        const result = await getPersonTotals()
        if (!cancelled) setData(result)
      } catch (e) {
        if (!cancelled) {
          setError(e instanceof Error ? e.message : 'Falha ao carregar totais.')
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

  if (loading) return <LoadingSpinner label="Carregando totais…" />
  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-800">
        {error}
      </div>
    )
  }
  if (!data) return null

  const { persons, summary } = data

  return (
    <div>
      <h1 className="text-2xl font-bold tracking-tight text-slate-900">Totais por pessoa</h1>
      <p className="mt-1 text-sm text-slate-600">
        Receitas, despesas e saldo individual; rodapé com totais gerais.
      </p>

      <div className="mt-6 overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
        <table className="min-w-full divide-y divide-slate-200 text-left text-sm">
          <thead className="bg-slate-50">
            <tr>
              <th className="px-4 py-3 font-semibold text-slate-700">Nome</th>
              <th className="px-4 py-3 font-semibold text-slate-700">Total receitas</th>
              <th className="px-4 py-3 font-semibold text-slate-700">Total despesas</th>
              <th className="px-4 py-3 font-semibold text-slate-700">Saldo</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100">
            {persons.length === 0 ? (
              <tr>
                <td colSpan={4} className="px-4 py-8 text-center text-slate-500">
                  Nenhum dado para exibir.
                </td>
              </tr>
            ) : (
              persons.map((row) => (
                <tr key={row.id} className="hover:bg-slate-50/80">
                  <td className="px-4 py-3 font-medium text-slate-900">{row.name}</td>
                  <td className="px-4 py-3 text-slate-700">{formatBrl(row.totalIncome)}</td>
                  <td className="px-4 py-3 text-slate-700">{formatBrl(row.totalExpense)}</td>
                  <td className={`px-4 py-3 ${balanceClass(row.balance)}`}>
                    {formatBrl(row.balance)}
                  </td>
                </tr>
              ))
            )}
          </tbody>
          <tfoot className="border-t-2 border-slate-300 bg-indigo-50/80">
            <tr className="font-bold text-slate-900">
              <td className="px-4 py-3">Totais gerais</td>
              <td className="px-4 py-3">{formatBrl(summary.totalIncome)}</td>
              <td className="px-4 py-3">{formatBrl(summary.totalExpense)}</td>
              <td className={`px-4 py-3 ${balanceClass(summary.balance)}`}>
                {formatBrl(summary.balance)}
              </td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  )
}
