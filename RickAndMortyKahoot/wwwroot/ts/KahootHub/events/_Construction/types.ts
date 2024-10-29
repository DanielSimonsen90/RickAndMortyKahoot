import { Game } from "../../../models/Game";
import { User } from "../../../models/User";
import { Guid } from "../../../types";

type KahootHubEvents = {
  Error: [action: string, error: any];

  UserJoin: [gameId: Guid, user: User]
  UserLeave: [gameId: Guid, user: User]

  GameCreate: [gameId: Guid, game: Game];
}

export default KahootHubEvents;