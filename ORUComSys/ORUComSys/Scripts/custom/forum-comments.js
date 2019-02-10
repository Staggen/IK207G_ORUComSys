$(document).ready(() => {
    console.log("Script loaded: forum-comments.js");
});

$("#postSection").on("keyup", ".commentTextArea", AdjustTextArea);

function AdjustTextArea() {
    this.style.height = "1px";
    this.style.height = (this.scrollHeight) + "px";
}