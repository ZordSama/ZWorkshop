import { useEffect, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { customersService } from '@/services/customers'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { columns } from './components/customers-columns'
import { CustomersDialogs } from './components/customers-dialogs'
import { CustomersPrimaryButtons } from './components/customers-primary-buttons'
import { CustomersTable } from './components/customers-table'
import CustomersProvider from './context/customers-context'
import { Customer, customerListSchema } from './data/schema'

export default function Customers() {
  const {
    data: rawCustomersData,
    refetch,
    isLoading,
  } = useQuery({
    queryKey: ['testCustomers'],
    queryFn: customersService.getAllWithUser,
    // refetchOnWindowFocus: false,
    // refetchOnMount: false,
    // staleTime: Infinity,
    // enabled: true,
  })

  const [customers, setCustomers] = useState<Customer[]>([])

  useEffect(() => {
    if (rawCustomersData) {
      const parsedData = customerListSchema.safeParse(rawCustomersData.data)
      if (parsedData.success) {
        setCustomers(parsedData.data)
      } else {
        console.error('Error parsing customer data:', parsedData.error)
        // Optionally set an empty array or handle the error in another way
        setCustomers([])
      }
    }
  }, [rawCustomersData])

  return (
    <CustomersProvider>
      <Header fixed>
        <Search />
        <div className='ml-auto flex items-center space-x-4'>
          <ThemeSwitch />
          <ProfileDropdown />
        </div>
      </Header>

      <Main>
        <div className='mb-2 flex flex-wrap items-center justify-between space-y-2'>
          <div>
            <h2 className='text-2xl font-bold tracking-tight'>Quản lý khách hàng</h2>
            <p className='text-muted-foreground'>
              Thông tin của khách hàng được hiển thị ở bảng dưới.
            </p>
          </div>
          <CustomersPrimaryButtons />
        </div>
        <div className='-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-x-12 lg:space-y-0'>
          <CustomersTable
            data={customers}
            columns={columns}
            isLoading={isLoading}
          />
        </div>
      </Main>

      <CustomersDialogs onSuccess={refetch} />
    </CustomersProvider>
  )
}
