import {
  IconBarrierBlock,
  IconBrowserCheck,
  IconBug,
  IconChecklist,
  IconDeviceGamepad2,
  IconError404,
  IconHelp,
  IconLayoutDashboard,
  IconLock,
  IconLockAccess,
  IconNotification,
  IconPackages,
  IconPalette,
  IconServerOff,
  IconSettings,
  IconTool,
  IconUserCog,
  IconUserOff,
  IconUsers,
  IconUserShield,
} from '@tabler/icons-react'
import { AudioWaveform, Command, GalleryVerticalEnd } from 'lucide-react'
import { type SidebarData } from '../types'

export const sidebarData: SidebarData = {
  user: {
    userId: '',
    username: 'Khách',
    role: ['Guest'],
    avatar: undefined,
  },
  teams: [
    {
      name: 'ZWorkshop',
      logo: Command,
      plan: 'Test APP',
    },
    {
      name: 'Test Data',
      logo: GalleryVerticalEnd,
      plan: 'Enterprise',
    },
    {
      name: 'Test Data 2',
      logo: AudioWaveform,
      plan: 'Startup',
    },
  ],
  navGroups: [
    {
      title: 'General',
      items: [
        {
          title: 'Cửa hàng',
          url: '/',
          icon: IconDeviceGamepad2,
          authGroup: [],
        },
        {
          title: 'Thư viện',
          url: '/library',
          icon: IconLayoutDashboard,
          authGroup: ['Customer'],
        },
        {
          title: 'Tổng quát',
          url: '/dashboard',
          icon: IconLayoutDashboard,
          authGroup: ['Admin', 'SuperAdmin'],
        },
        {
          title: 'Lịch sử mua hàng',
          url: '/purchase-history',
          icon: IconChecklist,
          authGroup: ['Customer'],
        },
        {
          title: 'Quản lý App/Game',
          url: '/products',
          icon: IconPackages,
          authGroup: ['Admin', 'SuperAdmin'],
        },
        {
          title: 'Quản lý nhà phát hành',
          url: '/publishers',
          icon: IconPalette,
          authGroup: ['Admin', 'SuperAdmin'],
        },
        {
          title: 'Quản lý nhân viên',
          url: '/employees',
          icon: IconUserShield,
          authGroup: ['Admin', 'SuperAdmin'],
        },
        {
          title: 'Quản lý khách hàng',
          url: '/customers',
          icon: IconUsers,
          authGroup: ['Admin', 'SuperAdmin'],
        },
      ],
    },
    {
      title: 'Trang test',
      items: [
        {
          title: 'Auth',
          icon: IconLockAccess,
          items: [
            {
              title: 'Sign In',
              url: '/sign-in',
            },
            {
              title: 'Sign In (2 Col)',
              url: '/sign-in-2',
            },
            {
              title: 'Sign Up',
              url: '/sign-up',
            },
            {
              title: 'Forgot Password',
              url: '/forgot-password',
            },
            {
              title: 'OTP',
              url: '/otp',
            },
          ],
        },
        {
          title: 'Test báo lỗi',
          icon: IconBug,
          items: [
            {
              title: 'Unauthorized',
              url: '/401',
              icon: IconLock,
            },
            {
              title: 'Forbidden',
              url: '/403',
              icon: IconUserOff,
            },
            {
              title: 'Not Found',
              url: '/404',
              icon: IconError404,
            },
            {
              title: 'Internal Server Error',
              url: '/500',
              icon: IconServerOff,
            },
            {
              title: 'Maintenance Error',
              url: '/503',
              icon: IconBarrierBlock,
            },
          ],
        },
      ],
    },
    {
      title: 'Test khác',
      items: [
        {
          title: 'Settings',
          icon: IconSettings,
          items: [
            {
              title: 'Profile',
              url: '/settings',
              icon: IconUserCog,
            },
            {
              title: 'Account',
              url: '/settings/account',
              icon: IconTool,
            },
            {
              title: 'Appearance',
              url: '/settings/appearance',
              icon: IconPalette,
            },
            {
              title: 'Notifications',
              url: '/settings/notifications',
              icon: IconNotification,
            },
            {
              title: 'Display',
              url: '/settings/display',
              icon: IconBrowserCheck,
            },
          ],
        },
        {
          title: 'Help Center',
          url: '/help-center',
          icon: IconHelp,
          authGroup: [],
        },
      ],
    },
  ],
}
