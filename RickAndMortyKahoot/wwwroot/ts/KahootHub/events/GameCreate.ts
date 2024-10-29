import { User } from "../../models/User.js";
import { navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('GameCreate', (gameId, game) => {
  if (!localStorage.getItem('user')) return alert('You need to be logged in to create a game');

  const user: User = JSON.parse(localStorage.getItem('user')!);
  navigate(`/Game/${game.id}`);
})