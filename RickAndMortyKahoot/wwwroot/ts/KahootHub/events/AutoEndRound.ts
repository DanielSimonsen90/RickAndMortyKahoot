import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('AutoEndRound', (gameId, hostId) => {
  window.stopTimer();
  window.KahootHub.broadcast('EndRound', gameId, hostId);
});