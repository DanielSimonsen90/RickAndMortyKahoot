/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@types/jquery/index.d.ts" />
import KahootHub from "./signalr/KahootHub/index.js";
KahootHub.start().then(() => {
    console.log('KahootHub started');
});
$('#sendmessage').on('click', () => {
    const displayName = $("#displayname").val().toString();
    const message = $("#message").val()?.toString();
    if (!message)
        return alert('Message cannot be empty');
    KahootHub.broadcast('SendMessage', displayName, message);
});
