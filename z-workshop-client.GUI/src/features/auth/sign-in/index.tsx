import { Card } from '@/components/ui/card'
import AuthLayout from '../auth-layout'
import { UserAuthForm } from './components/user-auth-form'

export default function SignIn() {
  return (
    <AuthLayout>
      <Card className='p-6 mx-auto'>
        <div className='flex flex-col space-y-2 text-left'>
          <h1 className='text-2xl font-semibold tracking-tight'>Đăng nhập</h1>
          <p className='text-sm text-muted-foreground'>
            Hãy sử dụng tên đăng nhập và mật khẩu của bạn<br />
            để đăng nhập vào hệ thống
          </p>
        </div>
        <UserAuthForm />
        <p className='mt-4 px-8 text-center text-sm text-muted-foreground'>
          Bằng cách đăng nhập vào hệ thống, bạn đã đồng ý với{' '}
          <a
            href='/terms'
            className='underline underline-offset-4 hover:text-primary'
          >
            Điều khoản dịch vụ
          </a>{' '}
          và{' '}
          <a
            href='/privacy'
            className='underline underline-offset-4 hover:text-primary'
          >
            Chính sách bảo mật
          </a>
          của chúng tôi (Zord).
        </p>
      </Card>
    </AuthLayout>
  )
}
