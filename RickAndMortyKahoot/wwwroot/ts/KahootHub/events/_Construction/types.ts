import { Game } from "../../../models/Game";
import { GameQuestion } from "../../../models/GameQuestion";
import { User } from "../../../models/User";
import { Guid } from "../../../types";

type KahootHubEvents = {
  Error: [action: string, error: any];

  UserJoin: [gameId: Guid, user: User]
  UserLeave: [gameId: Guid, user: User]

  GameCreate: [gameId: Guid, game: Game];
  GameStart: [gameId: Guid];
  GameEnd: [gameId: Guid];

  NewQuestion: [gameId: Guid, question: GameQuestion];
  RoundEnd: [gameId: Guid, score: Array<{ key: Guid, value: number }>];
  AutoEndRound: [gameId: Guid, hostId: Guid];
}

export default KahootHubEvents;