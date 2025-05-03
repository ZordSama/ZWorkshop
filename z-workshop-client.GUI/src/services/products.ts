import axios from 'axios'
import { SERVER_API_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'

export const productService = {
  getProducts: async () => {
    const res = await axios
      .get(`${SERVER_API_URL}/Products/getAll`)
      .then((res) => res.data)

    return res
  },
  createProduct: async (product: FormData) => {
    const authToken = useAuthStore.getState().auth.accessToken
    const res = await axios
      .post(`${SERVER_API_URL}/Products`, product, {
        headers: {
          Authorization: `Bearer ${authToken}`,
          'Content-Type': 'multipart/form-data',
        },
      })
      .then((res) => res.data)
    return res
  },
  updateProduct: async (id: string, product: FormData) => {
    const authToken = useAuthStore.getState().auth.accessToken
    const res = await axios
      .put(`${SERVER_API_URL}/Products/${id}`, product, {
        headers: {
          Authorization: `Bearer ${authToken}`,
          'Content-Type': 'multipart/form-data',
        },
      })
      .then((res) => res.data)

    return res
  },
  deleteProduct: async (id: string) => {
    const authToken = useAuthStore.getState().auth.accessToken
    const res = await axios
      .delete(`${SERVER_API_URL}/Products/${id}`, {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      })
      .then((res) => res.data)

    return res
  },
}
