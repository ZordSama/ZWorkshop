// electron/main.cjs
const { app, BrowserWindow } = require('electron')
const path = require('path')
const url = require('url')
const fs = require('fs')

// Keep a global reference of the window object
let mainWindow

// Fix for __dirname pointing to the electron folder
// Go up one level to the project root
const projectRoot = path.join(__dirname, '..')

function createWindow() {
  // Create the browser window
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      preload: path.join(__dirname, 'preload.cjs'),

      webSecurity: process.env.NODE_ENV !== 'development',
    },
  })

  // Load the app
  if (process.env.NODE_ENV === 'development') {
    // Load from Vite dev server
    mainWindow.loadURL('http://localhost:5173')
    // Open DevTools
    mainWindow.webContents.openDevTools()
  } else {
    // Load the index.html from the dist folder - now using projectRoot
    const indexPath = path.join(projectRoot, 'dist', 'index.html')
    console.log('Loading index from:', indexPath)
    mainWindow.loadFile(indexPath)
  }

  // Emitted when the window is closed
  mainWindow.on('closed', function () {
    mainWindow = null
  })
}

// This method will be called when Electron has finished initialization
app.whenReady().then(() => {
  createWindow()
})

// Quit when all windows are closed
app.on('window-all-closed', function () {
  // On macOS it is common for applications to stay open
  // until the user explicitly quits
  if (process.platform !== 'darwin') app.quit()
})

app.on('activate', function () {
  // On macOS it's common to re-create a window when the dock icon is clicked
  if (mainWindow === null) createWindow()
})
