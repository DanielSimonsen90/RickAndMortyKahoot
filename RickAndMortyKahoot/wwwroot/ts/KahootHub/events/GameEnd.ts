import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

/**
 * When a game ends, this event is triggered.
 */
export default CreateEvent('GameEnd', (gameId, score) => {
  const currentUser = getCurrentUser();
  if (currentUser === null) return;

  // If user is not in the game that ended, return
  else if (currentUser.gameId !== gameId) return;

  // Navigate to the game page
  navigate(`/game/${gameId}`);
});