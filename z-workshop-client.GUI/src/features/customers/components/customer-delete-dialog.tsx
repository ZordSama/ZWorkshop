'use client'

import { useState } from 'react'
import { useMutation } from '@tanstack/react-query'
import { IconAlertTriangle } from '@tabler/icons-react'
import { customersService } from '@/services/customers'
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { ConfirmDialog } from '@/components/confirm-dialog'
import { Customer } from '../data/schema'
import { toast } from '@/hooks/use-toast'

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: Customer
  onSuccess: () => void
}

export function CustomersDeleteDialog({
  open,
  onOpenChange,
  currentRow,
  onSuccess,
}: Props) {
  const [value, setValue] = useState('')
  const customerDeleteMutation = useMutation({
    mutationFn: customersService.deleteCustomer,
    onSuccess: (data) => {
      onSuccess()
      onOpenChange(false)
      toast({
        title: 'Xóa thành công',
        description: data.message
      })
    },
  })

  const handleDelete = async () => {
    if (value.trim() !== currentRow.userDto.username) return
    customerDeleteMutation.mutate(currentRow.userDto.userId)
  }

  return (
    <ConfirmDialog
      open={open}
      onOpenChange={onOpenChange}
      handleConfirm={handleDelete}
      disabled={value.trim() !== currentRow.userDto.username}
      title={
        <span className='text-destructive'>
          <IconAlertTriangle
            className='mr-1 inline-block stroke-destructive'
            size={18}
          />{' '}
          Xóa khách hàng
        </span>
      }
      desc={
        <div className='space-y-4'>
          <p className='mb-2'>
            Bạn có chắc muốn xóa khách hàng:{' '}
            <span className='font-bold'>{currentRow.customerDto.fullname}</span>
            ?
            <br />
            Với lệnh này bạn sẽ xóa khách hàng{' '}
            <span className='font-bold'>
              {currentRow.customerDto.fullname}
            </span>{' '}
            với tên đăng nhập{' '}
            <span className='font-bold'>{currentRow.userDto.username}</span>{' '}
            khỏi hệ thống. Hành động này là không thể hoàn tác.
          </p>

          <Label className='my-2'>
            Tên đăng nhập:
            <Input
              value={value}
              onChange={(e) => setValue(e.target.value)}
              placeholder='Hãy nhập lại tên đăng nhập của khách hàng để xác nhận.'
            />
          </Label>

          <Alert variant='destructive'>
            <AlertTitle>Warning!</AlertTitle>
            <AlertDescription>
              Hãy thận trọng, hành động này là không thể hoàn tác.
            </AlertDescription>
          </Alert>
        </div>
      }
      confirmText='Xác nhận'
      destructive
    />
  )
}
