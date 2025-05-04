'use client'

import { useState } from 'react'
import { useMutation } from '@tanstack/react-query'
import { IconAlertTriangle } from '@tabler/icons-react'
import { publisherService } from '@/services/publishers'
import { toast } from '@/hooks/use-toast'
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { ConfirmDialog } from '@/components/confirm-dialog'
import { Publisher } from '../data/schema'

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: Publisher
  onSuccess: () => void
}

export function PublishersDeleteDialog({
  open,
  onOpenChange,
  currentRow,
  onSuccess,
}: Props) {
  const [value, setValue] = useState('')
  const publisherDeleteMutation = useMutation({
    mutationFn: publisherService.deletePublisher,
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
    if (value.trim() !== currentRow.email) return
    publisherDeleteMutation.mutate(currentRow.publisherId)
  }

  return (
    <ConfirmDialog
      open={open}
      onOpenChange={onOpenChange}
      handleConfirm={handleDelete}
      disabled={value.trim() !== currentRow.email}
      title={
        <span className='text-destructive'>
          <IconAlertTriangle
            className='mr-1 inline-block stroke-destructive'
            size={18}
          />{' '}
          Xóa nhà phát hành
        </span>
      }
      desc={
        <div className='space-y-4'>
          <p className='mb-2'>
            Bạn có chắc muốn xóa nhà phát hành:{' '}
            <span className='font-bold'>{currentRow.email}</span>
            ?
            <br />
            Với lệnh này bạn sẽ xóa nhà phát hành{' '}
            <span className='font-bold'>{currentRow.name}</span> với email{' '}
            <span className='font-bold'>{currentRow.email}</span> khỏi hệ thống.
            Hành động này là không thể hoàn tác.
          </p>

          <Label className='my-2'>
            Tên đăng nhập:
            <Input
              value={value}
              onChange={(e) => setValue(e.target.value)}
              placeholder='Hãy nhập lại email của nhà phát hành để xác nhận.'
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
