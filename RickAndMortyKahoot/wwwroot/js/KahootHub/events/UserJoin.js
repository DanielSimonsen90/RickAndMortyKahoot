import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('UserJoin', (gameId, user) => {
    const currentUser = getCurrentUser();
    if (currentUser === null)
        return;
    if (currentUser.id === user.id)
        navigate(`/Game/${user.gameId}`);
    else if (currentUser.gameId && currentUser.gameId === user.gameId) {
        $(".user-list").append(`<li>${user.username}</li>`);
    }
});
