import { Guid } from "../types";

export type Game = {
  id: Guid;
  hostId: Guid;
  userIds: Array<Guid>;
  inviteCode: Guid;
  questions: Array<any>; // TODO: Define GameQuestion
  isActive: boolean;
}