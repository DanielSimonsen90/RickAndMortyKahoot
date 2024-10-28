import { Guid } from "../types";

export type User = {
  id: Guid;
  username: string;
  gameId: Guid;
}