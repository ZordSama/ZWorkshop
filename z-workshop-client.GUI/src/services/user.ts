import axios from 'axios'
import { SERVER_API_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'
import { AuthUser } from '@/utils/types'

// const authState = useAuthStore.getState().auth

export const userService = {
  login: async (data: any) => {
    const authState = useAuthStore.getState().auth
    const res = await axios
      .post(`${SERVER_API_URL}/Auth/login`, data)
      .then((res) => res.data)

    console.log(res)
    authState.setAccessToken(res.token)
  },
  me: async (): Promise<AuthUser> => {
    const authState = useAuthStore.getState().auth
    const user = await axios
      .get(`${SERVER_API_URL}/Auth/me`, {
        headers: {
          Authorization: `Bearer ${authState.accessToken}`,
        },
      })
      .then((res) => res.data.user)

    return user
  },
  logout: async () => {
    const authState = useAuthStore.getState().auth
    authState.reset()
  },
}
