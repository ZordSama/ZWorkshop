import ComingSoon from '@/components/coming-soon'
import { createLazyFileRoute } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/(errors)/comingsoon')({
  component: ComingSoon,
})

