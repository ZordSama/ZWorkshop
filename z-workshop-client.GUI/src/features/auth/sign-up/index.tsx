import { Link } from '@tanstack/react-router'
import { Card } from '@/components/ui/card'
import AuthLayout from '../auth-layout'
import { SignUpForm } from './components/sign-up-form'

export default function SignUp() {
  return (
    <AuthLayout>
      <Card className='p-6 w-[400px] md:w-[500px] mx-auto'>
        <div className='mb-2 flex flex-col space-y-2 text-left'>
          <h1 className='text-lg font-semibold tracking-tight'>
            Tạo tài khoản mới
          </h1>
          <p className='text-sm text-muted-foreground'>
            Nhập đủ thông tin để tạo tài khoản trong hệ thống<br />
            Bạn đã có tài khoản? hãy {' '}
            <Link
              to='/sign-in'
              className='underline underline-offset-4 hover:text-primary'
            >
              Đăng nhập
            </Link>
          </p>
        </div>
        <SignUpForm />
        <p className='mt-4 px-6 text-center text-sm text-muted-foreground'>
          Bằng cách đăng ký, bạn đồng ý với{' '}
          <a
            href='/terms'
            className='underline underline-offset-4 hover:text-primary'
          >
            Điều khoản sử dụng
          </a>{' '}
          và{' '}
          <a
            href='/privacy'
            className='underline underline-offset-4 hover:text-primary'
          >
            Chính sách bảo mật
          </a>{' '}
          của chúng tôi.
        </p>
      </Card>
    </AuthLayout>
  )
}
