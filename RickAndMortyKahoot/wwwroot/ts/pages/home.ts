/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@types/jquery/index.d.ts" />

import { User } from "../models/User.js";
import KahootHub from "../KahootHub/index.js";
import { LOCAL_STORAGE_KEYS } from "../constants.js";

$('#create-game-form').on('submit', function(e) {
  e.preventDefault();
  e.stopImmediatePropagation();

  const json = $("#user-data").val();
  if (!(typeof json === 'string')) return alert('Invalid JSON');
  const user: User = JSON.parse(json);

  const questionAmount = $("#question-amount").val();
  if (questionAmount !== undefined && isNaN(parseInt(questionAmount.toString()))) return alert('Invalid question amount');

  KahootHub.broadcast('CreateGame', user.id, parseInt(questionAmount!.toString()) ?? null);
  if (this instanceof HTMLButtonElement) {
    this.disabled = true;
    this.innerHTML = "Creating your game...";
  }
});

$('#join-game-form').on('submit', (e) => {
  e.preventDefault();

  const [userId, inviteCode] = ["UserId", "InviteCode"].map(name => $(`input[name=${name}]`).val());
  if (!(typeof userId === 'string')) return alert('Invalid user id');
  if (!(typeof inviteCode === 'string')) return alert('Invalid invite code');

  KahootHub.broadcast('Connect', userId, inviteCode);
});

const userJson = $('#user-data').val();
if (typeof userJson === 'string') {
  sessionStorage.setItem(LOCAL_STORAGE_KEYS.USER, userJson);
  
  KahootHub.start();
}