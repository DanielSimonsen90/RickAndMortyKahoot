import { getCurrentUser, navigate, saveToSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
/**
 * When a game is created, this event is triggered.
 */
export default CreateEvent('GameCreate', (gameId, game) => {
    const currentUser = getCurrentUser();
    if (!currentUser || game.hostId !== currentUser.id)
        return;
    // Save the isHost session storage value and navigate to the game page
    saveToSessionStorage('isHost', true);
    navigate(`/Game/${game.id}`);
});
