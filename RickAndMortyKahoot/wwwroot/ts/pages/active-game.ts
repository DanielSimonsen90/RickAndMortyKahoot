import { getCurrentUser, getFromSessionStorage } from "../utils.js";
import KahootHub from "../KahootHub/index.js";


KahootHub.start().then(() => {
  const currentUser = getCurrentUser();
  if (currentUser === null || currentUser.gameId === null) return;

  const isHost = getFromSessionStorage('isHost');
  if (isHost) KahootHub.broadcast('NextQuestion', currentUser.gameId, currentUser.id);
})