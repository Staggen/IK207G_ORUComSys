$(document).ready(() => {
    console.log("Script loaded: forum-comments.js");
});

$("#postWall").on("keyup", ".commentTextArea", AdjustTextArea);

function AdjustTextArea() {
    this.style.height = "1px";
    this.style.height = (this.scrollHeight) + "px";
}