import { useEmployees } from '../context/employees-context'
import { EmployeesDeleteDialog } from './employees-delete-dialog'
import { EmployeesActionDialog } from './employees-action-dialog'

export function EmployeesDialogs({ onSuccess }: { onSuccess: () => void }) {
  const { open, setOpen, currentRow, setCurrentRow } = useEmployees()
  return (
    <>
      <EmployeesActionDialog
        key='employee-add'
        open={open === 'add'}
        onOpenChange={() => setOpen('add')}
        onSuccess={onSuccess}
      />

      {currentRow && (
        <>
          <EmployeesActionDialog
            key={`employee-edit-${currentRow.employeeDto.employeeId}`}
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

          <EmployeesDeleteDialog
            key={`employee-delete-${currentRow.employeeDto.employeeId}`}
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
