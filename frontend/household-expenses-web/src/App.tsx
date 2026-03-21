import { BrowserRouter, Route, Routes } from 'react-router-dom'
import Layout from './components/Layout/Layout'
import CategoriesPage from './pages/categories/CategoriesPage'
import CategoryTotalsPage from './pages/categories/CategoryTotalsPage'
import DashboardPage from './pages/dashboard/DashboardPage'
import PersonsPage from './pages/persons/PersonsPage'
import PersonTotalsPage from './pages/persons/PersonTotalsPage'
import TransactionsPage from './pages/transactions/TransactionsPage'

/**
 * Raiz da SPA: rotas aninhadas sob o layout com sidebar (React Router).
 */
export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/persons" element={<PersonsPage />} />
          <Route path="/persons/totals" element={<PersonTotalsPage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/categories/totals" element={<CategoryTotalsPage />} />
          <Route path="/transactions" element={<TransactionsPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
