import { useEffect, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { productService } from '@/services/products'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { columns } from './components/products-columns'
import { ProductsDialogs } from './components/products-dialogs'
import { ProductsPrimaryButtons } from './components/products-primary-buttons'
import { ProductsTable } from './components/products-table'
import ProductsProvider from './context/products-context'
import { Product, productListSchema } from './data/schema'

export default function Products() {
  const {
    data: rawProductsData,
    refetch,
    isLoading,
  } = useQuery({
    queryKey: ['testProducts'],
    queryFn: productService.getProducts,
  })

  const [products, setProducts] = useState<Product[]>([])

  useEffect(() => {
    if (rawProductsData) {
      const parsedData = productListSchema.safeParse(rawProductsData.data)
      if (parsedData.success) {
        setProducts(parsedData.data)
        console.log('Parsed product data:', parsedData.data)
      } else {
        console.error('Error parsing product data:', parsedData.error)
        // Optionally set an empty array or handle the error in another way
        setProducts([])
      }
    }
  }, [rawProductsData])

  return (
    <ProductsProvider>
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
              Quản lý App/Game
            </h2>
            <p className='text-muted-foreground'>
              Thông tin của App/Game được hiển thị ở bảng dưới.
            </p>
          </div>
          <ProductsPrimaryButtons />
        </div>
        <div className='-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-x-12 lg:space-y-0'>
          <ProductsTable
            data={products}
            columns={columns}
            isLoading={isLoading}
          />
        </div>
      </Main>

      <ProductsDialogs onSuccess={refetch} />
    </ProductsProvider>
  )
}
