import { format } from 'date-fns'
import { ColumnDef } from '@tanstack/react-table'
import { cn } from '@/lib/utils'
import { Badge } from '@/components/ui/badge'
import { Checkbox } from '@/components/ui/checkbox'
import { callTypes } from '../data/data'
import { Employee } from '../data/schema'
import { DataTableColumnHeader } from './data-table-column-header'
import { DataTableRowActions } from './data-table-row-actions'

export const columns: ColumnDef<Employee>[] = [
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
    id: 'fullname',
    accessorFn: (row) => row.employeeDto.fullname,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Họ tên' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('fullname')}</span>
    },
    enableSorting: true,
    enableHiding: false,
    enableGlobalFilter: true,
  },
  {
    id: 'hiredDate',
    accessorFn: (row) => row.employeeDto.hiredDate,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Ngày vào làm' />
    ),
    cell: ({ row }) => {
      var hiredDate: Date = row.getValue('hiredDate')
      return <span>{format(new Date(hiredDate), 'dd/MM/yyyy')}</span>
    },
    enableSorting: true,
    enableHiding: true,
    enableGlobalFilter: true,
    
  },
  {
    id: 'dob',
    accessorFn: (row) => row.employeeDto.dob,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Ngày sinh' />
    ),
    cell: ({ row }) => {
      var dob: Date = row.getValue('dob')
      return <span>{format(new Date(dob), 'dd/MM/yyyy')}</span>
    },
    enableSorting: true,
    enableHiding: true,
    enableGlobalFilter: true,
  },
  {
    id: 'address',
    accessorFn: (row) => row.employeeDto.address,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Địa chỉ' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('address')}</span>
    },
    enableSorting: false,
    enableHiding: true,
    enableGlobalFilter: true,
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
    enableGlobalFilter: true,
  },
  {
    id: 'phone',
    accessorFn: (row) => row.employeeDto.phone,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Số điện thoại' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('phone')}</span>
    },
    enableSorting: false,
    enableHiding: true,
    enableGlobalFilter: true,
  },
  {
    id: 'email',
    accessorFn: (row) => row.employeeDto.email,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Email' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('email')}</span>
    },
    enableSorting: false,
    enableHiding: true,
    enableGlobalFilter: true,
  },
  {
    id: 'role',
    accessorFn: (row) => row.userDto.role,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Vai trò' />
    ),
    cell: ({ row }) => {
      const cssClass =
        row.getValue('role') == 'SuperAdmin' ? 'bg-red-500' : 'bg-blue-500'
      return (
        <div className='flex space-x-2'>
          <Badge variant='outline' className={cn('capitalize', cssClass)}>
            {row.getValue('role')}
          </Badge>
        </div>
      )
    },
  },
  {
    id: 'status',
    accessorFn: (row) => row.employeeDto.status,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Trạng thái' />
    ),
    cell: ({ row }) => {
      var status = row.original.employeeDto.status
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
    enableGlobalFilter: false,
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
