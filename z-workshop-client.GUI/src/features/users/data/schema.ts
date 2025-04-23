import { z } from 'zod'

const userRoleSchema = z.union([
  z.literal('superadmin'),
  z.literal('admin'),
  z.literal('cashier'),
  z.literal('manager'),
])

const userSchema = z.object({
  userId: z.string(),
  username: z.string(),
  role: userRoleSchema,
  createdAt: z.coerce.date().optional(),
  lastUpdate: z.coerce.date().optional(),
})
export type User = z.infer<typeof userSchema>

export const userListSchema = z.array(userSchema)
