import { Guid } from "../types.js";

export type User = {
  id: Guid;
  username: string;
  gameId: Guid | null;
}