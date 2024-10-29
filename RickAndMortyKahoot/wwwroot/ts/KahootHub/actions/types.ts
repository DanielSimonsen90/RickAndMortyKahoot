import { Guid } from "../../types";

type KahootHubActions = {
  Connect: [userId: Guid, inviteCode: Guid];
  Disconnect: [userId: Guid, gameId: Guid];

  CreateGame: [userId: Guid, amountOfQuestions: number | null];
  StartGame: [gameId: Guid];
  EndGame: [gameId: Guid];

  
}

export default KahootHubActions;