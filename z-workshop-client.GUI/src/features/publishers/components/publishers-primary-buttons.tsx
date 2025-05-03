import { IconUserPlus } from '@tabler/icons-react'
import { Button } from '@/components/ui/button'
import { usePublishers } from '../context/publishers-context'

export function PublishersPrimaryButtons() {
  const { setOpen } = usePublishers()
  return (
    <div className='flex gap-2'>

      <Button className='space-x-1' onClick={() => setOpen('add')}>
        <span>Thêm nhà phát hành</span> <IconUserPlus size={18} />
      </Button>
    </div>
  )
}
