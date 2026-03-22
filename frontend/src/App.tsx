import { BrowserRouter, Route, Routes } from 'react-router-dom'
import PrivateRoute from './components/PrivateRoute'
import Layout from './components/Layout/Layout'
import { AuthProvider } from './contexts/AuthContext'
import LoginPage from './pages/auth/LoginPage'
import RegisterPage from './pages/auth/RegisterPage'
import CategoriesPage from './pages/categories/CategoriesPage'
import CategoryTotalsPage from './pages/categories/CategoryTotalsPage'
import DashboardPage from './pages/dashboard/DashboardPage'
import PersonsPage from './pages/persons/PersonsPage'
import PersonTotalsPage from './pages/persons/PersonTotalsPage'
import TransactionsPage from './pages/transactions/TransactionsPage'

/**
 * Rotas públicas (login/register) e área autenticada com layout e sidebar.
 */
export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          <Route element={<PrivateRoute />}>
            <Route element={<Layout />}>
              <Route path="/" element={<DashboardPage />} />
              <Route path="/persons" element={<PersonsPage />} />
              <Route path="/persons/totals" element={<PersonTotalsPage />} />
              <Route path="/categories" element={<CategoriesPage />} />
              <Route path="/categories/totals" element={<CategoryTotalsPage />} />
              <Route path="/transactions" element={<TransactionsPage />} />
            </Route>
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}
