import { ANSWER_TIMEOUT_SECONDS } from "../../constants.js";
import { Answer } from "../../models/Answer.js";
import { timer, getCurrentUser, getFromSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

export default CreateEvent('NewQuestion', (gameId, question) => {
  const currentUser = getCurrentUser();
  if (currentUser === null) return;
  else if (currentUser.gameId !== gameId) return;

  $.get(`${window.location.origin}/Game/QuestionView?gameId=${gameId}&questionId=${question.id}`, view => {
    $('#current-question').html(view);
    if (question.title.includes("**")) {
      // replace ** in .question h1 with <strong> tags in .question h1
      let started = false;
      $('.question h1').html(question.title.replace(/\*\*/g, match => {
        started = !started;
        return started ? "<strong>" : "</strong>";
      }));
    }

    $('.choice').on('click', function () {
      const choices = this.closest('.choices')?.querySelectorAll('.choice')!;
      choices.forEach(choice => choice.setAttribute('disabled', 'true'));
      this.classList.add('selected');

      const index = +this.id;
      const answer: Answer = {
        questionId: question.id,
        userId: currentUser.id,
        index, timestamp: Date.now(),
      };

      window.KahootHub.broadcast('SubmitAnswer', gameId, answer);
    });

    
    const isHost = getFromSessionStorage('isHost');
    timer({
      timeoutSeconds: ANSWER_TIMEOUT_SECONDS,
      showCritical: true,
      callback: () => {
        if (isHost) window.KahootHub.broadcast('EndRound', gameId, currentUser.id);
      },
    })
  });
});