import KahootHub from "../KahootHub/index.js";
import { getCurrentUser, saveCurrentUser } from "../utils.js";

KahootHub.start();

const user = getCurrentUser();
const gameId = $("#game-id").val();
if (user && typeof gameId === 'string') {
  user.gameId = gameId;
  saveCurrentUser(user);
}

$("#start-game").on('click', () => {
  alert('Start game will be implemented soon');
})