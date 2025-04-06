import { ref, watch } from 'vue'

export function useTheme() {
  const theme = ref(localStorage.getItem('theme') || 'light')
  
  const toggleTheme = () => {
    theme.value = theme.value === 'light' ? 'dark' : 'light'
    localStorage.setItem('theme', theme.value)
    updateThemeClass()
  }
  
  const updateThemeClass = () => {
    if (theme.value === 'dark') {
      document.documentElement.classList.add('dark')
    } else {
      document.documentElement.classList.remove('dark')
    }
  }
  
  // Initialize theme when component mounts
  if (process.client) {
    updateThemeClass()
    
    // Watch for theme changes
    watch(theme, () => {
      updateThemeClass()
    })
  }
  
  return {
    theme,
    toggleTheme
  }
}