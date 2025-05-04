export interface IAppCapsule {
  id: number
  name: string
  thumbnail: string
  fallback: string
  price: number
}
export interface AuthUser {
  userId: string
  username: string
  role: ('Admin' | 'Customer' | 'SuperAdmin' | 'Guest')[]
  avatar: string | undefined
}

export interface UpdateWithFileMutationProps {
  id: string
  data: FormData
}

export interface CustomerPurchase {
  purchaseId: string
  customerId: string
  customerFullname: string
  productId: string
  productName: string
  closeAt: Date
  unitPrice: number
}
