'use client'

import { useState } from 'react'
import { useMutation } from '@tanstack/react-query'
import { IconAlertTriangle } from '@tabler/icons-react'
import { toast } from '@/hooks/use-toast'
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { ConfirmDialog } from '@/components/confirm-dialog'
import { Product } from '../data/schema'
import { productService } from '@/services/products'

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: Product
  onSuccess: () => void
}

export function ProductsDeleteDialog({
  open,
  onOpenChange,
  currentRow,
  onSuccess,
}: Props) {
  const [value, setValue] = useState('')
  const productDeleteMutation = useMutation({
    mutationFn: productService.deleteProduct,
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
    if (value.trim() !== currentRow.name) return
    productDeleteMutation.mutate(currentRow.productId)
  }

  return (
    <ConfirmDialog
      open={open}
      onOpenChange={onOpenChange}
      handleConfirm={handleDelete}
      disabled={value.trim() !== currentRow.name}
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
            Bạn có chắc muốn xóa sản phẩm:{' '}
            <span className='font-bold'>{currentRow.name}</span>
            ?
            <br />
            Với lệnh này bạn sẽ xóa sản phẩm{' '}
            <span className='font-bold'>{currentRow.name}</span> của nhà phát hành{' '}
            <span className='font-bold'>{currentRow.publisherName}</span> khỏi hệ thống.
            Hành động này là không thể hoàn tác.
          </p>

          <Label className='my-2'>
            Tên sản phẩm:
            <Input
              value={value}
              onChange={(e) => setValue(e.target.value)}
              placeholder='Hãy nhập lại tên của sản phẩm để xác nhận.'
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
