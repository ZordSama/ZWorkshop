import { z } from 'zod'

const productTypeSchema = z.union([
  z.literal('game'),
  z.literal('app'),
  z.literal('banned'),
])
export type ProductType = z.infer<typeof productTypeSchema>

const productSchema = z.object({
  productId: z.string(),
  name: z.string(),
  price: z.number(),
  type: productTypeSchema,
  genre: z.string(),
  desc: z.string(),
  approvedBy: z.string(),
  publisherId: z.string(),
  publisherName: z.string(),
  createdAt: z.coerce.date(),
  lastUpdate: z.coerce.date(),
})

export type Product = z.infer<typeof productSchema>
export const productListSchema = z.array(productSchema)
