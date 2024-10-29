import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
/**
 * When a user joins a game, this event is triggered.
 */
export default CreateEvent('UserJoin', (gameId, user) => {
    const currentUser = getCurrentUser();
    if (currentUser === null)
        return;
    // If the user that joined is the current user, navigate to the game page of the user's game
    if (currentUser.id === user.id)
        navigate(`/Game/${user.gameId}`);
    // If the user that joined is in the same game as the current user, add the user to the user list
    else if (currentUser.gameId && currentUser.gameId === user.gameId) {
        $(".user-list").append(`<li id="${user.id}">${user.username}</li>`);
    }
});
