import { DialogDescription } from '@radix-ui/react-dialog'
import { useMutation, useQuery } from '@tanstack/react-query'
import { publisherService } from '@/services/publishers'
import { shopService } from '@/services/shop'
import { SERVER_PUBLIC_URL } from '@/utils'
import { useAuthStore } from '@/stores/authStore'
import { toast } from '@/hooks/use-toast'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { Separator } from '@/components/ui/separator'
import { Product } from '../products/data/schema'

interface Props {
  product: Product
  open: boolean
  onOpenChange: (open: boolean) => void
}

export default function AppDetails({ product, open, onOpenChange }: Props) {
  const { data: rawPublisher } = useQuery({
    queryKey: ['publisher', product.publisherId],
    queryFn: () => publisherService.getPublisher(product.publisherId),
  })

  const purchaseMutation = useMutation({
    mutationFn: shopService.purchase,
    onSuccess: () => {
      toast({
        title: `Đã mua ${product.name}`,
        description: `${product.name} đã được thêm vào thư viện của bạn`,
      })
      onOpenChange(false)
    },
  })

  function onBuyBtnClick() {
    if (isBuyer) {
      purchaseMutation.mutate(product.productId)
    } else {
      toast({
        title: 'Bạn không có quyền thực hiện hành động này',
        description: 'Vui lòng đăng nhập với tài khoản người dùng',
        variant: 'destructive',
      })
    }
  }

  const userRole = useAuthStore.getState().auth.user?.role || 'Guest'
  const isBuyer = userRole.includes('Customer')

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogTrigger asChild>
        <Button>Chi tiết</Button>
      </DialogTrigger>
      <DialogContent className='max-w-[720px]'>
        <DialogHeader>
          <DialogTitle>{product.name}</DialogTitle>
          <DialogDescription></DialogDescription>
        </DialogHeader>
        <div className='flex flex-col gap-1 px-1'>
          <img
            src={`${SERVER_PUBLIC_URL}/${JSON.parse(product.desc).Thumbnail}`}
            alt={product.name}
            className='flex-auto rounded-md object-cover'
          />
          <h1 className='pt-1 text-lg font-bold'>Mô tả chi tiết:</h1>
          <span className='-mt-1'>{JSON.parse(product.desc).Description}</span>
          <Separator />
          <div className='flex w-full flex-auto flex-row justify-between'>
            <span className='my-auto font-bold'>Phát hành bởi:</span>
            <div className='flex flex-row items-end'>
              <img
                src={`${SERVER_PUBLIC_URL}/${rawPublisher?.data.avt}`}
                alt={rawPublisher?.data.name}
                className='h-8 w-8'
              />
              <span className='my-auto ml-2'>{rawPublisher?.data.name}</span>
            </div>
          </div>
          <Separator className='mb-1 mt-10' />
          <div className='flex w-full flex-auto flex-row justify-between'>
            <div className='flex flex-row items-start'>
              <span className='my-auto font-bold'>Giá bán:</span>
              <span className='my-auto ml-2'>
                {new Intl.NumberFormat('vi-VN', {
                  style: 'currency',
                  currency: 'VND',
                  currencyDisplay: 'code',
                }).format(product.price)}
              </span>
            </div>
            <Button
              className={`bg-green-500 hover:bg-green-600 ${isBuyer ? 'w-32' : 'w-96 hover:cursor-not-allowed'}`}
              disabled={!isBuyer}
              onClick={onBuyBtnClick}
            >
              {isBuyer ? 'Mua ngay' : 'hãy đăng nhập với tài khoản người dùng'}
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  )
}
