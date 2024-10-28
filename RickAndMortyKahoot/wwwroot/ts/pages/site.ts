function openModal(id: string) {
  const modal = document.getElementById(id) as HTMLDialogElement;
  modal.showModal();
}

function closeModal(id: string) {
  const modal = document.getElementById(id) as HTMLDialogElement;
  modal.close();
}
