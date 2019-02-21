$(document).ready(() => {
    console.log("Script loaded: profile.js");
});

$("#ProfilePageDiv").on("click", "#MakeAdmin", MakeAdmin);
$("#ProfilePageDiv").on("click", "#BanUser", ToggleSuspension);

function MakeAdmin() {
    var profileId = $(this).parents().eq(2).data("profile-id");
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

function ToggleSuspension() {
    var profileId = $(this).parents().eq(2).data("profile-id");
    var thisToPass = $(this);
    if ($(this).text() == "Ban User") {
        $.ajax({
            url: "/Profile/BanUser/" + profileId,
            type: "POST",
            dataType: "JSON",
            success: function (data) {
                if (data.result) {
                    SetBanButton(thisToPass);
                } else {
                    console.log("Controller Error: Unable to ban user");
                }
            },
            error: () => {
                console.log("Error: Unable to ban user.");
            }
        });
    } else if ($(this).text() == "Unban User") {
        $.ajax({
            url: "/Profile/UnbanUser/" + profileId,
            type: "POST",
            dataType: "JSON",
            success: function (data) {
                if (data.result) {
                    SetBanButton(thisToPass);
                } else {
                    console.log("Controller Error: Unable to unban user");
                }
            },
            error: () => {
                console.log("Error: Unable to unban user.");
            }
        });
    }
}

function SetBanButton(passedThis) {
    if ($(passedThis).text() == "Ban User") {
        $(passedThis).text("Unban User");
        if ($(passedThis).hasClass("btn-danger")) {
            $(passedThis).removeClass("btn-danger");
        }
        if (!$(passedThis).hasClass("btn-success")) {
            $(passedThis).addClass("btn-success");
        }
    } else {
        $(passedThis).text("Ban User");
        if ($(passedThis).hasClass("btn-success")) {
            $(passedThis).removeClass("btn-success");
        }
        if (!$(passedThis).hasClass("btn-danger")) {
            $(passedThis).addClass("btn-danger");
        }
    }
}