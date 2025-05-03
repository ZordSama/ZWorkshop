import { usePublishers } from '../context/publishers-context'
import { PublishersDeleteDialog } from './publishers-delete-dialog'
import { PublishersActionDialog } from './publishers-action-dialog'

export function PublishersDialogs({ onSuccess }: { onSuccess: () => void }) {
  const { open, setOpen, currentRow, setCurrentRow } = usePublishers()
  return (
    <>
      <PublishersActionDialog
        key='publisher-add'
        open={open === 'add'}
        onOpenChange={() => setOpen('add')}
        onSuccess={onSuccess}
      />

      {currentRow && (
        <>
          <PublishersActionDialog
            key={`publisher-edit-${currentRow.publisherId}`}
            open={open === 'edit'}
            onOpenChange={() => {
              setOpen('edit')
              setTimeout(() => {
                setCurrentRow(null)
              }, 500)
            }}
            currentRow={currentRow}
            onSuccess={onSuccess}
          />

          <PublishersDeleteDialog
            key={`publisher-delete-${currentRow.publisherId}`}
            open={open === 'delete'}
            onOpenChange={() => {
              setOpen('delete')
              setTimeout(() => {
                setCurrentRow(null)
              }, 500)
            }}
            currentRow={currentRow}
            onSuccess={onSuccess}
          />
        </>
      )}
    </>
  )
}
