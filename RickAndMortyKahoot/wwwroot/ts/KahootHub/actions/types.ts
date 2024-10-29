import { Answer } from "../../models/Answer";
import { Guid } from "../../types";

type KahootHubActions = {
  Connect: [userId: Guid, inviteCode: Guid];
  Disconnect: [userId: Guid, gameId: Guid];

  CreateGame: [userId: Guid, amountOfQuestions: number | null];
  StartGame: [gameId: Guid, hostId: Guid];
  EndGame: [gameId: Guid, hostId: Guid];

  NextQuestion: [gameId: Guid, hostId: Guid];
  SubmitAnswer: [gameId: Guid, answer: Answer];
  EndRound: [gameId: Guid, hostId: Guid];
}

export default KahootHubActions;