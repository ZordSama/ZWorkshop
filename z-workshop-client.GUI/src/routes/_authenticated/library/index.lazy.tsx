import Library from '@/features/library'
import { createLazyFileRoute } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/_authenticated/library/')({
  component: Library,
})

