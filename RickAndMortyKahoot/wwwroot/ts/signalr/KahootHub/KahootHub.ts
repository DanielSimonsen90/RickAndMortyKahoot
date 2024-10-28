/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />

import type KahootHubActions from './actions/types';
import type KahootHubEvents from './events/_Construction/types';
import Events from './events/index.js';

export default new class KahootHub {
  constructor() {
    // @ts-ignore
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl("/kahootHub")
      .build();

    for (const { eventName, handler } of Events) {
      this.on(eventName, handler);
    }
  }

  private _connection: signalR.HubConnection;

  public start() {
    return this._connection.start();
  }
  public stop() {
    return this._connection.stop();
  }

  public broadcast<ActionName extends keyof KahootHubActions>(
    actionName: ActionName,
    ...args: KahootHubActions[ActionName]
  ) {
    return this._connection.invoke(actionName, ...args);
  }
  public on<EventName extends keyof KahootHubEvents>(
    eventName: EventName,
    handler: (...args: KahootHubEvents[EventName]) => void
  ) {
    return this._connection.on(eventName, handler);
  }
}