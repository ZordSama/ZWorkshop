import axios from 'axios'
import { SERVER_API_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'

export const publisherService = {
  getPublishers: async () => {
    const res = await axios
      .get(`${SERVER_API_URL}/Publishers`)
      .then((res) => res.data)
    return res
  },
  getPublisher: async (id: string) => {
    const res = await axios
      .get(`${SERVER_API_URL}/Publishers/${id}`)
      .then((res) => res.data)
    return res
  },
  createPublisher: async (data: FormData) => {
    const authToken = useAuthStore.getState().auth.accessToken
    const res = await axios
      .post(`${SERVER_API_URL}/Publishers`, data, {
        headers: {
          Authorization: `Bearer ${authToken}`,
          'Content-Type': 'multipart/form-data',
        },
      })
      .then((res) => res.data)
    return res
  },
  updatePublisher: async (data: any) => {},
  deletePublisher: async (id: string) => {
    const authToken = useAuthStore.getState().auth.accessToken
    const res = await axios
      .delete(`${SERVER_API_URL}/Publishers/${id}`, {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
}
