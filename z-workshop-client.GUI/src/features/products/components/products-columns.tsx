import { format } from 'date-fns'
import { ColumnDef } from '@tanstack/react-table'
import { cn } from '@/lib/utils'
import { Badge } from '@/components/ui/badge'
import { Checkbox } from '@/components/ui/checkbox'
import { callTypes } from '../data/data'
import { Product } from '../data/schema'
import { DataTableColumnHeader } from './data-table-column-header'
import { DataTableRowActions } from './data-table-row-actions'

export const columns: ColumnDef<Product>[] = [
  {
    id: 'select',
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && 'indeterminate')
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label='Select all'
        className='translate-y-[2px]'
      />
    ),
    meta: {
      className: cn(
        'sticky md:table-cell left-0 z-10 rounded-tl',
        'bg-background transition-colors duration-200 group-hover/row:bg-muted group-data-[state=selected]/row:bg-muted'
      ),
    },
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label='Select row'
        className='translate-y-[2px]'
      />
    ),
    enableSorting: false,
    enableHiding: false,
    enableGlobalFilter: false,
  },
  {
    id: 'name',
    accessorFn: (row) => row.name,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Tên sản phẩm' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('name')}</span>
    },
    enableSorting: true,
    enableHiding: false,
    enableGlobalFilter: true,
  },
  {
    id: 'Price',
    accessorFn: (row) => row.price,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Giá sản phẩm' />
    ),
    cell: ({ row }) => {
      var price = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
        currencyDisplay: 'code',
      }).format(row.getValue('Price'))
      return <span>{price}</span>
    },
    enableSorting: false,
    enableHiding: true,
    enableGlobalFilter: true,
  },
  {
    id: 'status',
    accessorFn: (row) => row.type,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Trạng thái' />
    ),
    cell: ({ row }) => {
      var type = row.original.type
      const badgeColor = callTypes.get(type)
      return (
        <div className='flex space-x-2'>
          <Badge variant='outline' className={cn('capitalize', badgeColor)}>
            {row.getValue('status')}
          </Badge>
        </div>
      )
    },
    enableSorting: true,
    enableHiding: true,
    enableGlobalFilter: false,
  },
  {
    id: 'createdDate',
    accessorFn: (row) => row.createdAt,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Ngày cấp phép' />
    ),
    cell: ({ row }) => {
      var createAt: Date = row.getValue('createdDate')
      return <span>{format(new Date(createAt), 'dd/MM/yyyy')}</span>
    },
    enableSorting: true,
    enableHiding: true,
    enableGlobalFilter: true,
  },
  {
    id: 'Publisher',
    accessorFn: (row) => row.publisherName,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Nhà phát hành' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('Publisher')}</span>
    },
  },
  {
    id: 'actions',
    // header: ({ column }) =>
    //   <DataTableColumnHeader column={column} title='Hành động' />,
    cell: DataTableRowActions,
    enableHiding: false,
    enableSorting: false,
    enableGlobalFilter: false,
  },
]
