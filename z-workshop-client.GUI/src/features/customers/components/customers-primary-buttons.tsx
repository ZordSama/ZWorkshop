import { IconUserPlus } from '@tabler/icons-react'
import { Button } from '@/components/ui/button'
import { useCustomers } from '../context/customers-context'

export function CustomersPrimaryButtons() {
  const { setOpen } = useCustomers()
  return (
    <div className='flex gap-2'>

      <Button className='space-x-1' onClick={() => setOpen('add')}>
        <span>Thêm tài khoản khách hàng</span> <IconUserPlus size={18} />
      </Button>
    </div>
  )
}
