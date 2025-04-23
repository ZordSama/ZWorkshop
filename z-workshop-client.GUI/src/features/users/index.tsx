import { useEffect, useState } from 'react'
import axios from 'axios'
import { handleQueryError } from '@/utils'
import { Header } from '@/components/layout/header'
import { Main } from '@/components/layout/main'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { columns } from './components/users-columns'
import { UsersDialogs } from './components/users-dialogs'
import { UsersPrimaryButtons } from './components/users-primary-buttons'
import { UsersTable } from './components/users-table'
import UsersProvider from './context/users-context'
import { User, userListSchema } from './data/schema'

export default function Users() {
  const [userList, setUserList] = useState<User[]>([])

  const fetchUsers = async () => {
    try {
      const res = await axios.get('http://localhost:5062/api/Users')
      // console.log(res.data)
      const parsed = userListSchema.parse(res.data)
      // console.log(parsed)
      setUserList(parsed)
    } catch (err: any) {
      handleQueryError(err)
    }
  }
  useEffect(() => {
    // console.log('fetching users')
    fetchUsers()
  }, [])

  return (
    <UsersProvider>
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
            <h2 className='text-2xl font-bold tracking-tight'>Quản lý người dùng</h2>
            <p className='text-muted-foreground'>
              Xem và quản lý danh sách người dùng.
            </p>
          </div>
          <UsersPrimaryButtons />
        </div>
        <div className='-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-x-12 lg:space-y-0'>
          <UsersTable data={userList} columns={columns} />
        </div>
      </Main>

      <UsersDialogs onSuccess={fetchUsers} />
    </UsersProvider>
  )
}
