import { useEffect, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { shopService } from '@/services/shop'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { Purchase, purchaseListSchema } from './data/schema'
import PurchasesProvider from './context/purchases-context'
import { PurchasesTable } from './components/purchases-table'
import { columns } from './components/purchases-columns'

export default function Purchases() {
  const {
    data: rawPurchasesData,
    isLoading,
  } = useQuery({
    queryKey: ['testPurchases'],
    queryFn: shopService.getCustomerPurchases,
  })

  const [purchases, setPurchases] = useState<Purchase[]>([])

  useEffect(() => {
    if (rawPurchasesData) {
      const parsedData = purchaseListSchema.safeParse(rawPurchasesData)
      if (parsedData.success) {
        setPurchases(parsedData.data)
        console.log('Parsed purchase data:', parsedData.data)
      } else {
        console.error('Error parsing purchase data:', parsedData.error)
        setPurchases([])
      }
    }
  }, [rawPurchasesData])

  return (
    <PurchasesProvider>
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
              Quản lý nhà phát hành
            </h2>
            <p className='text-muted-foreground'>
              Thông tin của nhà phát hành được hiển thị ở bảng dưới.
            </p>
          </div>
        </div>
        <div className='-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-x-12 lg:space-y-0'>
          <PurchasesTable
            data={purchases}
            columns={columns}
            isLoading={isLoading}
          />
        </div>
      </Main>
    </PurchasesProvider>
  )
}
