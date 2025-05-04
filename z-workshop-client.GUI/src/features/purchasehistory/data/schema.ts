import { z } from 'zod'

const purchaseSchema = z.object({
  purchaseId: z.string(),
  customerId: z.string(),
  customerFullname: z.string(),
  productId: z.string(),
  productName: z.string(),
  closeAt: z.coerce.date(),
  unitPrice: z.number(),
})

export type Purchase = z.infer<typeof purchaseSchema>
export const purchaseListSchema = z.array(purchaseSchema)
