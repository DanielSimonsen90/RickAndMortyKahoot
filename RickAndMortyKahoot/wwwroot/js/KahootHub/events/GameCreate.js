import { navigate, saveToSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('GameCreate', (gameId, game) => {
    saveToSessionStorage('isHost', true);
    navigate(`/Game/${game.id}`);
});
