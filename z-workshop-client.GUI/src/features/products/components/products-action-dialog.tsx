import { useEffect, useState } from 'react'
import { z } from 'zod'
import { useForm } from 'react-hook-form'
import { CaretSortIcon } from '@radix-ui/react-icons'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery } from '@tanstack/react-query'
import { productService } from '@/services/products'
import { publisherService } from '@/services/publishers'
import { CheckIcon } from 'lucide-react'
import { cn } from '@/lib/utils'
import { UpdateWithFileMutationProps } from '@/utils/types'
import { toast } from '@/hooks/use-toast'
import { Button } from '@/components/ui/button'
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command'
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Textarea } from '@/components/ui/textarea'
import {
  Publisher,
  publisherListSchema,
} from '@/features/publishers/data/schema'
import { Product } from '../data/schema'

const productSchema = z
  .object({
    productId: z.string().optional(),
    name: z.string().min(1).max(100),
    price: z.coerce.number().min(0, 'Không được nhỏ hơn 0'),
    type: z.union([z.literal('Game'), z.literal('App')]),
    genre: z.string(),
    description: z.string(),
    thumbnail: z
      .any()
      .refine((file) => file instanceof File || file instanceof Blob, {
        message: 'Ảnh sản phẩm không hợp lệ',
      })
      .optional(),
    publisherId: z.string().optional(),
    isEdit: z.boolean(),
  })
  .superRefine((data, ctx) => {
    if (data.isEdit && !data.productId) {
      ctx.addIssue({
        code: 'custom',
        message: 'productId là bắt buộc',
        path: ['productId'],
      })
    }
  })

type ProductForm = z.infer<typeof productSchema>

interface Props {
  currentRow?: Product
  open: boolean
  onOpenChange: (open: boolean) => void
  onSuccess: () => void
}

const typeOption = [
  {
    label: 'Game',
    value: 'Game',
  },
  {
    label: 'App',
    value: 'App',
  },
]

