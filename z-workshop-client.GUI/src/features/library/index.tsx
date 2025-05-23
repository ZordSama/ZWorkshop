import { useEffect, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import {
  IconAdjustmentsHorizontal,
  IconSortAscendingLetters,
  IconSortDescendingLetters,
} from '@tabler/icons-react'
import { shopService } from '@/services/shop'
import { SERVER_PUBLIC_URL } from '@/utils'
import { Play } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { Product, productListSchema } from '../products/data/schema'

const appText = new Map<string, string>([
  ['all', 'All Apps'],
  ['Game', 'Game'],
  ['App', 'App'],
])

export default function Library() {
  const { data: rawProductsData, isLoading } = useQuery({
    queryKey: ['getCustomerLib'],
    queryFn: shopService.getCustomerLibrary,
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

  const [sort, setSort] = useState('ascending')
  const [appType, setAppType] = useState('all')
  const [searchTerm, setSearchTerm] = useState('')
  const filteredApps = products
    .sort((a, b) =>
      sort === 'ascending'
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name)
    )
    .filter((app) =>
      appType === 'Game'
        ? app.type === 'Game'
        : appType === 'App'
          ? app.type === 'App'
          : true
    )
    .filter((app) => app.name.toLowerCase().includes(searchTerm.toLowerCase()))
  return (
    <>
      {/* ===== Top Heading ===== */}
      <Header>
        <Search />
        <div className='ml-auto flex items-center gap-4'>
          <ThemeSwitch />
          <ProfileDropdown />
        </div>
      </Header>

      {/* ===== Content ===== */}
      <Main fixed>
        <div>
          <h1 className='text-2xl font-bold tracking-tight'>Thư viện</h1>
          <p className='text-muted-foreground'>
            Thư viện game và ứng dụng của bạn
          </p>
        </div>
        <div className='my-4 flex items-end justify-between sm:my-0 sm:items-center'>
          <div className='flex flex-col gap-4 sm:my-4 sm:flex-row'>
            <Input
              placeholder='Tìm kiếm...'
              className='h-9 w-40 lg:w-[250px]'
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
            <Select value={appType} onValueChange={setAppType}>
              <SelectTrigger className='w-36'>
                <SelectValue>{appText.get(appType)}</SelectValue>
              </SelectTrigger>
              <SelectContent>
                <SelectItem value='all'>All Apps</SelectItem>
                <SelectItem value='Game'>Game</SelectItem>
                <SelectItem value='App'>App</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <Select value={sort} onValueChange={setSort}>
            <SelectTrigger className='w-16'>
              <SelectValue>
                <IconAdjustmentsHorizontal size={18} />
              </SelectValue>
            </SelectTrigger>
            <SelectContent align='end'>
              <SelectItem value='ascending'>
                <div className='flex items-center gap-4'>
                  <IconSortAscendingLetters size={16} />
                  <span>A-Z</span>
                </div>
              </SelectItem>
              <SelectItem value='descending'>
                <div className='flex items-center gap-4'>
                  <IconSortDescendingLetters size={16} />
                  <span>Z-A</span>
                </div>
              </SelectItem>
            </SelectContent>
          </Select>
        </div>
        <Separator className='shadow' />
        <ul className='faded-bottom no-scrollbar grid gap-4 overflow-auto pb-16 pt-4 md:grid-cols-2 lg:grid-cols-3'>
          {isLoading ? (
            <>Đang tải...</>
          ) : (
            filteredApps.map((app) => (
              <li
                key={app.productId}
                className='relative flex flex-auto flex-col flex-wrap justify-between gap-2 rounded-md border p-1 hover:shadow-md md:flex-row'
              >
                <img
                  src={`${SERVER_PUBLIC_URL}/${JSON.parse(app.desc).Thumbnail}`}
                  alt={app.name}
                  className='flex-auto rounded-md object-cover'
                />
                <div className='flex flex-auto flex-col justify-between p-2'>
                  <div className='flex flex-auto flex-row flex-wrap justify-between'>
                    <h3 className='text-lg font-semibold'>{app.name}</h3>
                    <span className='text-muted-foreground'>
                      {app.genre}
                    </span>
                  </div>
                  <Link to='/comingsoon' className='flex flex-auto'>
                    <Button className='flex-auto'>
                      <Play size={16} />
                      Chơi
                    </Button>
                  </Link>
                </div>
              </li>
            ))
          )}
        </ul>
      </Main>
    </>
  )
}
