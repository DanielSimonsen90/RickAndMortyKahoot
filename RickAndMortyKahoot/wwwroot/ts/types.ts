/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />

export type Guid = string;

declare global {
  // @ts-ignore
  declare let SignalR: typeof signalR;
  
  interface Window {
    KahootHub: typeof import('./KahootHub/KahootHub').default;
    roundTimeout: number;
  }
}