import axios from 'axios'
import { SERVER_API_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'

export const shopService = {
  purchase: async (id: string) => {
    const token = useAuthStore.getState().auth.accessToken
    const res = await axios
      .post(
        `${SERVER_API_URL}/Shop/purchase/${id}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then((res) => res.data)
    return res
  },
  getPurchases: async () => {
    const token = useAuthStore.getState().auth.accessToken
    const res = await axios
      .get(`${SERVER_API_URL}/Shop/getAllPurchase`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  getCustomerPurchases: async (id: string) => {
    const token = useAuthStore.getState().auth.accessToken
    const res = await axios
      .get(`${SERVER_API_URL}/Shop/getCustomerPurchases/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  getCustomerLibrary: async () => {
    const user = useAuthStore.getState().auth.user
    const token = useAuthStore.getState().auth.accessToken
    const res = await axios
      .get(`${SERVER_API_URL}/Shop/getLib/${user?.userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => res.data)
    return res
  },
}
