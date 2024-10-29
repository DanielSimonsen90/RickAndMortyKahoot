import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('GameEnd', (gameId) => {
    const currentUser = getCurrentUser();
    if (currentUser === null)
        return;
    else if (currentUser.gameId !== gameId)
        return;
    navigate("/Game");
});
