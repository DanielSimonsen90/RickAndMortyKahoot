/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />
/**
 * C# Guid representation
 */
export type Guid = string;

/**
 * Set global vairables that Typescript doesn't recognize
 */

declare global {
  // @ts-ignore
  declare let SignalR: typeof signalR;
  
  interface Window {
    KahootHub: typeof import('./KahootHub/KahootHub').default;
    roundTimeout: number;
    stopTimer: () => void;
  }
}