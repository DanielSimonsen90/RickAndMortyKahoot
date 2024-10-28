/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />

import type KahootHubActions from './actions/types';
import type KahootHubEvents from './events/_Construction/types';
import Events from './events/index.js';

const logEvent = (...args: any[]) => console.log(`[KahootHub Event]`, ...args);
const logAction = (...args: any[]) => console.log(`[KahootHub Action]`, ...args);

// @ts-ignore
window.SignalR = signalR;
class KahootHub {
  constructor() {
    this._connection = new SignalR.HubConnectionBuilder()
      .withUrl("/kahoothub")
      .withAutomaticReconnect()
      .build();

    for (const { eventName, handler } of Events) {
      // @ts-ignore
      this.on(eventName, handler);
    }
  }

  private _connection: signalR.HubConnection;

  public async start() {
    return this._connection.start().catch(reason => {
      console.error('Failed to start KahootHub', reason);
      return Promise.reject(reason);
    })
  }
  public stop() {
    return this._connection.stop();
  }

  public broadcast<ActionName extends keyof KahootHubActions>(
    actionName: ActionName,
    ...args: KahootHubActions[ActionName]
  ) {
    const broadcast = () => {
      logAction(actionName, ...args);
      this._connection.invoke(actionName, ...args);
    }

    if (this._connection.state !== SignalR.HubConnectionState.Connected) {
      return this.start().then(broadcast);
    } else {
      return broadcast();
    }
  }
  public on<EventName extends keyof KahootHubEvents>(
    eventName: EventName,
    handler: (...args: KahootHubEvents[EventName]) => void
  ) {
    this._connection.on(eventName, (...args) => logEvent(eventName, ...args));
    return this._connection.on(eventName, handler);
  }
}

const instance = new KahootHub();
export default instance;