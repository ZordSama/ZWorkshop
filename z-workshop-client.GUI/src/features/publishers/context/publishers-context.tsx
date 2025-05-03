import React, { useState } from 'react'
import useDialogState from '@/hooks/use-dialog-state'
import { Publisher } from '../data/schema'

type PublishersDialogType = 'add' | 'edit' | 'delete'

interface PublishersContextType {
  open: PublishersDialogType | null
  setOpen: (str: PublishersDialogType | null) => void
  currentRow: Publisher | null
  setCurrentRow: React.Dispatch<React.SetStateAction<Publisher | null>>
}

const PublishersContext = React.createContext<PublishersContextType | null>(null)

interface Props {
  children: React.ReactNode
}

export default function PublishersProvider({ children }: Props) {
  const [open, setOpen] = useDialogState<PublishersDialogType>(null)
  const [currentRow, setCurrentRow] = useState<Publisher | null>(null)

  return (
    <PublishersContext value={{ open, setOpen, currentRow, setCurrentRow }}>
      {children}
    </PublishersContext>
  )
}

// eslint-disable-next-line react-refresh/only-export-components
export const usePublishers = () => {
  const publishersContext = React.useContext(PublishersContext)

  if (!publishersContext) {
    throw new Error('usePublishers has to be used within <PublishersContext>')
  }

  return publishersContext
}
