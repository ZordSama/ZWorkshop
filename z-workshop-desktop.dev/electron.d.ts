interface Window {
    require: (module: string) => any;
    electron: {
      ipcRenderer: {
        send: (channel: string, ...args: any[]) => void;
        on: (channel: string, listener: (event: any, ...args: any[]) => void) => void;
        removeAllListeners: (channel: string) => void;
      };
    };
  }