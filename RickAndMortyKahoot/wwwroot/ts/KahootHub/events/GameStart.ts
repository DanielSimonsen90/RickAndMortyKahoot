import { getCurrentUser, navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('GameStart', (gameId) => {
  const currentUser = getCurrentUser();
  if (currentUser === null) return;

  if (currentUser.gameId === gameId) {
    navigate(`/Game/${gameId}/active`);
  }
});