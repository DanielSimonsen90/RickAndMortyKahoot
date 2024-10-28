import KahootHubEvents from "./types";

export default function CreateEvent<EventName extends keyof KahootHubEvents>(
  eventName: EventName,
  handler: (...args: KahootHubEvents[EventName]) => void
) {
  return {
    eventName,
    handler
  }
}