import * as React from 'react'
import { ChevronLeft, ChevronRight } from 'lucide-react'
import { DayPicker } from 'react-day-picker'
import { format, getMonth, getYear } from 'date-fns'
import { cn } from '@/lib/utils'
import { buttonVariants } from '@/components/ui/button'


export type CalendarProps = React.ComponentProps<typeof DayPicker>

function Calendar({
  className,
  classNames,
  showOutsideDays = true,
  locale,
  ...props
}: CalendarProps) {
  const [month, setMonth] = React.useState<Date>(props.defaultMonth || new Date())
  
  const handleYearChange = (year: number) => {
    const newDate = new Date(month)
    newDate.setFullYear(year)
    setMonth(newDate)
  }
  
  const handleMonthChange = (monthIndex: number) => {
    const newDate = new Date(month)
    newDate.setMonth(monthIndex)
    setMonth(newDate)
  }
  
  const years = React.useMemo(() => {
    const currentYear = new Date().getFullYear()
    return Array.from({ length: 125 }, (_, i) => currentYear - 124 + i).reverse()
  }, [])
  
  const getLocalizedMonths = () => {
    if (locale) {
      return Array.from({ length: 12 }, (_, i) => {
        const date = new Date(2000, i, 1)
        return format(date, 'LLL', { locale })
      });
    } else {
      return Array.from({ length: 12 }, (_, i) => {
        return new Date(2000, i, 1).toLocaleString('en-US', { month: 'narrow' });
      });
    }
  }
  
  const months = getLocalizedMonths();

  const CustomCaption = ({ displayMonth }: { displayMonth: Date }) => {
    const monthIndex = getMonth(displayMonth)
    const year = getYear(displayMonth)
    
    return (
      <div className="flex justify-center space-x-2">
        <select 
          className="text-sm px-2 py-1 rounded-md border bg-background"
          value={monthIndex}
          onChange={(e) => handleMonthChange(Number(e.target.value))}
        >
          {months.map((month, idx) => (
            <option key={idx} value={idx}>
              {month}
            </option>
          ))}
        </select>
        
        <select 
          className="text-sm px-2 py-1 rounded-md border bg-background"
          value={year}
          onChange={(e) => handleYearChange(Number(e.target.value))}
        >
          {years.map((y) => (
            <option key={y} value={y}>
              {y}
            </option>
          ))}
        </select>
      </div>
    )
  }

  return (
    <DayPicker
      month={month}
      onMonthChange={setMonth}
      showOutsideDays={showOutsideDays}
      className={cn('p-3', className)}
      classNames={{
        months: 'flex flex-col sm:flex-row space-y-4 sm:space-x-4 sm:space-y-0',
        month: 'space-y-4',
        caption: 'flex justify-center pt-1 relative items-center',
        caption_label: 'hidden', // Hide default caption label
        nav: 'space-x-1 flex items-center',
        nav_button: cn(
          buttonVariants({ variant: 'outline' }),
          'h-7 w-7 bg-transparent p-0 opacity-50 hover:opacity-100'
        ),
        nav_button_previous: 'absolute left-1',
        nav_button_next: 'absolute right-1',
        table: 'w-full border-collapse space-y-1',
        head_row: 'flex',
        head_cell:
          'text-muted-foreground rounded-md w-8 font-normal text-[0.8rem]',
        row: 'flex w-full mt-2',
        cell: cn(
          'relative p-0 text-center text-sm focus-within:relative focus-within:z-20 [&:has([aria-selected])]:bg-accent [&:has([aria-selected].day-outside)]:bg-accent/50 [&:has([aria-selected].day-range-end)]:rounded-r-md',
          props.mode === 'range'
            ? '[&:has(>.day-range-end)]:rounded-r-md [&:has(>.day-range-start)]:rounded-l-md first:[&:has([aria-selected])]:rounded-l-md last:[&:has([aria-selected])]:rounded-r-md'
            : '[&:has([aria-selected])]:rounded-md'
        ),
        day: cn(
          buttonVariants({ variant: 'ghost' }),
          'h-8 w-8 p-0 font-normal aria-selected:opacity-100'
        ),
        day_range_start: 'day-range-start',
        day_range_end: 'day-range-end',
        day_selected:
          'bg-primary text-primary-foreground hover:bg-primary hover:text-primary-foreground focus:bg-primary focus:text-primary-foreground',
        day_today: 'bg-accent text-accent-foreground',
        day_outside:
          'day-outside text-muted-foreground aria-selected:bg-accent/50 aria-selected:text-muted-foreground',
        day_disabled: 'text-muted-foreground opacity-50',
        day_range_middle:
          'aria-selected:bg-accent aria-selected:text-accent-foreground',
        day_hidden: 'invisible',
        ...classNames,
      }}
      components={{
        IconLeft: () => <ChevronLeft className="h-4 w-4" />,
        IconRight: () => <ChevronRight className="h-4 w-4" />,
        Caption: ({ displayMonth }) => <CustomCaption displayMonth={displayMonth} />
      }}
      {...props}
    />
  )
}
Calendar.displayName = 'Calendar'

export { Calendar }