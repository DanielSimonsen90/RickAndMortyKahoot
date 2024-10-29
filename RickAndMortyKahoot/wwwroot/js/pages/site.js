"use strict";
/**
 * Open a modal dialog. This function is called on event handlers by MVC Views.
 * @param id Id of the modal to open
 */
function openModal(id) {
    const modal = document.getElementById(id);
    modal.showModal();
}
/**
 * Close a modal dialog. This function is called on event handlers by MVC Views.
 * @param id Id of the modal to close
 */
function closeModal(id) {
    const modal = document.getElementById(id);
    modal.close();
}
