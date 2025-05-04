

import Purchases from '@/features/purchasehistory'
import { createLazyFileRoute } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/_authenticated/purchase-history/')({
  component: Purchases,
})

