import { ANSWER_TIMEOUT_SECONDS } from "../../constants.js";
import { Answer } from "../../models/Answer.js";
import { timer, getCurrentUser, getFromSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";

/**
 * When a new question is asked, this event is triggered.
 */
export default CreateEvent('NewQuestion', (gameId, question) => {
  const currentUser = getCurrentUser();
  if (currentUser === null) return;
  else if (currentUser.gameId !== gameId) return;

  // Get the question view from the server
  $.get(`${window.location.origin}/Game/QuestionView?gameId=${gameId}&questionId=${question.id}`, view => {
    // Set the html of the current question to the question view
    $('#current-question').html(view);

    // If the question title contains **, replace ** with <strong> tags
    if (question.title.includes("**")) {
      // replace ** in .question h1 with <strong> tags in .question h1
      let started = false;
      $('.question h1').html(question.title.replace(/\*\*/g, match => {
        started = !started;
        return started ? "<strong>" : "</strong>";
      }));
    }

    // Add click event to each choice
    $('.choice').on('click', function () {
      // When a choice is clicked, disable all other choices and add the selected class to the clicked choice
      const choices = this.closest('.choices')?.querySelectorAll('.choice')!;
      choices.forEach(choice => choice.setAttribute('disabled', 'true'));
      this.classList.add('selected');

      // Build the answer object to broadcast to the hub
      const index = +this.id;
      const answer: Answer = {
        questionId: question.id,
        userId: currentUser.id,
        index, timestamp: Date.now(),
      };

      // Broadcast the SubmitAnswer action to the hub
      window.KahootHub.broadcast('SubmitAnswer', gameId, answer);
    });

    // Start the timer for the round
    const isHost = getFromSessionStorage('isHost');
    timer({
      selector: '#round-timer',
      timeoutSeconds: ANSWER_TIMEOUT_SECONDS,
      showCritical: true,
      callback: () => {
        // When the timer runs out, and the user is the host, broadcast the EndRound action to the hub
        if (isHost) window.KahootHub.broadcast('EndRound', gameId, currentUser.id);
      },
    })
  });
});