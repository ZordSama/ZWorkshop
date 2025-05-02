import { useEffect, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { employeesService } from '@/services/employees'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import EmployeesProvider from './context/employees-context'
import { Employee, employeeListSchema } from './data/schema'
import { EmployeesPrimaryButtons } from './components/employees-primary-buttons'
import { EmployeesTable } from './components/employees-table'
import { EmployeesDialogs } from './components/employees-dialogs'
import { columns } from './components/employees-columns'

export default function Employees() {
  const {
    data: rawEmployeesData,
    refetch,
    isLoading,
  } = useQuery({
    queryKey: ['testEmployees'],
    queryFn: employeesService.getAll,
    // refetchOnWindowFocus: false,
    // refetchOnMount: false,
    // staleTime: Infinity,
    // enabled: true,
  })

  const [employees, setEmployees] = useState<Employee[]>([])

  useEffect(() => {
    if (rawEmployeesData) {
      const parsedData = employeeListSchema.safeParse(rawEmployeesData.data)
      if (parsedData.success) {
        setEmployees(parsedData.data)
      } else {
        console.error('Error parsing employee data:', parsedData.error)
        // Optionally set an empty array or handle the error in another way
        setEmployees([])
      }
    }
  }, [rawEmployeesData])

  return (
    <EmployeesProvider>
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
            <h2 className='text-2xl font-bold tracking-tight'>
              Quản lý nhân viên
            </h2>
            <p className='text-muted-foreground'>
              Thông tin của nhân viên được hiển thị ở bảng dưới.
            </p>
          </div>
          <EmployeesPrimaryButtons />
        </div>
        <div className='-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-x-12 lg:space-y-0'>
          <EmployeesTable
            data={employees}
            columns={columns}
            isLoading={isLoading}
          />
        </div>
      </Main>

      <EmployeesDialogs onSuccess={refetch} />
    </EmployeesProvider>
  )
}
