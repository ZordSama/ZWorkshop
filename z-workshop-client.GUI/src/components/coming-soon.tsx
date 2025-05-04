import { Link } from '@tanstack/react-router'
import { IconPlanet } from '@tabler/icons-react'
import { Button } from './ui/button'

export default function ComingSoon() {
  return (
    <div className='h-svh'>
      <div className='m-auto flex h-full w-full flex-col items-center justify-center gap-2'>
        <IconPlanet size={72} />
        <h1 className='text-4xl font-bold leading-tight'>
          Chức năng sắp ra mắt 👀
        </h1>
        <p className='text-center text-muted-foreground'>
          Chức năng này đang trong quá trình phát triển <br />
          Hãy chờ chúng tôi một chút!
        </p>
        <Link to='/'>
          <Button variant={'outline'}>Quay về trang chủ</Button>
        </Link>
      </div>
    </div>
  )
}
