import { useEffect, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { publisherService } from '@/services/publishers'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { columns } from './components/publishers-columns'
import { PublishersDialogs } from './components/publishers-dialogs'
import { PublishersPrimaryButtons } from './components/publishers-primary-buttons'
import { PublishersTable } from './components/publishers-table'
import PublishersProvider from './context/publishers-context'
import { Publisher, publisherListSchema } from './data/schema'

export default function Publishers() {
  const {
    data: rawPublishersData,
    refetch,
    isLoading,
  } = useQuery({
    queryKey: ['testPublishers'],
    queryFn: publisherService.getPublishers,
    // refetchOnWindowFocus: false,
    // refetchOnMount: false,
    // staleTime: Infinity,
    // enabled: true,
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

  return (
    <PublishersProvider>
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
          <PublishersPrimaryButtons />
        </div>
        <div className='-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-x-12 lg:space-y-0'>
          <PublishersTable
            data={publishers}
            columns={columns}
            isLoading={isLoading}
          />
        </div>
      </Main>

      <PublishersDialogs onSuccess={refetch} />
    </PublishersProvider>
  )
}
