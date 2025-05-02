import { z } from 'zod'
import { format } from 'date-fns'
import { useForm } from 'react-hook-form'
import { CalendarIcon } from '@radix-ui/react-icons'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation } from '@tanstack/react-query'
import { customersService } from '@/services/customers'
import { vi } from 'date-fns/locale'
import { cn } from '@/lib/utils'
import { toast } from '@/hooks/use-toast'
import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
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
import { Separator } from '@/components/ui/separator'
import { Customer } from '../data/schema'

const customerSchema = z.object({
  customerId: z.string().optional(),
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
    isEdit: z.boolean(),
  })
  .superRefine(({ isEdit, customerFormData, userFormData }, ctx) => {
    if (isEdit && !customerFormData.customerId) {
      ctx.addIssue({
        code: 'custom',
        message: 'Mã khách hàng không tồn tại!',
        path: ['customerFormData.customerId'],
      })
    }
    if (!isEdit && !userFormData?.username) {
      ctx.addIssue({
        code: 'custom',
        message: 'Tên đăng nhập không được để trống!',
        path: ['userFormData.username'],
      })
    }
    if (!isEdit && !userFormData?.password) {
      ctx.addIssue({
        code: 'custom',
        message: 'Mật khẩu không được để trống!',
        path: ['userFormData.password'],
      })
    }
    if (!isEdit && !userFormData?.confirmPassword) {
      ctx.addIssue({
        code: 'custom',
        message: 'Xác nhận mật khẩu không được để trống!',
      })
    }
    if (!isEdit && userFormData?.password !== userFormData?.confirmPassword) {
      ctx.addIssue({
        code: 'custom',
        message: 'Mật khẩu không khớp!',
        path: ['userFormData.confirmPassword'],
      })
    }
  })
type CustomerForm = z.infer<typeof formSchema>

interface Props {
  currentRow?: Customer
  open: boolean
  onOpenChange: (open: boolean) => void
  onSuccess: () => void
}

export function CustomersActionDialog({
  currentRow,
  open,
  onOpenChange,
  onSuccess,
}: Props) {
  const isEdit = !!currentRow
  // console.log('isedit? ', isEdit)
  const form = useForm<CustomerForm>({
    resolver: zodResolver(formSchema),
    defaultValues: isEdit
      ? {
          customerFormData: {
            ...currentRow.customerDto,
          },
          isEdit,
        }
      : {
          customerFormData: {
            fullname: '',
            address: '',
            email: '',
            phone: '',
          },
          isEdit,
        },
  })

  const customerMutation = useMutation({
    mutationFn: isEdit
      ? customersService.updateCustomer
      : customersService.createNewCustomer,
    onSuccess: (data) => {
      toast({
        variant: 'default',
        title: isEdit ? 'Cập nhật thành công' : 'Thêm mới thành công',
        description: data.message,
      })
      onSuccess()
      onOpenChange(false)
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
    isEdit
      ? customerMutation.mutate(transformedData.customerFormData)
      : customerMutation.mutate(transformedData)
  }

  //   const
  return (
    <Dialog
      open={open}
      onOpenChange={(state) => {
        form.reset()
        onOpenChange(state)
      }}
    >
      <DialogContent className='sm:max-w-lg'>
        <DialogHeader className='text-left'>
          <DialogTitle>
            {isEdit ? 'Chỉnh sửa người dùng' : 'Thêm người dùng'}
          </DialogTitle>
          <DialogDescription>
            {isEdit ? 'Cập nhật thông tin. ' : 'Thêm mới thông tin. '}
            Ấn xác nhận khi đã hoàn thành.
          </DialogDescription>
        </DialogHeader>
        <div className='-mr-4 h-[30rem] w-full overflow-y-auto py-1 pr-4'>
          <Form {...form}>
            <form
              id='customer-form'
              onSubmit={form.handleSubmit(onSubmit)}
              className='space-y-4 p-0.5'
            >
              {!isEdit && (
                <>
                  <span>Thông tin đăng nhập</span>
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
                            placeholder='username'
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
                    name='userFormData.password'
                    render={({ field }) => (
                      <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                        <FormLabel className='col-span-2 text-right'>
                          Mật khẩu
                        </FormLabel>
                        <FormControl>
                          <Input
                            placeholder='password'
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
                    name='userFormData.confirmPassword'
                    render={({ field }) => (
                      <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                        <FormLabel className='col-span-2 text-right'>
                          Xác nhận mật khẩu
                        </FormLabel>
                        <FormControl>
                          <Input
                            placeholder='Confirm password'
                            className='col-span-4'
                            autoComplete='off'
                            {...field}
                          />
                        </FormControl>
                        <FormMessage className='col-span-4 col-start-3' />
                      </FormItem>
                    )}
                  />
                  <Separator />
                </>
              )}

              <span>Thông tin cá nhân</span>
              <FormField
                control={form.control}
                name='customerFormData.customerId'
                render={({ field }) => <input type='hidden' {...field} />}
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
                              'w-[240px] pl-3 text-left font-normal',
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
                    <FormLabel className='col-span-2 text-right'>
                      Email
                    </FormLabel>
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
            </form>
          </Form>
        </div>
        <DialogFooter>
          <Button type='submit' form='customer-form'>
            Xác nhận
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
