import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'

const linkClass =
  'block rounded-lg px-3 py-2 text-sm font-medium text-slate-600 transition hover:bg-slate-100 hover:text-slate-900'
const activeClass = 'bg-indigo-50 text-indigo-700 hover:bg-indigo-50 hover:text-indigo-700'

/**
 * Navegação lateral: destaca a rota ativa com base no pathname atual.
 */
export default function Sidebar() {
  const { email, logout } = useAuth()
  const navigate = useNavigate()

  function handleLogout() {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <aside className="flex w-56 shrink-0 flex-col border-r border-slate-200 bg-white">
      <div className="border-b border-slate-100 px-4 py-5">
        <span className="text-base font-bold tracking-tight text-slate-900">
          Household Expenses
        </span>
        <p className="mt-1 text-xs text-slate-500">Gestão doméstica</p>
      </div>
      <nav className="flex flex-1 flex-col gap-0.5 p-3" aria-label="Principal">
        <NavLink
          to="/"
          end
          className={({ isActive }) => `${linkClass} ${isActive ? activeClass : ''}`}
        >
          Dashboard
        </NavLink>
        <NavLink
          to="/persons"
          end
          className={({ isActive }) => `${linkClass} ${isActive ? activeClass : ''}`}
        >
          Pessoas
        </NavLink>
        <NavLink
          to="/persons/totals"
          className={({ isActive }) => `${linkClass} ${isActive ? activeClass : ''}`}
        >
          Totais por pessoa
        </NavLink>
        <NavLink
          to="/categories"
          end
          className={({ isActive }) => `${linkClass} ${isActive ? activeClass : ''}`}
        >
          Categorias
        </NavLink>
        <NavLink
          to="/categories/totals"
          className={({ isActive }) => `${linkClass} ${isActive ? activeClass : ''}`}
        >
          Totais por categoria
        </NavLink>
        <NavLink
          to="/transactions"
          className={({ isActive }) => `${linkClass} ${isActive ? activeClass : ''}`}
        >
          Transações
        </NavLink>
      </nav>

      <div className="border-t border-slate-100 p-3">
        <p className="truncate px-3 text-xs text-slate-500" title={email ?? undefined}>
          {email ?? '—'}
        </p>
        <button
          type="button"
          onClick={handleLogout}
          className="mt-2 w-full rounded-lg border border-slate-200 px-3 py-2 text-left text-sm font-medium text-slate-700 transition hover:bg-slate-50"
        >
          Sair
        </button>
      </div>
    </aside>
  )
}
