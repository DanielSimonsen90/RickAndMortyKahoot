import Error from "./Error.js";

import UserJoin from "./UserJoin.js";
import UserLeave from "./UserLeave.js";

import GameCreate from "./GameCreate.js";
import GameStart from "./GameStart.js";
import GameEnd from "./GameEnd.js";

import NewQuestion from "./NewQuestion.js";
import RoundEnd from "./RoundEnd.js";
import AutoEndRound from "./AutoEndRound.js";

export default [
  Error,
  UserJoin,
  UserLeave,
  GameCreate,
  GameStart,
  GameEnd,
  NewQuestion,
  RoundEnd,
  AutoEndRound,
] as const;