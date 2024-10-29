import { Guid } from "../types";
import { GameQuestion } from "./GameQuestion";

export type Game = {
  id: Guid;
  hostId: Guid;
  userIds: Array<Guid>;
  inviteCode: Guid;
  questions: Array<GameQuestion>; 
  currentQuestion: GameQuestion | null;
  isActive: boolean;
}