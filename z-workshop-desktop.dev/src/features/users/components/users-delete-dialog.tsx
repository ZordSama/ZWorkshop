'use client'

import { useState } from 'react'
import axios from 'axios'
import { IconAlertTriangle } from '@tabler/icons-react'
import { handleQueryError } from '@/utils'
import { toast } from '@/hooks/use-toast'
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { ConfirmDialog } from '@/components/confirm-dialog'
import { User } from '../data/schema'

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: User
  onSuccess: () => void
}

export function UsersDeleteDialog({
  open,
  onOpenChange,
  currentRow,
  onSuccess,
}: Props) {
  const [value, setValue] = useState('')

  const handleDelete = async () => {
    if (value.trim() !== currentRow.username) return

    try {
      await axios.delete(`http://localhost:5062/api/users/${currentRow.userId}`)
      toast({
        title: 'The following user has been deleted:',
        description: (
          <pre className='mt-2 w-[340px] rounded-md bg-slate-950 p-4'>
            <code className='text-white'>
              {JSON.stringify(currentRow, null, 2)}
            </code>
          </pre>
        ),
      })
      onSuccess()
    } catch (error: any) {
      handleQueryError(error)
    } finally {
      onOpenChange(false)
    }
  }

  return (
    <ConfirmDialog
      open={open}
      onOpenChange={onOpenChange}
      handleConfirm={handleDelete}
      disabled={value.trim() !== currentRow.username}
      title={
        <span className='text-destructive'>
          <IconAlertTriangle
            className='mr-1 inline-block stroke-destructive'
            size={18}
          />{' '}
          Xóa người dùng
        </span>
      }
      desc={
        <div className='space-y-4'>
          <p className='mb-2'>
            Bạn có chắc muốn xóa người dùng:{' '}
            <span className='font-bold'>{currentRow.username}</span>?
            <br />
            Với lệnh này bạn sẽ xóa người dùng với vai trò{' '}
            <span className='font-bold'>
              {currentRow.role.toUpperCase()}
            </span>{' '}
            khỏi hệ thống. Hành động này không thể hoàn tác.
          </p>

          <Label className='my-2'>
            Username:
            <Input
              value={value}
              onChange={(e) => setValue(e.target.value)}
              placeholder='Hãy nhập lại tên người dùng để xác nhận.'
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
