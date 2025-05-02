import { z } from 'zod'

const employeeStatusSchema = z.union([
  z.literal('active'),
  z.literal('warning'),
  z.literal('banned'),
])
export type EmployeeStatus = z.infer<typeof employeeStatusSchema>

const userDtoSchema = z.object({
  userId: z.string(),
  username: z.string(),
  role: z.union([z.literal('Admin'), z.literal('SuperAdmin')]),
})

const employeeDtoSchema = z.object({
  employeeId: z.string(),
  fullname: z.string(),
  dob: z.coerce.date(),
  role: z.union([z.literal('Admin'), z.literal('SuperAdmin')]).optional(),
  hiredDate: z.coerce.date(),
  address: z.string(),
  phone: z.string(),
  email: z.string(),
  status: z.number().transform((numStatus, ctx): EmployeeStatus => {
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

const employeeWithUser = z.object({
  employeeDto: employeeDtoSchema,
  userDto: userDtoSchema,
})

export type Employee = z.infer<typeof employeeWithUser>
export const employeeListSchema = z.array(employeeWithUser)
