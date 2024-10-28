function openModal(id) {
  /** @type {HTMLDialogElement} */
  const modal = document.getElementById(id);

  modal.showModal();
}

function closeModal(id) {
  /** @type {HTMLDialogElement} */
  const modal = document.getElementById(id);

  modal.close();
}