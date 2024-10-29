import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
/**
 * When a game starts, this event is triggered.
 */
export default CreateEvent('GameStart', (gameId) => {
    const currentUser = getCurrentUser();
    if (currentUser === null)
        return;
    // If the current user is in the game that started, navigate to the active game page
    if (currentUser.gameId === gameId) {
        navigate(`/Game/${gameId}/active`);
    }
});
