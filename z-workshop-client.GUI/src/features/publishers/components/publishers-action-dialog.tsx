import { z } from 'zod'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation } from '@tanstack/react-query'
import { publisherService } from '@/services/publishers'
import { cn } from '@/lib/utils'
import { toast } from '@/hooks/use-toast'
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Publisher } from '../data/schema'

const publisherSchema = z
  .object({
    publisherId: z.string().optional(),
    name: z.string().min(1).max(100),
    email: z.string().email(),
    status: z
      .union([z.literal('active'), z.literal('warning'), z.literal('banned')])
      .optional(),
    fileAvt: z
      .any()
      .refine((file) => file instanceof File || file instanceof Blob, {
        message: 'Logo is required',
      })
      .optional(),
    isEdit: z.boolean(),
  })
  .superRefine((data, ctx) => {
    if (data.isEdit && !data.publisherId) {
      ctx.addIssue({
        code: 'custom',
        message: 'publisherId is required',
        path: ['publisherId'],
      })
    }
  })

type PublisherForm = z.infer<typeof publisherSchema>

interface Props {
  currentRow?: Publisher
  open: boolean
  onOpenChange: (open: boolean) => void
  onSuccess: () => void
}

const statusOptions = [
  {
    label: 'Active',
    value: 'active',
  },
  {
    label: 'Warning',
    value: 'warning',
  },
  {
    label: 'Banned',
    value: 'banned',
  },
]

export function PublishersActionDialog({
  currentRow,
  open,
  onOpenChange,
  onSuccess,
}: Props) {
  const isEdit = !!currentRow
  // console.log('isedit? ', isEdit)
  const form = useForm<PublisherForm>({
    resolver: zodResolver(publisherSchema),
    defaultValues: isEdit
      ? {
          ...currentRow,
          fileAvt: undefined,
          isEdit,
        }
      : {
          name: '',
          email: '',
          status: 'active',
          fileAvt: undefined,
          isEdit,
        },
  })

  const publisherMutation = useMutation({
    mutationFn: isEdit
      ? publisherService.updatePublisher
      : publisherService.createPublisher,
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

  const onSubmit = (data: PublisherForm) => {
    // publisherMutation.mutate(data)
    const formData = new FormData()
    if (isEdit && data.publisherId)
      formData.append('PublisherId', data.publisherId)
    formData.append('Name', data.name)
    formData.append('Email', data.email)
    switch (data.status) {
      case 'banned':
        formData.append('Status', '-2')
        break
      case 'warning':
        formData.append('Status', '-1')
        break
      default:
        formData.append('Status', '0')
        break
    }

    if (data.fileAvt) formData.append('FileAvt', data.fileAvt)

    publisherMutation.mutate(formData)
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
            {isEdit ? 'Chỉnh sửa nhà phát hành' : 'Thêm mới nhà phát hành'}
          </DialogTitle>
          <DialogDescription>
            {isEdit ? 'Cập nhật thông tin. ' : 'Thêm mới thông tin. '}
            Ấn xác nhận khi đã hoàn thành.
          </DialogDescription>
        </DialogHeader>
        <div className='-mr-4 h-[30rem] w-full overflow-y-auto py-1 pr-4'>
          <Form {...form}>
            <form
              id='publisher-form'
              onSubmit={form.handleSubmit(onSubmit)}
              className='space-y-4 p-0.5'
            >
              <FormField
                control={form.control}
                name='publisherId'
                render={({ field }) => <input type='hidden' {...field} />}
              />
              <FormField
                control={form.control}
                name='name'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Tên NPH
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
              {!isEdit && (
                <FormField
                  control={form.control}
                  name='status'
                  render={({ field }) => (
                    <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                      <FormLabel className='col-span-2 text-right'>
                        Trạng thái
                      </FormLabel>
                      <FormControl>
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <SelectTrigger
                            className={cn(
                              'col-span-4 pl-3 text-left font-normal',
                              !field.value && 'text-muted-foreground'
                            )}
                          >
                            <SelectValue placeholder='Chọn vai trò' />
                          </SelectTrigger>
                          <SelectContent>
                            {statusOptions.map((role) => (
                              <SelectItem key={role.value} value={role.value}>
                                {role.label}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      </FormControl>
                      <FormMessage className='col-span-4 col-start-3' />
                    </FormItem>
                  )}
                />
              )}

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
                name='fileAvt'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Ảnh logo NPH
                    </FormLabel>
                    <FormControl>
                      <Input
                        type='file'
                        accept='image/*'
                        className='col-span-4'
                        onChange={(e) => field.onChange(e.target.files?.[0])}
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
          <Button type='submit' form='publisher-form'>
            Xác nhận
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
