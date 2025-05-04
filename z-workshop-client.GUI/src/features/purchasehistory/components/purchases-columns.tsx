import { format } from 'date-fns'
import { ColumnDef } from '@tanstack/react-table'
import { Purchase } from '../data/schema'
import { DataTableColumnHeader } from './data-table-column-header'

export const columns: ColumnDef<Purchase>[] = [
  {
    id: 'purchaseId',
    accessorFn: (row) => row.purchaseId,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='ID giao dịch' />
    ),
    cell: ({ row }) => {
      return <div className='flex space-x-2'>{row.getValue('purchaseId')}</div>
    },
    enableSorting: true,
    enableHiding: true,
    enableGlobalFilter: false,
  },
  {
    id: 'productName',
    accessorFn: (row) => row.productName,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Tên App/Game' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('productName')}</span>
    },
    enableSorting: true,
    enableHiding: false,
    enableGlobalFilter: true,
  },
  {
    id: 'unitPrice',
    accessorFn: (row) => row.unitPrice,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Giá trị mua' />
    ),
    cell: ({ row }) => {
      return (
        <span>
          {new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
            currencyDisplay: 'code',
          }).format(row.getValue('unitPrice'))}
        </span>
      )
    },
    enableSorting: true,
    enableHiding: false,
    enableGlobalFilter: false,
  },
  {
    id: 'buyer',
    accessorFn: (row) => row.customerFullname,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Người mua' />
    ),
    cell: ({ row }) => {
      return <span>{row.getValue('buyer')}</span>
    },
  },
  {
    id: 'closeAt',
    accessorFn: (row) => row.closeAt,
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title='Ngày mua' />
    ),
    cell: ({ row }) => {
      var createAt: Date = row.getValue('closeAt')
      return <span>{format(new Date(createAt), 'dd/MM/yyyy')}</span>
    },
    enableSorting: true,
    enableHiding: true,
    enableGlobalFilter: true,
  },
]
