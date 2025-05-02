import axios from 'axios'
import { toast } from '@/hooks/use-toast'

export function handleQueryError(err: any) {
  console.error(err)
  if (err?.name === 'ZodError') {
    toast({
      title: 'Validation error',
      description: err.errors
        .map((e: { message: any }) => e.message)
        .join(', '),
      variant: 'destructive',
    })
    console.error('Validation error:', err)
  } else if (axios.isAxiosError(err)) {
    // setError(err.response?.data?.message || 'API error')
    toast({
      title: 'API error',
      description: err.response?.data?.message || 'API error',
      variant: 'destructive',
    })
  } else {
    // setError('Unexpected error')
    toast({
      title: 'Unexpected error',
      description: 'Unexpected error',
      variant: 'destructive',
    })
  }
}

export const SERVER_API_URL = 'http://localhost:5045/api'
