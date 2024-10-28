/// <reference path="C:/Users/Ejer/AppData/Roaming/npm/node_modules/@types/jquery/index.d.ts" />
import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('RecieveMessage', (username, message) => {
  $('#discussion').append(`
    <p>
      <strong>${username}</strong>: ${message}
    </p>  
  `);
})