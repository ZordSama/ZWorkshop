'use client'

import { useState } from 'react'
import { useMutation } from '@tanstack/react-query'
import { IconAlertTriangle } from '@tabler/icons-react'
import { employeesService } from '@/services/employees'
import { toast } from '@/hooks/use-toast'
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { ConfirmDialog } from '@/components/confirm-dialog'
import { Employee } from '../data/schema'

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: Employee
  onSuccess: () => void
}

export function EmployeesDeleteDialog({
  open,
  onOpenChange,
  currentRow,
  onSuccess,
}: Props) {
  const [value, setValue] = useState('')
  const employeeDeleteMutation = useMutation({
    mutationFn: employeesService.deleteEmployee,
    onSuccess: (data) => {
      onSuccess()
      onOpenChange(false)
      toast({
        title: 'Xóa thành công',
        description: data.message,
      })
    },
  })

  const handleDelete = async () => {
    if (value.trim() !== currentRow.userDto.username) return
    employeeDeleteMutation.mutate(currentRow.userDto.userId)
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
          Xóa nhân viên
        </span>
      }
      desc={
        <div className='space-y-4'>
          <p className='mb-2'>
            Bạn có chắc muốn xóa nhân viên:{' '}
            <span className='font-bold'>{currentRow.employeeDto.fullname}</span>
            ?
            <br />
            Với lệnh này bạn sẽ xóa nhân viên{' '}
            <span className='font-bold'>
              {currentRow.employeeDto.fullname}
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
              placeholder='Hãy nhập lại tên đăng nhập của nhân viên để xác nhận.'
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
