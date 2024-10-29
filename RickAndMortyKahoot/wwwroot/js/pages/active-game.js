import { getCurrentUser, getFromSessionStorage } from "../utils.js";
import KahootHub from "../KahootHub/index.js";
KahootHub.start().then(() => {
    const currentUser = getCurrentUser();
    if (currentUser === null || currentUser.gameId === null)
        return;
    $('#end-game').on('click', () => {
        KahootHub.broadcast('EndGame', currentUser.gameId, currentUser.id);
    });
    const isHost = getFromSessionStorage('isHost');
    if (isHost)
        KahootHub.broadcast('NextQuestion', currentUser.gameId, currentUser.id);
});
