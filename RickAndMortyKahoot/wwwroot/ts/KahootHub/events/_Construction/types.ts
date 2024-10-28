import { Game } from "../../../models/Game";
import { User } from "../../../models/User";

type KahootHubEvents = {
  Error: [action: string, error: any];

  UserJoin: [user: User]
  UserLeave: [user: User]

  GameCreate: [game: Game];
}

export default KahootHubEvents;