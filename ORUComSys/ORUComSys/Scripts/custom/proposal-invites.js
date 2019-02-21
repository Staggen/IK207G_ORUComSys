$(document).ready(() => {
    console.log("Script loaded: proposal-invites.js");
});

$("#SearchField").on("keyup", SearchUsers);
$(".invite-button").on("click", InviteUser);

function SearchUsers() {
    var searchString = $("#SearchField").val();

    // Make an array of all users displayed on the page
    var users = document.getElementsByClassName("personal-info");
    var userArray = jQuery.makeArray(users);

    // Remove if name doesn't contain search string
    for (var i = 0; i < userArray.length; i++) {
        if (!userArray[i].innerHTML.toString().toUpperCase().includes(searchString.toUpperCase())) { // Convert to uppercase to avoid case sensitivity
            // Get the card element of the users whose names do not include the search string.
            if (!$(userArray[i]).parents().eq(4).hasClass("d-none")) {
                $(userArray[i]).parents().eq(4).addClass("d-none");
            };
        }
    }

    // Show currently hidden results if they contain search string. Same as a
    for (var i = 0; i < userArray.length; i++) {
        if (userArray[i].innerHTML.toString().toUpperCase().includes(searchString.toUpperCase())) {
            if ($(userArray[i]).parents().eq(4).hasClass("d-none")) {
                $(userArray[i]).parents().eq(4).removeClass("d-none");
            };
        }
    }
}

function InviteUser() {
    var UserId = $(this).attr("id");
    var proposalId = $(this).data("proposal-id");
    var action = $(this).text();

    if (action == "Invite") {
        SendInvite(UserId, proposalId);
    } else if (action == "Remove") {
        RemoveInvite(UserId, proposalId);
    }
}

function SendInvite(UserId, proposalId) {
    var invite = { ProposalId: proposalId, ProfileId: UserId };
    $.ajax({
        url: "/Proposal/AddProposalInvite/",
        type: "POST",
        data: JSON.stringify(invite),
        contentType: "application/json;charset=UTF-8",
        success: function (data) {
            if (data.result) { // If invite successfully sent in controller
                SetBannedButton(UserId);
            } else {
                console.log("Controller Error: Unable to send invite.");
            }
        },
        error: () => {
            console.log("Error: Unable to send invite.");
        }
    });
}

function RemoveInvite(UserId, proposalId) {
    console.log("Remove Invite");
    var invite = { ProposalId: proposalId, ProfileId: UserId };
    $.ajax({
        url: "/Proposal/RemoveProposalInvite/",
        type: "POST",
        data: JSON.stringify(invite),
        contentType: "application/json;charset=UTF-8",
        success: function (data) {
            if (data.result) { // If invite successfully removed in controller
                SetBannedButton(UserId);
            } else {
                console.log("Controller Error: Unable to remove invite.");
            }
        },
        error: () => {
            console.log("Error: Unable to remove invite.");
        }
    });
}

function SetBannedButton(UserId) {
    var SelectedButton = document.getElementById(UserId);
    if ($(SelectedButton).text() == "Invite") {
        $(SelectedButton).text("Remove");
        if ($(SelectedButton).hasClass("btn-success")) {
            $(SelectedButton).removeClass("btn-success");
        }
        if (!$(SelectedButton).hasClass("btn-danger")) {
            $(SelectedButton).addClass("btn-danger");
        }
    } else if ($(SelectedButton).text() == "Remove") {
        $(SelectedButton).text("Invite");

        if ($(SelectedButton).hasClass("btn-danger")) {
            $(SelectedButton).removeClass("btn-danger");
        }
        if (!$(SelectedButton).hasClass("btn-success")) {
            $(SelectedButton).addClass("btn-success");
        }
    }
}