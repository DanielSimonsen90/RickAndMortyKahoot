import KahootHub from "../KahootHub/index.js";
import { getCurrentUser, saveCurrentUser } from "../utils.js";
KahootHub.start();
const user = getCurrentUser();
const gameId = $("#game-id").val();
if (user && typeof gameId === 'string') {
    user.gameId = gameId;
    saveCurrentUser(user);
    $("#start-game").on('click', () => {
        KahootHub.broadcast('StartGame', gameId);
    });
    $("#leave-game").on('click', () => {
        KahootHub.broadcast('Disconnect', user.id, gameId);
    });
    $("#leave-and-end-game").on('click', () => {
        KahootHub.broadcast('EndGame', gameId);
        KahootHub.broadcast('Disconnect', user.id, gameId);
    });
}
