import CreateEvent from "./_Construction/CreateEvent.js";
export default CreateEvent('Error', (action, error) => {
    console.error(`[KahootHub Error] Action "${action}" failed.`, error);
});
