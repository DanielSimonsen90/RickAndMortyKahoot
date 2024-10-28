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

  KahootHub.broadcast('CreateGame', user.Id, questionAmount ?? null);
});

$('#join-game').on('click', () => {
  openModal("join-game-modal");
});