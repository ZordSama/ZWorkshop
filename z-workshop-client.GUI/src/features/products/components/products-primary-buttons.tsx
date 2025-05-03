import { IconUserPlus } from '@tabler/icons-react'
import { Button } from '@/components/ui/button'
import { useProducts } from '../context/products-context'

export function ProductsPrimaryButtons() {
  const { setOpen } = useProducts()
  return (
    <div className='flex gap-2'>

      <Button className='space-x-1' onClick={() => setOpen('add')}>
        <span>Thêm nhà phát hành</span> <IconUserPlus size={18} />
      </Button>
    </div>
  )
}
