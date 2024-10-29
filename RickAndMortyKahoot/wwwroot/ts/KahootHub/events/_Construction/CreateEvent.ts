import KahootHubEvents from "./types";

/**
 * Creates an event handler data object that wraps the event name and handler.
 * @param eventName Name of the event to wrap
 * @param handler Handler to call when the event is received
 * @returns Event handler data
 */
export default function CreateEvent<EventName extends keyof KahootHubEvents>(
  eventName: EventName,
  handler: (...args: KahootHubEvents[EventName]) => void
) {
  return {
    eventName,
    handler
  }
}