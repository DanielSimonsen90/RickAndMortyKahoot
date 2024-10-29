import { getCurrentUser, getFromSessionStorage, navigate, saveToSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

/**
 * When a user leaves a game, this event is triggered.
 */
export default CreateEvent('UserLeave', (gameId, user) => {
  const currentUser = getCurrentUser();
  if (currentUser === null) return;

  // If the user that left is the current user, navigate to the home page and reset the isHost session storage value (if it is set)
  if (currentUser.id === user.id) {
    const isHost = getFromSessionStorage('isHost');
    if (isHost) saveToSessionStorage('isHost', undefined);
    navigate('/');
  }

  // If the user that left is in the same game as the current user, remove the user from the user list
  else if (currentUser.gameId && currentUser.gameId === user.gameId) {
    $(`.user-list li#${user.id}`).remove();
  }
})