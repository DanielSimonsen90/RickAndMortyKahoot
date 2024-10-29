import { getCurrentUser, saveCurrentUser } from "../utils.js";
import KahootHub from "../KahootHub/index.js";

const user = getCurrentUser();
const gameId = $("#game-id").val();
if (user && typeof gameId === 'string') {
  user.gameId = gameId;
  saveCurrentUser(user);

  KahootHub.start();

  $("#start-game").on('click', () => {
    KahootHub.broadcast('StartGame', gameId, user.id);
  })

  $("#leave-game").on('click', () => {
    KahootHub.broadcast('Disconnect', user.id, gameId);
  });
  $("#leave-and-end-game").on('click', () => {
    KahootHub.broadcast('EndGame', gameId, user.id);
    KahootHub.broadcast('Disconnect', user.id, gameId);
  });
}
