import axios from 'axios'
import { SERVER_API_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'

const accessToken = useAuthStore.getState().auth.accessToken
export const customersService = {
  getAllWithUser: async () => {
    const res = await axios
      .get(`${SERVER_API_URL}/Customers/getAll`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      })
      .then((res) => res.data)
    // console.log(data)
    return res
  },
  getCustomerById: async (customerId: number) => {
    const res = await axios
      .get(`${SERVER_API_URL}/Customers/${customerId}`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  createNewCustomer: async (data: any) => {
    const res = await axios
      .post(`${SERVER_API_URL}/Users/register`, data, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  updateCustomer: async (data: any) => {
    const res = await axios
      .put(`${SERVER_API_URL}/Customers/${data.customerId}`, data, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  deleteCustomer: async (id: string) => {
    const res = await axios
      .delete(`${SERVER_API_URL}/Users/${id}`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
}
