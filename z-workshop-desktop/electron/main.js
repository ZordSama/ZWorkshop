const { app, BrowserWindow } = require('electron')
const path = require('path')
const url = require('url')

// Keep a global reference of the window object
let mainWindow

function createWindow() {
  // Create the browser window
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      nodeIntegration: true,
      contextIsolation: false
    }
  })

  // Load the app
  const isProd = process.env.NODE_ENV === 'production'
  
  if (isProd) {
    // In production, load the built app
    mainWindow.loadURL(url.format({
      pathname: path.join(__dirname, '../.output/public/index.html'),
      protocol: 'file:',
      slashes: true
    }))
  } else {
    // In development, load from the dev server
    mainWindow.loadURL('http://localhost:3000')
  }

  // Open DevTools in development
  if (!isProd) {
    mainWindow.webContents.openDevTools()
  }

  // Handle window being closed
  mainWindow.on('closed', function () {
    mainWindow = null
  })
}

// Create window when Electron is ready
app.on('ready', createWindow)

// Quit when all windows are closed
app.on('window-all-closed', function () {
  if (process.platform !== 'darwin') app.quit()
})

// On macOS, recreate window when dock icon is clicked
app.on('activate', function () {
  if (mainWindow === null) createWindow()
})