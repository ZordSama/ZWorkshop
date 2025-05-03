import { createLazyFileRoute } from '@tanstack/react-router'
import Publishers from '@/features/publishers'

export const Route = createLazyFileRoute('/_authenticated/publishers/')({
  component: Publishers,
})
