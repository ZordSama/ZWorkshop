import { CustomerStatus } from './schema'

export const callTypes = new Map<CustomerStatus, string>([
  ['active', 'bg-teal-100/30 text-teal-900 dark:text-teal-200 border-teal-200'],
  [
    'warning',
    'bg-yellow-100/30 text-yellow-900 dark:text-yellow-200 border-yellow-200',
  ],
  [
    'banned',
    'bg-destructive/10 dark:bg-destructive/50 text-destructive dark:text-primary border-destructive/10',
  ],
])
