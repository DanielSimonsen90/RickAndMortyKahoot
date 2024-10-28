/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />
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
    _connection;
    start() {
        return this._connection.start();
    }
    stop() {
        return this._connection.stop();
    }
    broadcast(actionName, ...args) {
        return this._connection.invoke(actionName, ...args);
    }
    on(eventName, handler) {
        return this._connection.on(eventName, handler);
    }
};
