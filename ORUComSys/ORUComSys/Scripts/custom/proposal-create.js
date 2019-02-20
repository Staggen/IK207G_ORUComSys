$(document).ready(() => {
    console.log("Script loaded: proposal-create.js");
});

$(".first-time").on("click", "#add-second-time", ToggleSecondTime);
$(".second-time").on("click", "#add-third-time", ToggleThirdTime);
$(".third-time").on("click", "#show-all", ToggleAllTimes);

function ToggleFirstTime() {
    if (!$("#first-time").hasClass("d-none")) {
        $("#first-time").addClass("d-none");
    } else {
        $("#first-time").toggleClass("d-none");
    }
}

function ToggleSecondTime() {
    $("#second-time").toggleClass("d-none");
    ToggleFirstTime();
}

function ToggleThirdTime() {
    $("#third-time").toggleClass("d-none");
    ToggleSecondTime();
}

function ToggleAllTimes() {
    $("#first-time").toggleClass("d-none");
    $("#second-time").toggleClass("d-none");
}