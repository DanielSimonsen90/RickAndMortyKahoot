import { Answer } from "../../../models/Answer";
import { Game } from "../../../models/Game";
import { GameQuestion } from "../../../models/GameQuestion";
import { Score } from "../../../models/Score";
import { User } from "../../../models/User";
import { Guid } from "../../../types";

/**
 * The KahootHubEvents type is a union type that contains all the events that can be received from the KahootHub SignalR hub.
 * Each event is a tuple with arguments passed to the event.
 */
type KahootHubEvents = {
  Error: [action: string, error: any];

  UserJoin: [gameId: Guid, user: User]
  UserLeave: [gameId: Guid, user: User]

  GameCreate: [gameId: Guid, game: Game];
  GameStart: [gameId: Guid];
  GameEnd: [gameId: Guid, score: Score];

  NewQuestion: [gameId: Guid, question: GameQuestion];
  RoundEnd: [gameId: Guid, correctAnswer: Answer, score: Score, question: GameQuestion];
  AutoEndRound: [gameId: Guid, hostId: Guid];
}

export default KahootHubEvents;