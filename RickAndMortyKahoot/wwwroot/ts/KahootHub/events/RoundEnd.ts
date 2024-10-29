import { ROUND_END_TIMEOUT_SECONDS } from "../../constants.js";
import { getCurrentUser, getFromSessionStorage, timer } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

/**
 * When a round ends, this event is triggered.
 */
export default CreateEvent('RoundEnd', (gameId, answer, score, question) => {
  const currentUser = getCurrentUser();
  if (currentUser === null || currentUser.gameId !== gameId) return;

  // Send round-end data to server to recieve CorrectAnswer view
  $.post({
    url: `${window.location.origin}/Game/CorrectAnswer?gameId=${gameId}&userId=${currentUser.id}`,
    data: JSON.stringify({ 
      answerIndex: answer.index, 
      score, 
    }),
    contentType: 'application/json',
  }).done(view => {
    // Set the html and visualize the correct answer
    $('#correct-answer').html(view);
    $(`.choice#${answer.index}`).addClass('correct');
    $('.choice').attr('disabled', 'true');

    // If the user is host, stop the timer for round timeout
    const isHost = getFromSessionStorage('isHost');
    if (isHost) clearTimeout(window.roundTimeout);

    // Start the timer for the round end
    timer({
      timeoutSeconds: ROUND_END_TIMEOUT_SECONDS,
      selector: '#round-timer-end',
      callback: () => {
        // If the user is host, broadcast the NextQuestion action to the hub
        if (isHost) window.KahootHub.broadcast('NextQuestion', gameId, currentUser.id);
      },
    });
  });
});