import { format } from 'date-fns'
import { ColumnDef } from '@tanstack/react-table'
import { cn } from '@/lib/utils'
import { Badge } from '@/components/ui/badge'
import { Checkbox } from '@/components/ui/checkbox'
import { callTypes } from '../data/data'
import { Customer } from '../data/schema'
import { DataTableColumnHeader } from './data-table-column-header'

export const columns: ColumnDef<Customer>[] = [
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
  },
  {
    id: 'fullName',
    accessorFn: (row) => row.customerDto.fullName,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Họ tên' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('fullName')}</span>
    },
    enableSorting: true,
    enableHiding: false,
  },
  {
    id: 'dob',
    accessorFn: (row) => row.customerDto.dob,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Ngày sinh' />
    ),
    cell: ({ row }) => {
      var dob: Date = row.getValue('dob')
      return <span>{format(new Date(dob), 'dd/mm/yyyy')}</span>
    },
    enableSorting: true,
    enableHiding: true,
  },
  {
    id: 'address',
    accessorFn: (row) => row.customerDto.address,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Địa chỉ' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('address')}</span>
    },
    enableSorting: false,
    enableHiding: true,
  },
  {
    id: 'username',
    accessorFn: (row) => row.userDto.username,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Tên đăng nhập' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('username')}</span>
    },
    enableSorting: false,
    enableHiding: false,
  },
  {
    id: 'phone',
    accessorFn: (row) => row.customerDto.phone,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Số điện thoại' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('phone')}</span>
    },
    enableSorting: false,
    enableHiding: true,
  },
  {
    id: 'email',
    accessorFn: (row) => row.customerDto.email,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Email' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('email')}</span>
    },
    enableSorting: false,
    enableHiding: true,
  },
  {
    id: 'status',
    accessorFn: (row) => row.customerDto.status,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Trạng thái' />
    ),
    cell: ({ row }) => {
      var status = row.original.customerDto.status
      const badgeColor = callTypes.get(status)
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
  },
  {
    id: 'actions'
  }
]
