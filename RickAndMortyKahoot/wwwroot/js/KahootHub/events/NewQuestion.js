import { ANSWER_TIMEOUT_SECONDS } from "../../constants.js";
import { getCurrentUser, getFromSessionStorage } from "../../utils.js";
import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('NewQuestion', (gameId, question) => {
    const currentUser = getCurrentUser();
    if (currentUser === null)
        return;
    else if (currentUser.gameId !== gameId)
        return;
    $.get(`${window.location.origin}/Game/QuestionView?gameId=${gameId}&questionId=${question.id}`, view => {
        $('#current-question').html(view);
        $('.choices li').on('click', function () {
            const index = +this.id;
            const answer = {
                questionId: question.id,
                userId: currentUser.id,
                index, timestamp: Date.now(),
            };
            window.KahootHub.broadcast('SubmitAnswer', gameId, answer);
        });
        const isHost = getFromSessionStorage('isHost');
        if (isHost)
            setTimeout(() => {
                window.KahootHub.broadcast('EndRound', gameId, currentUser.id);
            }, ANSWER_TIMEOUT_SECONDS * 1000);
    });
});
