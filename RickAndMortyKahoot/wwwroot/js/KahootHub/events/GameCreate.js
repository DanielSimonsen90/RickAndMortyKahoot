import { navigate } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('GameCreate', game => {
    if (!localStorage.getItem('user'))
        return alert('You need to be logged in to create a game');
    const user = JSON.parse(localStorage.getItem('user'));
    navigate(`/Game/${game.id}`);
});
