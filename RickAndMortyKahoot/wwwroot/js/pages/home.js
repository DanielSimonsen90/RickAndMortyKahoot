/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@types/jquery/index.d.ts" />
import KahootHub from "../KahootHub/index.js";
import { SESSION_STORAGE_KEYS } from "../constants.js";
import { saveToSessionStorage } from "../utils.js";
// Handler for creating a game
$('#create-game-form').on('submit', function (e) {
    // Stop and prevent default form behavior
    e.preventDefault();
    e.stopImmediatePropagation();
    const json = $("#user-data").val();
    if (!(typeof json === 'string'))
        return alert('Invalid JSON');
    const user = JSON.parse(json);
    const questionAmount = $("#question-amount").val();
    if (questionAmount !== undefined && isNaN(parseInt(questionAmount.toString())))
        return alert('Invalid question amount');
    // Broadcast the CreateGame action
    KahootHub.broadcast('CreateGame', user.id, parseInt(questionAmount.toString()) ?? null);
    // Disable the button and change the text to indicate that the game is being created
    if (this instanceof HTMLButtonElement) {
        this.disabled = true;
        this.innerHTML = "Creating your game...";
    }
});
// Handler for joining a game
$('#join-game-form').on('submit', (e) => {
    // Stop and prevent default form behavior
    e.preventDefault();
    const [userId, inviteCode] = ["UserId", "InviteCode"].map(name => $(`input[name=${name}]`).val());
    if (!(typeof userId === 'string'))
        return alert('Invalid user id');
    if (!(typeof inviteCode === 'string'))
        return alert('Invalid invite code');
    // Broadcast the Connect action
    KahootHub.broadcast('Connect', userId, inviteCode);
});
// Clear the user data from the session storage since we're on the home page
saveToSessionStorage('user', undefined);
saveToSessionStorage('isHost', undefined);
// Get and save the user data from the hidden input field, if it exists
const userJson = $('#user-data').val();
if (typeof userJson === 'string') {
    sessionStorage.setItem(SESSION_STORAGE_KEYS.USER, userJson);
    KahootHub.start();
}
