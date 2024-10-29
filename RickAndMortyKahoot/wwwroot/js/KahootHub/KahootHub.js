/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@microsoft/signalr/dist/esm/index.d.ts" />
import Events from './events/index.js';
const logEvent = (...args) => console.log(`[KahootHub Event]`, ...args);
const logAction = (...args) => console.log(`[KahootHub Action]`, ...args);
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
    _connection;
    async start() {
        return this._connection.start()
            .then(() => console.log('KahootHub started'))
            .catch(reason => {
            console.error('Failed to start KahootHub', reason);
            return Promise.reject(reason);
        });
    }
    stop() {
        return this._connection.stop();
    }
    broadcast(actionName, ...args) {
        const broadcast = () => {
            logAction(actionName, args);
            this._connection.invoke(actionName, ...args);
        };
        if (this._connection.state !== SignalR.HubConnectionState.Connected) {
            return this.start().then(broadcast);
        }
        else {
            return broadcast();
        }
    }
    on(eventName, handler) {
        this._connection.on(eventName, (...args) => logEvent(eventName, ...args));
        return this._connection.on(eventName, handler);
    }
}
const instance = new KahootHub();
export default instance;
