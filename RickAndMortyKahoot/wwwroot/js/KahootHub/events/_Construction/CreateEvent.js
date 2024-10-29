/**
 * Creates an event handler data object that wraps the event name and handler.
 * @param eventName Name of the event to wrap
 * @param handler Handler to call when the event is received
 * @returns Event handler data
 */
export default function CreateEvent(eventName, handler) {
    return {
        eventName,
        handler
    };
}
