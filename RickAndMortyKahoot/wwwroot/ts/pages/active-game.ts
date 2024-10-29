import { getCurrentUser, getFromSessionStorage } from "../utils.js";
import KahootHub from "../KahootHub/index.js";

KahootHub.start().then(() => {
  const currentUser = getCurrentUser();
  if (currentUser === null || currentUser.gameId === null) return;

  $('#end-game').on('click', () => {
    KahootHub.broadcast('EndGame', currentUser.gameId!, currentUser.id);
  });

  // If the current user is the host, broadcast the NextQuestion event as soon as the page loads
  const isHost = getFromSessionStorage('isHost');
  if (isHost) KahootHub.broadcast('NextQuestion', currentUser.gameId, currentUser.id);
});