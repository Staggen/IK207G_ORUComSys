$(document).ready(() => {
    console.log("Script loaded: profile.js");
});

$("#ProfilePageDiv").on("click", "#MakeAdmin", MakeAdmin);

function MakeAdmin() {
    var currentUrl = window.location.href;
    var urlArray = currentUrl.split("/Profile/Index/");
    var profileId = urlArray[1];

    $.ajax({
        url: "/Profile/MakeAdmin/" + profileId,
        type: "POST",
        success: () => {
            if (!$("#MakeAdmin").hasClass("d-none")) {
                $("#MakeAdmin").addClass("d-none");
            }
            console.log("MakeAdmin() => Success!");
        },
        error: () => {
            console.log("Error: Unable to make user admin.");
        }
    });
}