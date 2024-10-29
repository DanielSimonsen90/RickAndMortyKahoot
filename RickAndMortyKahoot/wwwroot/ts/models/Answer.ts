import { Guid } from "../types";

export type Answer = {
  questionId: Guid;
  index: number;
  userId: Guid;
  timestamp: number;
}