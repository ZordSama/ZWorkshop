import path from 'path'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import { TanStackRouterVite } from '@tanstack/router-plugin/vite'
import 'dotenv/config'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), TanStackRouterVite()],
  base: './',
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),

      // fix loading all icon chunks in dev mode
      // https://github.com/tabler/tabler-icons/issues/1233
      '@tabler/icons-react': '@tabler/icons-react/dist/esm/icons/index.mjs',
    },
  },
  // Prevent Vite from handling electron-specific modules
  optimizeDeps: {
    exclude: ['electron'],
  },
  // Ensure proper handling for dependencies in Electron
  build: {
    outDir: 'dist',
    assetsDir: 'assets',
    emptyOutDir: true,
    rollupOptions: {
      external: ['electron'],
      output: {
        manualChunks: undefined, // Avoid code splitting
      },
    },
    assetsInlineLimit: 0,
    chunkSizeWarningLimit: 1600,
    // Ensure proper path resolution in production
    sourcemap: true,
  },
  server: {
    port: 5173,
  },
})
