import { AxiosError } from 'axios'
import { toast } from '@/hooks/use-toast'

export function handleServerError(error: unknown) {
  // eslint-disable-next-line no-console
  // console.log(error)

  let errMsg = 'Something went wrong!'

  // toast({ variant: 'destructive', title: 'WHAT THE FUCK????' })
  if (error instanceof AxiosError) {
    errMsg = error.response?.data.message || errMsg
    // errMsg = error.response?
  }

  console.log(errMsg)
  toast({ variant: 'destructive', title: 'Lỗi', description: errMsg })
}
