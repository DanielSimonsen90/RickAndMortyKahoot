/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@types/jquery/index.d.ts" />
import KahootHub from "../KahootHub/index.js";
import { LOCAL_STORAGE_KEYS } from "../constants.js";
$('#create-game').on('click', function () {
    const json = $("#user-data").val();
    if (!(typeof json === 'string'))
        return alert('Invalid JSON');
    const user = JSON.parse(json);
    const questionAmount = $("#question-amount").val();
    if (questionAmount !== undefined && !(typeof questionAmount === 'number'))
        return alert('Invalid question amount');
    KahootHub.broadcast('CreateGame', user.id, questionAmount ?? null);
    if (this instanceof HTMLButtonElement) {
        this.disabled = true;
        this.innerHTML = "Creating your game...";
    }
});
$('#join-game').on('click', () => {
    openModal("join-game-modal");
});
$('#join-game-form').on('submit', (e) => {
    e.preventDefault();
    const [userId, inviteCode] = ["UserId", "InviteCode"].map(name => $(`input[name=${name}]`).val());
    if (!(typeof userId === 'string'))
        return alert('Invalid user id');
    if (!(typeof inviteCode === 'string'))
        return alert('Invalid invite code');
    KahootHub.broadcast('Connect', userId, inviteCode);
});
const userJson = $('#user-data').val();
if (typeof userJson === 'string') {
    sessionStorage.setItem(LOCAL_STORAGE_KEYS.USER, userJson);
    KahootHub.start();
}
