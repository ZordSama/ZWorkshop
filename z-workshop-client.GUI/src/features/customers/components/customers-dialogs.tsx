import { useCustomers } from '../context/customers-context'
import { CustomersDeleteDialog } from './customer-delete-dialog'
import { CustomersActionDialog } from './customers-action-dialog'

export function CustomersDialogs({ onSuccess }: { onSuccess: () => void }) {
  const { open, setOpen, currentRow, setCurrentRow } = useCustomers()
  return (
    <>
      <CustomersActionDialog
        key='customer-add'
        open={open === 'add'}
        onOpenChange={() => setOpen('add')}
        onSuccess={onSuccess}
      />

      {currentRow && (
        <>
          <CustomersActionDialog
            key={`customer-edit-${currentRow.customerDto.customerId}`}
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

          <CustomersDeleteDialog
            key={`customer-delete-${currentRow.customerDto.customerId}`}
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
