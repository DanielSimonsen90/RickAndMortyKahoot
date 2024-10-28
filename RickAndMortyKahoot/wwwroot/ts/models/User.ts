import { Guid } from "../types";

export type User = {
  Id: Guid;
  Username: string;
  GameId: Guid;
}