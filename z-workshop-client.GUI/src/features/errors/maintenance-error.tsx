import { Button } from '@/components/ui/button'

export default function MaintenanceError() {
  return (
    <div className='h-svh'>
      <div className='m-auto flex h-full w-full flex-col items-center justify-center gap-2'>
        <h1 className='text-[7rem] font-bold leading-tight'>503</h1>
        <span className='font-medium'>Hệ thống đang bảo trì!</span>
        <p className='text-center text-muted-foreground'>
          Zworkshop đang được bảo trì nâng cấp <br />
          Chúng tôi sẽ sớm hoạt động trở lại.
        </p>
        <div className='mt-6 flex gap-4'>
          <Button variant='outline'>Chi tiết</Button>
        </div>
      </div>
    </div>
  )
}
