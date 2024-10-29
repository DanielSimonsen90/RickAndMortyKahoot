import { ROUND_END_TIMEOUT_SECONDS } from "../../constants.js";
import { getCurrentUser, getFromSessionStorage, timer } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('RoundEnd', (gameId, answer, score, question) => {
  const currentUser = getCurrentUser();
  if (currentUser === null || currentUser.gameId !== gameId) return;

  $.post({
    url: `${window.location.origin}/Game/CorrectAnswer?gameId=${gameId}&userId=${currentUser.id}`,
    data: JSON.stringify({ 
      answerIndex: answer.index, 
      score, 
    }),
    contentType: 'application/json',
  }).done(view => {
    $('#correct-answer').html(view);
    $(`.choice#${answer.index}`).addClass('correct');
    $('.choice').attr('disabled', 'true');

    const isHost = getFromSessionStorage('isHost');
    if (isHost) clearTimeout(window.roundTimeout);

    timer({
      timeoutSeconds: ROUND_END_TIMEOUT_SECONDS,
      selector: '#round-timer-end',
      callback: () => {
        if (isHost) window.KahootHub.broadcast('NextQuestion', gameId, currentUser.id);
      },
    });
  });
});