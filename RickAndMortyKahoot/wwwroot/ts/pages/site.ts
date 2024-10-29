/**
 * Open a modal dialog. This function is called on event handlers by MVC Views.
 * @param id Id of the modal to open
 */
function openModal(id: string) {
  const modal = document.getElementById(id) as HTMLDialogElement;
  modal.showModal();
}

/**
 * Close a modal dialog. This function is called on event handlers by MVC Views.
 * @param id Id of the modal to close
 */
function closeModal(id: string) {
  const modal = document.getElementById(id) as HTMLDialogElement;
  modal.close();
}