export function ProductsActionDialog({
  currentRow,
  open,
  onOpenChange,
  onSuccess,
}: Props) {
  const { data: rawPublishersData } = useQuery({
    queryKey: ['testPublishers'],
    queryFn: publisherService.getPublishers,
  })

  const [publishers, setPublishers] = useState<Publisher[]>([])

  useEffect(() => {
    if (rawPublishersData) {
      const parsedData = publisherListSchema.safeParse(rawPublishersData.data)
      if (parsedData.success) {
        setPublishers(parsedData.data)
        console.log('Parsed publisher data:', parsedData.data)
      } else {
        console.error('Error parsing publisher data:', parsedData.error)
        // Optionally set an empty array or handle the error in another way
        setPublishers([])
      }
    }
  }, [rawPublishersData])

  const isEdit = !!currentRow
  // console.log('isedit? ', isEdit)
  const form = useForm<ProductForm>({
    resolver: zodResolver(productSchema),
    defaultValues: isEdit
      ? {
          ...currentRow,
          thumbnail: undefined,
          description: JSON.parse(currentRow.desc).Description || '',
          isEdit,
        }
      : {
          name: '',
          type: 'Game',
          thumbnail: undefined,
          isEdit,
        },
  })

  const productCreateMutation = useMutation({
    mutationFn: productService.createProduct,
    onSuccess: (data) => {
      toast({
        variant: 'default',
        title: 'Thêm mới thành công',
        description: data.message,
      })
      onSuccess()
      onOpenChange(false)
      form.reset()
    },
  })

  const productUpdateMutation = useMutation({
    mutationFn: ({ id, data }: UpdateWithFileMutationProps) =>
      productService.updateProduct(id, data),
    onSuccess: (data) => {
      toast({
        variant: 'default',
        title: 'Cập nhật thành công',
        description: data.message,
      })
      onSuccess()
      onOpenChange(false)
    },
  })

  const onSubmit = (data: ProductForm) => {
    const formData = new FormData()
    if (isEdit && data.productId) formData.append('ProductId', data.productId)
    formData.append('Name', data.name)
    formData.append('Type', data.type)
    formData.append('Price', data.price.toString())
    formData.append('Genre', data.genre)
    formData.append('Desc', data.description)
    if (!isEdit && data.publisherId)
      formData.append('PublisherId', data.publisherId)

    if (data.thumbnail) formData.append('Thumbnail', data.thumbnail)
    if (isEdit && data.productId) {
      productUpdateMutation.mutate({ id: data.productId, data: formData })
    } else {
      productCreateMutation.mutate(formData)
    }
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
            {isEdit ? 'Chỉnh sửa sản phẩm' : 'Thêm mới sản phẩm'}
          </DialogTitle>
          <DialogDescription>
            {isEdit ? 'Cập nhật thông tin. ' : 'Thêm mới thông tin. '}
            Ấn xác nhận khi đã hoàn thành.
          </DialogDescription>
        </DialogHeader>
        <div className='-mr-4 h-[30rem] w-full overflow-y-auto py-1 pr-4'>
          <Form {...form}>
            <form
              id='product-form'
              onSubmit={form.handleSubmit(onSubmit)}
              className='space-y-4 p-0.5'
            >
              <FormField
                control={form.control}
                name='productId'
                render={({ field }) => <input type='hidden' {...field} />}
              />
              <FormField
                control={form.control}
                name='name'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Tên Game/App
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder='Assasin Creed'
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
                name='price'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Giá bán
                    </FormLabel>
                    <FormControl>
                      <Input
                        className='col-span-4'
                        autoComplete='off'
                        type='number'
                        step='1'
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className='col-span-4 col-start-3' />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='type'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Loại sản phẩm
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
                          {typeOption.map((role) => (
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
              <FormField
                control={form.control}
                name='genre'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Thể loại
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder='Action, Adventure'
                        className='col-span-4'
                        autoComplete='off'
                        step='1'
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className='col-span-4 col-start-3' />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='thumbnail'
                render={({ field }) => (
                  <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                    <FormLabel className='col-span-2 text-right'>
                      Ảnh sản phẩm
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
              {!isEdit && (
                <FormField
                  control={form.control}
                  name='publisherId'
                  render={({ field }) => (
                    <FormItem className='grid grid-cols-6 items-center gap-x-4 gap-y-1 space-y-0'>
                      <FormLabel className='col-span-2 text-right'>
                        Nhà phát hành
                      </FormLabel>
                      <Popover>
                        <PopoverTrigger asChild>
                          <FormControl>
                            <Button
                              variant='outline'
                              role='combobox'
                              className={cn(
                                'col-span-4 text-start',
                                !field.value && 'text-muted-foreground'
                              )}
                            >
                              {field.value
                                ? publishers.find(
                                    (publisher) =>
                                      publisher.publisherId === field.value
                                  )?.name
                                : 'Chọn nhà phát hành'}
                              <CaretSortIcon className='ml-2 h-4 w-4 shrink-0 opacity-50' />
                            </Button>
                          </FormControl>
                        </PopoverTrigger>
                        <PopoverContent className='p-0'>
                          <Command>
                            <CommandInput placeholder='Tìm...' />
                            <CommandEmpty>
                              Không tìm thấy... <br />
                              <i className='text-muted-foreground'>
                                nếu cần đăng ký cho NPH mới hãy chuyển sang tab
                                quản lý NPH.
                              </i>
                            </CommandEmpty>
                            <CommandGroup>
                              <CommandList>
                                {publishers.map((publisher) => (
                                  <CommandItem
                                    value={publisher.name}
                                    key={publisher.publisherId}
                                    onSelect={() => {
                                      form.setValue(
                                        'publisherId',
                                        publisher.publisherId
                                      )
                                    }}
                                  >
                                    <CheckIcon
                                      className={cn(
                                        'mr-2 h-4 w-4',
                                        publisher.publisherId === field.value
                                          ? 'opacity-100'
                                          : 'opacity-0'
                                      )}
                                    />
                                    {publisher.name}
                                  </CommandItem>
                                ))}
                              </CommandList>
                            </CommandGroup>
                          </Command>
                        </PopoverContent>
                      </Popover>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              )}
              <FormField
                control={form.control}
                name='description'
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Chi tiết về sản phẩm</FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder='Mô tả chi tiết về sản phẩm này'
                        className='resize-none'
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </form>
          </Form>
        </div>
        <DialogFooter>
          <Button type='submit' form='product-form'>
            Xác nhận
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
