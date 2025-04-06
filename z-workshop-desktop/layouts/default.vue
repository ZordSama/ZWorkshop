<template>
    <div class="min-h-screen bg-background">
      <header class="bg-primary text-primary-foreground p-4">
        <div class="container mx-auto flex justify-between items-center">
          <h1 class="text-xl font-bold">Electron Nuxt App</h1>
          <nav class="flex space-x-4">
            <NuxtLink to="/" class="hover:text-opacity-80">Home</NuxtLink>
            <NuxtLink to="/settings" class="hover:text-opacity-80">Settings</NuxtLink>
            <Button variant="outline" size="sm" @click="toggleTheme">
              <i-icon :name="theme === 'light' ? 'moon' : 'sun'" class="mr-2" />
              {{ theme === 'light' ? 'Dark' : 'Light' }}
            </Button>
          </nav>
        </div>
      </header>
      
      <main class="container mx-auto p-4">
        <slot />
      </main>
      
      <footer class="bg-muted p-4 mt-8">
        <div class="container mx-auto text-center text-muted-foreground">
          <p>&copy; {{ new Date().getFullYear() }} Your Company Name</p>
        </div>
      </footer>
    </div>
  </template>
  
  <script setup>
  import { Button } from '@/components/ui/button'
  import { useTheme } from '~/composables/useTheme'
  
  const { theme, toggleTheme } = useTheme()
  </script>