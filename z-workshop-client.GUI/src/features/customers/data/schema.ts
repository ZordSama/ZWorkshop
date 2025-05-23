import { z } from 'zod'

const customerStatusSchema = z.union([
  z.literal('active'),
  z.literal('warning'),
  z.literal('banned'),
])
export type CustomerStatus = z.infer<typeof customerStatusSchema>

const userDtoSchema = z.object({
  userId: z.string(),
  username: z.string(),
})

const customerDtoSchema = z.object({
  customerId: z.string(),
  fullname: z.string(),
  dob: z.coerce.date(),
  address: z.string(),
  phone: z.string(),
  email: z.string(),
  status: z.number().transform((numStatus, ctx): CustomerStatus => {
    switch (numStatus) {
      case 0:
        return 'active'
      case -1:
        return 'warning'
      case -2:
        return 'banned'
      default:
        // Handle unexpected status numbers: Add an issue to Zod's error context
        ctx.addIssue({
          code: z.ZodIssueCode.custom, // Use 'custom' for non-standard errors
          message: `Invalid status number: ${numStatus}. Expected 0, -1, or -2.`,
        })
        return z.NEVER
    }
  }),
  userId: z.string(),
  createdAt: z.coerce.date(),
  lastUpdate: z.coerce.date(),
})

const customerWithUser = z.object({
  customerDto: customerDtoSchema,
  userDto: userDtoSchema,
})

export type Customer = z.infer<typeof customerWithUser>
export const customerListSchema = z.array(customerWithUser)
