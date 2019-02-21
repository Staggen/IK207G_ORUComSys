$(document).ready(() => {
    console.log("Script loaded: meeting-create.js");
});

$("body").on("change", "#meeting-type-btn", SetSubmitButton);

function SetSubmitButton() {
    if ($("#meeting-type-btn").val() == 0) {
        $("#meeting-submit-btn").val("Create Meeting");
    } else {
        $("#meeting-submit-btn").val("Invite People");
    }
}