import CreateEvent from "./_Construction/CreateEvent.js";
/**
 * When an error occurs, this event is triggered.
 */
export default CreateEvent('Error', (action, error) => {
    console.error(`[KahootHub Error] Action "${action}" failed.`, error);
    const message = (() => {
        // From string "RickAndMortyKahoot.Models.Exceptions.Users.UserAlreadyOwnsGameException' was thrown.", regex last part
        const split = error.message.split('.');
        const exception = split[split.length - 2].replace("'", '').split(' ').shift();
        switch (exception) {
            // Game
            case 'AllQuestionsAnsweredException': return 'All questions have been answered.';
            case 'InvalidGameException': return "Couldn't find the game matching the provided ID.";
            case 'InvalidGameStateException': return 'The game is in an invalid state (probably not active).';
            case 'NotHostException': return 'You are not the host of this game and cannot perform that action.';
            // User
            case 'InvalidUserException': return "Couldn't find the user matching the provided ID.";
            case 'UserAlreadyConnectedException': return 'You are already connected to this game.';
            case 'UserAlreadyOwnsGameException': return 'You already own a game.';
            default: return exception;
        }
    })();
    alert(message);
});
