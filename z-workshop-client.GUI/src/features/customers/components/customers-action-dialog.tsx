import { z } from 'zod'
import { format } from 'date-fns'
import { useForm } from 'react-hook-form'
import { CalendarIcon } from '@radix-ui/react-icons'
import { zodResolver } from '@hookform/resolvers/zod'
import { cn } from '@/lib/utils'
import { Button } from '@/components/ui/button'
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
import { Popover, PopoverTrigger } from '@/components/ui/popover'
import { Customer } from '../data/schema'

const formSchema = z
  .object({
    customerId: z.string().optional(),
    fullName: z.string().min(1).max(100),
    dob: z.date(),
    address: z.string().min(1).max(100),
    email: z.string().email(),
    phone: z.string().length(10),
    status: z
      .union([z.literal('active'), z.literal('warning'), z.literal('banned')])
      .optional(),
    isEdit: z.boolean(),
  })
  .superRefine(({ isEdit, customerId }, ctx) => {
    if (isEdit && !customerId) {
      ctx.addIssue({
        code: 'custom',
        message: 'Mã khách hàng không tồn tại!',
        path: ['customerId'],
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
  const form = useForm<CustomerForm>({
    resolver: zodResolver(formSchema),
    defaultValues: isEdit
      ? {
          ...currentRow.customerDto,
          isEdit,
        }
      : {
          fullName: '',
          dob: new Date(),
          address: '',
          email: '',
          phone: '',
          isEdit,
        },
  })

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
        <div className='-mr-4 h-[26.25rem] w-full overflow-y-auto py-1 pr-4'>
          <Form {...form}>
            <form id='customer-form' className='space-y-4 p-0.5'>
              <FormField
                control={form.control}
                name='customerId'
                render={({ field }) => <input type='hidden' {...field} />}
              />
              <FormField
                control={form.control}
                name='fullName'
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
                name='dob'
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
                              format(field.value, 'MMM d, yyyy')
                            ) : (
                              <span>Pick a date</span>
                            )}
                            <CalendarIcon className='ml-auto h-4 w-4 opacity-50' />
                          </Button>
                        </FormControl>
                      </PopoverTrigger>
                    </Popover>
                    <FormMessage className='col-span-4 col-start-3' />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='address'
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
                name='email'
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
                name='phone'
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
      </DialogContent>
      <DialogFooter>
        <Button type='submit' form='customer-form'>
          Xác nhận
        </Button>
      </DialogFooter>
    </Dialog>
  )
}
