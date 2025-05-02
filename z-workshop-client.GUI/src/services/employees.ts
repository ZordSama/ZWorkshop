import axios from 'axios'
import { SERVER_API_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'

export const employeesService = {
  getAll: async () => {
    const authState = useAuthStore.getState().auth
    const res = await axios
      .get(`${SERVER_API_URL}/Employees/getAll`, {
        headers: {
          Authorization: `Bearer ${authState.accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  getEmployeeById: async (id: any) => {
    const authState = useAuthStore.getState().auth
    const res = await axios
      .get(`${SERVER_API_URL}/Employees/${id}`, {
        headers: {
          Authorization: `Bearer ${authState.accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },

  createNewEmployee: async (data: any) => {
    const authState = useAuthStore.getState().auth
    const res = await axios
      .post(`${SERVER_API_URL}/Users/employee-issue`, data, {
        headers: {
          Authorization: `Bearer ${authState.accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  updateEmployee: async (data: any) => {
    const authState = useAuthStore.getState().auth
    const res = await axios
      .put(`${SERVER_API_URL}/Employees/${data.employeeId}`, data, {
        headers: {
          Authorization: `Bearer ${authState.accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
  deleteEmployee: async (id: string) => {
    const authState = useAuthStore.getState().auth
    const res = await axios
      .delete(`${SERVER_API_URL}/Users/${id}`, {
        headers: {
          Authorization: `Bearer ${authState.accessToken}`,
        },
      })
      .then((res) => res.data)
    return res
  },
}
