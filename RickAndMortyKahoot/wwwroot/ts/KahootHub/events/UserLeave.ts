import { getCurrentUser, getFromSessionStorage, navigate, saveToSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('UserLeave', (gameId, user) => {
  const currentUser = getCurrentUser();
  if (currentUser === null) return;

  if (currentUser.id === user.id) {
    const isHost = getFromSessionStorage('isHost');
    if (isHost) saveToSessionStorage('isHost', false);
    navigate('/');
  }
  else if (currentUser.gameId && currentUser.gameId === user.gameId) {
    $(`.user-list li#${user.id}`).remove();
  }
})