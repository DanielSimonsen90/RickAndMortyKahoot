/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />

import type KahootHubActions from './actions/types';
import type KahootHubEvents from './events/_Construction/types';
import Events from './events/index.js';

// Log events and actions to the console
const logEvent = (...args: any[]) => console.log(`[KahootHub Event]`, ...args);
const logAction = (...args: any[]) => console.log(`[KahootHub Action]`, ...args);

// @ts-ignore -- SignalR is a global variable, but not recognized by TypeScript
window.SignalR = signalR;
class KahootHub {
  constructor() {
    // Connect to the KahootHub SignalR hub
    this._connection = new SignalR.HubConnectionBuilder()
      .withUrl("/kahoothub")
      .withAutomaticReconnect()
      .build();

    // Register event handlers from the Events module
    for (const { eventName, handler } of Events) {
      // @ts-ignore
      this.on(eventName, handler);
    }
  }

  private _connection: signalR.HubConnection;

  /**
   * Starts the connection to the KahootHub SignalR hub.
   * @returns A promise that resolves when the connection is started
   */
  public async start() {
    return this._connection.start()
      .then(() => console.log('KahootHub started'))
      .catch(reason => {
        console.error('Failed to start KahootHub', reason);
        return Promise.reject(reason);
      });
  }
  public stop() {
    return this._connection.stop();
  }

  /**
   * Broadcasts an action to the KahootHub SignalR hub.
   * @param actionName Name of the action to broadcast
   * @param args Arguments to pass to the action
   */
  public broadcast<ActionName extends keyof KahootHubActions>(
    actionName: ActionName,
    ...args: KahootHubActions[ActionName]
  ) {
    const broadcast = () => {
      logAction(actionName, args);
      this._connection.invoke(actionName, ...args);
    };

    // If the connection is not connected, start it before broadcasting, then broadcast
    if (this._connection.state !== SignalR.HubConnectionState.Connected) {
      return this.start().then(broadcast);
    } else {
      return broadcast();
    }
  }

  /**
   * Registers an event handler for the specified event.
   * @param eventName Name of the event to register a handler for
   * @param handler Handler to call when the event is received
   */
  public on<EventName extends keyof KahootHubEvents>(
    eventName: EventName,
    handler: (...args: KahootHubEvents[EventName]) => void
  ) {
    this._connection.on(eventName, (...args) => logEvent(eventName, ...args));
    return this._connection.on(eventName, handler);
  }
}

/**
 * Singleton instance of the KahootHub class.
 */
const instance = new KahootHub();
export default instance;