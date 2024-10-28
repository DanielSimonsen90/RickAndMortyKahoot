import { Guid } from "../../types";

type KahootHubActions = {
  SendMessage: [username: string, message: string];

  Connect: [userId: Guid, gameId: Guid];
  Disconnect: [userId: Guid, gameId: Guid];

  CreateGame: [userId: Guid, amountOfQuestions: number | null];
}

export default KahootHubActions;