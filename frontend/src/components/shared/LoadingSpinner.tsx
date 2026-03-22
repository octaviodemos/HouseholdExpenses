/**
 * Indicador visual genérico usado enquanto dados assíncronos são carregados.
 */
export default function LoadingSpinner({ label = 'Carregando…' }: { label?: string }) {
  return (
    <div
      className="flex flex-col items-center justify-center gap-3 py-16 text-slate-600"
      role="status"
      aria-live="polite"
    >
      <div
        className="h-10 w-10 animate-spin rounded-full border-2 border-slate-200 border-t-indigo-600"
        aria-hidden
      />
      <span className="text-sm font-medium">{label}</span>
    </div>
  )
}
