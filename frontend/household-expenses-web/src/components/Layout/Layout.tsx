import { Outlet } from 'react-router-dom'
import Sidebar from './Sidebar'

/**
 * Esqueleto da aplicação: barra lateral fixa + área de conteúdo com as rotas filhas.
 */
export default function Layout() {
  return (
    <div className="flex min-h-screen bg-slate-50">
      <Sidebar />
      <main className="min-w-0 flex-1 overflow-auto">
        <div className="mx-auto max-w-6xl px-6 py-8">
          <Outlet />
        </div>
      </main>
    </div>
  )
}
