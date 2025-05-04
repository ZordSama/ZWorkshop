import { HTMLAttributes, useState } from 'react'
import { z } from 'zod'
import { format } from 'date-fns'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation } from '@tanstack/react-query'
import { useNavigate } from '@tanstack/react-router'
import { customersService } from '@/services/customers'
import { vi } from 'date-fns/locale'
import { CalendarIcon } from 'lucide-react'
import { cn } from '@/lib/utils'
import { toast } from '@/hooks/use-toast'
import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'
import { PasswordInput } from '@/components/password-input'

type SignUpFormProps = HTMLAttributes<HTMLDivElement>

const customerSchema = z.object({
  fullname: z.string().min(1).max(100),
  dob: z.date(),
  address: z.string().min(1).max(100),
  email: z.string().email(),
  phone: z.string().length(10),
  status: z
    .union([z.literal('active'), z.literal('warning'), z.literal('banned')])
    .optional(),
})

const userSchema = z.object({
  username: z.string().optional(),
  password: z.string().optional(),
  confirmPassword: z.string().optional(),
})

const formSchema = z
  .object({
    customerFormData: customerSchema,
    userFormData: userSchema.optional(),
  })
  .superRefine(({ userFormData }, ctx) => {
    if (!userFormData?.username) {
      ctx.addIssue({
        code: 'custom',
        message: 'Tên đăng nhập không được để trống!',
        path: ['userFormData.username'],
      })
    }
    if (!userFormData?.password) {
      ctx.addIssue({
        code: 'custom',
        message: 'Mật khẩu không được để trống!',
        path: ['userFormData.password'],
      })
    }
    if (!userFormData?.confirmPassword) {
      ctx.addIssue({
        code: 'custom',
        message: 'Xác nhận mật khẩu không được để trống!',
      })
    }
    if (userFormData?.password !== userFormData?.confirmPassword) {
      ctx.addIssue({
        code: 'custom',
        message: 'Mật khẩu không khớp!',
        path: ['userFormData.confirmPassword'],
      })
    }
  })
type CustomerForm = z.infer<typeof formSchema>
export function SignUpForm({ className, ...props }: SignUpFormProps) {
  const [isLoading, setIsLoading] = useState(false)
  const navigate = useNavigate()
  const customerMutation = useMutation({
    mutationFn: customersService.createNewCustomer,
    onSuccess: (data) => {
      toast({
        variant: 'default',
        title: 'Đăng ký thành công',
        description: data.message,
      })
      navigate({
        to: '/sign-in',
      })
    },
    onSettled: () => setIsLoading(false),
  })

  const form = useForm<CustomerForm>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      customerFormData: {
        fullname: '',
        address: '',
        email: '',
        phone: '',
      },
    },
  })

  const onSubmit = (data: CustomerForm) => {
    const { customerFormData, userFormData } = data
    const transformedData = {
      customerFormData: {
        ...customerFormData,
        dob: customerFormData.dob.toISOString().split('T')[0], // "YYYY-MM-DD"
      },
      userFormData,
    }
    console.log('data: ', transformedData)
    customerMutation.mutate(transformedData)
  }

  return (
    <div className={cn('grid gap-6', className)} {...props}>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <div className='grid gap-2'>
            <FormField
              control={form.control}
              name='userFormData.username'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Tên đăng nhập
                  </FormLabel>
                  <FormControl>
                    <Input
                      className='col-span-4'
                      placeholder='Username'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='userFormData.password'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Mật khẩu
                  </FormLabel>
                  <FormControl>
                    <PasswordInput
                      className='col-span-4'
                      placeholder='********'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='userFormData.confirmPassword'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Xác nhận mật khẩu
                  </FormLabel>
                  <FormControl>
                    <PasswordInput
                      className='col-span-4'
                      placeholder='********'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='customerFormData.fullname'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Họ tên
                  </FormLabel>
                  <FormControl>
                    <Input
                      placeholder='Nguyễn Văn A'
                      className='col-span-4'
                      autoComplete='off'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className='col-span-4 col-start-3' />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='customerFormData.dob'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Ngày sinh
                  </FormLabel>
                  <Popover>
                    <PopoverTrigger asChild>
                      <FormControl>
                        <Button
                          variant={'outline'}
                          className={cn(
                            'col-span-4 pl-3 text-left font-normal',
                            !field.value && 'text-muted-foreground'
                          )}
                        >
                          {field.value ? (
                            format(field.value, 'MM/dd/yyyy')
                          ) : (
                            <span>Chọn ngày sinh</span>
                          )}
                          <CalendarIcon className='ml-auto h-4 w-4 opacity-50' />
                        </Button>
                      </FormControl>
                    </PopoverTrigger>
                    <PopoverContent className='w-auto p-0' align='start'>
                      <Calendar
                        mode='single'
                        locale={vi}
                        selected={field.value}
                        onSelect={field.onChange}
                        disabled={(date: Date) =>
                          date > new Date() || date < new Date('1900-01-01')
                        }
                      />
                    </PopoverContent>
                  </Popover>
                  <FormMessage className='col-span-4 col-start-3' />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='customerFormData.address'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Địa chỉ
                  </FormLabel>
                  <FormControl>
                    <Input
                      placeholder='Số 1, Đường 2, Phường 3, Quận 4, Thành phố Hồ Chí Minh'
                      className='col-span-4'
                      autoComplete='off'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className='col-span-4 col-start-3' />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='customerFormData.email'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>Email</FormLabel>
                  <FormControl>
                    <Input
                      placeholder='example@gmail.com'
                      className='col-span-4'
                      autoComplete='off'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className='col-span-4 col-start-3' />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='customerFormData.phone'
              render={({ field }) => (
                <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                  <FormLabel className='col-span-2 text-right'>
                    Số điện thoại
                  </FormLabel>
                  <FormControl>
                    <Input
                      placeholder='0123456789'
                      className='col-span-4'
                      autoComplete='off'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className='col-span-4 col-start-3' />
                </FormItem>
              )}
            />
            <Button className='mt-2' disabled={isLoading}>
              Đăng ký
            </Button>
          </div>
        </form>
      </Form>
    </div>
  )
}
