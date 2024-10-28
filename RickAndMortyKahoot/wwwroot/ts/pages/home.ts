/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@types/jquery/index.d.ts" />

import { User } from "../models/User.js";
import KahootHub from "../KahootHub/index.js";

KahootHub.start().then(() => {
  console.log('KahootHub started');
});

$('#create-game').on('click', () => {
  const json = $("#user-data").val();
  if (!(typeof json === 'string')) return alert('Invalid JSON');
  const user: User = JSON.parse(json);

  const questionAmount = $("#question-amount").val();
  if (questionAmount !== undefined && !(typeof questionAmount === 'number')) return alert('Invalid question amount');

  KahootHub.broadcast('CreateGame', user.id, questionAmount ?? null);
});

$('#join-game').on('click', () => {
  openModal("join-game-modal");
});

$('#join-game-form').on('submit', (e) => {
  e.preventDefault();

  const [userId, inviteCode] = ["UserId", "InviteCode"].map(name => $(`input[name=${name}]`).val());
  if (!(typeof userId === 'string')) return alert('Invalid user id');
  if (!(typeof inviteCode === 'string')) return alert('Invalid invite code');

  KahootHub.broadcast('Connect', userId, inviteCode);
});

const userJson = $('#user-data').val();
if (typeof userJson === 'string') localStorage.setItem('user', userJson);