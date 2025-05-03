import { z } from 'zod'

const publisherStatusSchema = z.union([
  z.literal('active'),
  z.literal('warning'),
  z.literal('banned'),
])
export type PublisherStatus = z.infer<typeof publisherStatusSchema>

const publisherSchema = z.object({
  publisherId: z.string(),
  name: z.string(),
  avt: z.string(),
  email: z.string(),
  status: z.number().transform((numStatus, ctx): PublisherStatus => {
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
  createdAt: z.coerce.date(),
  lastUpdate: z.coerce.date(),
})

export type Publisher = z.infer<typeof publisherSchema>
export const publisherListSchema = z.array(publisherSchema)
