import { ROUND_END_TIMEOUT_SECONDS } from "../../constants.js";
import { getCurrentUser, getFromSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('RoundEnd', (gameId, score) => {
    const currentUser = getCurrentUser();
    if (currentUser === null || currentUser.gameId !== gameId)
        return;
    console.log('RoundEnd', score);
    const isHost = getFromSessionStorage('isHost');
    if (isHost) {
        clearTimeout(window.roundTimeout);
        setTimeout(() => {
            window.KahootHub.broadcast('NextQuestion', gameId, currentUser.id);
        }, ROUND_END_TIMEOUT_SECONDS * 1000);
    }
});
