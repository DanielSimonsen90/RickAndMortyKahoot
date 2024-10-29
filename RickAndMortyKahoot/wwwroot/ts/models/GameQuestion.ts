import { Guid } from "../types";

type Question = {
  id: Guid;
  title: string;
  answser: string;
  choices: string[];
  readonly answerIndex: number;
}

export type GameQuestion = Question & {
  available: boolean;
  timestamp: number;
}