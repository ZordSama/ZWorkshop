import { ProductType } from './schema'

export const callTypes = new Map<ProductType, string>([
  ['game', 'bg-blue-100/30 text-blue-900 dark:text-blue-200 border-blue-200'],
  [
    'app',
    'bg-yellow-100/30 text-yellow-900 dark:text-yellow-200 border-yellow-200',
  ],
])
