import { getCurrentUser } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

/**
 * When a round automatically ends from all users answering in time, this event is triggered.
 */
export default CreateEvent('AutoEndRound', (gameId, hostId) => {
  window.stopTimer(); // Stop the timer, if it is running

  // If current user is not the host or in the of the game that was auto-ended, return
  const currentUser = getCurrentUser();
  if (currentUser === null || currentUser.gameId !== gameId || currentUser.id !== hostId) return;

  // Acknowledge the end of the round event from hub and broadcast EndRound action
  window.KahootHub.broadcast('EndRound', gameId, hostId);
});