$(document).ready(() => {
    console.log("Script loaded: profile-makeAdmin.js");
});


$("body").on("click", "#MakeAdmin", makeAdmin);

function makeAdmin() {
    console.log("click");

    var currentUrl = window.location.href;
    var urlArray = currentUrl.split("/Profile/Index/");
    var profileId = urlArray[1];
    console.log("")
    
    var userIdToMakeAdmin = profileId;
    var serviceUrl = "/FormalPost/MakeAdmin/" + userIdToMakeAdmin;
    console.log("variable created");
    $.ajax({
        url: serviceUrl,
        type: "POST",
        dataType: "JSON",
        success: () => {
            console.log("Success!");
            $("#MakeAdmin").addClass("d-none");
        }
    });
}