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
  role: string
  avatar: string | undefined
}

export interface UpdateWithFileMutationProps {
  id: string
  data: FormData
}
