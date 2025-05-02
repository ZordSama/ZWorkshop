import Cookies from 'js-cookie'
import { create } from 'zustand'
import { AuthUser } from '@/utils/types'

const ACCESS_TOKEN = 'thisisjustarandomstring'

interface AuthState {
  auth: {
    user: AuthUser | null
    setUser: (user: AuthUser | null) => void
    accessToken: string
    setAccessToken: (accessToken: string) => void
    resetAccessToken: () => void
    reset: () => void
  }
}

export const useAuthStore = create<AuthState>()((set) => {
  const cookieState = Cookies.get(ACCESS_TOKEN)
  const cookieUser = Cookies.get('me')
  const initUser = cookieUser ? JSON.parse(cookieUser) : null
  const initToken = cookieState ? cookieState : ''
  return {
    auth: {
      user: initUser,
      setUser: (user) => {
        Cookies.set('me', JSON.stringify(user), { expires: 1 })
        set((state) => ({ ...state, auth: { ...state.auth, user } }))
        // console.log('setUser', user)
      },
      // set((state) => ({ ...state, auth: { ...state.auth, user } })),
      accessToken: initToken,
      setAccessToken: (accessToken) =>
        set((state) => {
          Cookies.set(ACCESS_TOKEN, accessToken, { expires: 1 })
          return { ...state, auth: { ...state.auth, accessToken } }
        }),
      resetAccessToken: () =>
        set((state) => {
          Cookies.remove(ACCESS_TOKEN)
          return { ...state, auth: { ...state.auth, accessToken: '' } }
        }),
      reset: () =>
        set((state) => {
          Cookies.remove(ACCESS_TOKEN)
          return {
            ...state,
            auth: { ...state.auth, user: null, accessToken: '' },
          }
        }),
    },
  }
})

// export const useAuth = () => useAuthStore((state) => state.auth)

// export const useAuthUser = () => useAuthStore((state) => state.auth.user)

// export const useAuthUserAndToken = () =>
//   useAuthStore((state) => ({
//     user: state.auth.user,
//     accessToken: state.auth.accessToken,
//   }))
// export const useAuthActions = () =>
//   useAuthStore((state) => ({
//     setUser: state.auth.setUser,
//     setAccessToken: state.auth.setAccessToken,
//     resetAccessToken: state.auth.resetAccessToken,
//     reset: state.auth.reset,
//   }))
