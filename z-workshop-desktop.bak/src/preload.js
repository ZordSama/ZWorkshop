import { contextBridge, ipcRenderer } from "electron";

contextBridge.exposeInMainWorld("electronAPI", {
  // Window state
  onWindowState: (callback) =>
    ipcRenderer.on("window-state", (event, state) => callback(state)),
  isMaximized: () => ipcRenderer.invoke("window:is-maximized"),
  // Window controls
  minimize: () => ipcRenderer.send("window:minimize"),
  toggleMaximize: () => ipcRenderer.send("window:toggle-maximize"),
  close: () => ipcRenderer.send("window:close"),
});
