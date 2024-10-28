import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('GameCreate', game => {
    // $.ajax({
    //   type: "POST",
    //   url: "/Game",
    //   data: JSON.stringify(game),
    //   contentType: "application/json; charset=utf-8",
    //   dataType: "json",
    //   success: function (data, text, xhr) {
    //     if (xhr.status === 200) window.location.href = `/Game/${game.Id}`;
    //   },
    //   error: function (data, error) {
    //     if (error) console.error(error);
    //   }
    // });
    window.location.href = `/Game/${game.id}`;
});
