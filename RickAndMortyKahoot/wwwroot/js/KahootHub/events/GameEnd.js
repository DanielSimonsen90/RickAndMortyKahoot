import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('GameEnd', (gameId, score) => {
    const currentUser = getCurrentUser();
    if (currentUser === null)
        return;
    else if (currentUser.gameId !== gameId)
        return;
    navigate(`/game/${gameId}?userId=${currentUser.id}&showScore=true`);
});
