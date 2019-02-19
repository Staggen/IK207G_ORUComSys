$(document).ready(() => {
    console.log("Script loaded: forum-reactions.js");
});

$("#postWall").on("click", ".like-button", addReaction);
$("#postWall").on("click", ".love-button", addReaction);
$("#postWall").on("click", ".hate-button", addReaction);
$("#postWall").on("click", ".xd-button", addReaction);
$("#postWall").on("click", ".reaction-count", ToggleReactionListDiv);
$("#postWall").on("click", ".react-button", ToggleReactionDiv);

var interactingWithPostId;

function ToggleReactionDiv() {
    if ($(this).hasClass("focus")) {
        $(this).removeClass("focus");
    }
    interactingWithPostId = this.getAttribute("data-post-id");
    if ($("#reaction-popup").hasClass("d-none")) {
        CreateReactionPopper(this);
    } else {
        ToggleReactionDisplay();
    }
}

function CreateReactionPopper(passedThis) {
    var ref = passedThis;
    var pop = $("#reaction-popup");
    new Popper(ref, pop, {
        placement: "top",
        modifiers: {
            offset: {
                enabled: true,
                offset: "0, 10"
            }
        }
    });
    if (!$("#reaction-list-popup").hasClass("d-none")) {
        $("#reaction-list-popup").addClass("d-none");
    }
    ToggleReactionDisplay();
}

function addReaction() {
    var reactionType = this.getAttribute("name");
    reaction = { PostId: interactingWithPostId, Reaction: reactionType };
    $.ajax({
        type: "POST",
        url: "/api/AjaxApi/",
        data: JSON.stringify(reaction),
        contentType: "application/json;charset=UTF-8",
        success: () => {
            Update_Posts(forumType);
        },
        error: () => {
            console.log("Error: Failure to add reaction");
        }
    });
    ToggleReactionDisplay();
}

function ToggleReactionListDiv() {
    if ($(this).hasClass("focus")) {
        $(this).removeClass("focus");
    }
    if ($("#reaction-list-popup").hasClass("d-none")) {
        var postId = this.getAttribute("data-post-id");
        GetReactionListContent(postId);
    } else {
        ToggleReactionListDisplay();
    }
}

function GetReactionListContent(postId, passedThis) {
    var serviceUrl = "/Forum/GetReactionList/" + postId;
    var request = $.post(serviceUrl);
    request.done(function (data) {
        $("#reaction-list-popup").html(data);
        CreateReactionListPopper(passedThis);
    }).fail(() => {
        console.log("Error: Failure to display reaction information");
    });
}

function CreateReactionListPopper(passedThis) {
    var ref = passedThis;
    var pop = $("#reaction-list-popup");
    new Popper(ref, pop, {
        placement: "top",
        modifiers: {
            offset: {
                enabled: true,
                offset: "0, 10"
            }
        }
    });
    if (!$("#reaction-popup").hasClass("d-none")) {
        $("#reaction-popup").addClass("d-none");
    }
    ToggleReactionListDisplay();
}

function ToggleReactionDisplay() {
    $("#reaction-popup").toggleClass("d-none");
}

function ToggleReactionListDisplay() {
    $("#reaction-list-popup").toggleClass("d-none");
}