$(document).ready(() => {
    console.log("Script loaded: profile.js");
});

$("#ProfilePageDiv").on("click", "#ToggleAdmin", ToggleAdmin);
$("#ProfilePageDiv").on("click", "#ToggleBanned", ToggleSuspension);

function ToggleAdmin() {
    var profileId = $(this).parents().eq(2).data("profile-id");
    var thisToPass = $(this); // Required as "this" changes throghout the $.ajax
    if ($(this).text() == "Make Admin") {
        $.ajax({
            url: "/Profile/MakeAdmin/" + profileId,
            type: "POST",
            dataType: "JSON",
            success: function (data) {
                if (data.result) {
                    SetAdminButton(thisToPass);
                } else {
                    console.log("Controller Error: Unable to make user admin.");
                }
            },
            error: () => {
                console.log("Error: Unable to make user admin.");
            }
        });
    } else if ($(this).text() == "Remove Admin") {
        $.ajax({
            url: "/Profile/RemoveAdmin/" + profileId,
            type: "POST",
            dataType: "JSON",
            success: function (data) {
                if (data.result) {
                    SetAdminButton(thisToPass);
                } else {
                    console.log("Controller Error: Unable to remove user from admins.");
                }
            },
            error: () => {
                console.log("Error: Unable to remove user from admins.");
            }
        });
    }
}

function ToggleSuspension() {
    var profileId = $(this).parents().eq(2).data("profile-id");
    var thisToPass = $(this); // Required as "this" changes throghout the $.ajax
    if ($(this).text() == "Ban User") {
        $.ajax({
            url: "/Profile/BanUser/" + profileId,
            type: "POST",
            dataType: "JSON",
            success: function (data) {
                if (data.result) {
                    SetBannedButton(thisToPass);
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
                    SetBannedButton(thisToPass);
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

function SetAdminButton(passedThis) {
    if ($(passedThis).text() == "Make Admin") {
        if ($(passedThis).hasClass("btn-success")) {
            $(passedThis).removeClass("btn-success");
        }
        if (!$(passedThis).hasClass("btn-danger")) {
            $(passedThis).addClass("btn-danger");
        }
        $(passedThis).text("Remove Admin");
    } else {
        if ($(passedThis).hasClass("btn-danger")) {
            $(passedThis).removeClass("btn-danger");
        }
        if (!$(passedThis).hasClass("btn-success")) {
            $(passedThis).addClass("btn-success");
        }
        $(passedThis).text("Make Admin");
    }
}

function SetBannedButton(passedThis) {
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