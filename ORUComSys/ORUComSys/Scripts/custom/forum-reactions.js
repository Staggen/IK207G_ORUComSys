$(document).ready(() => {
    console.log("Script loaded: forum-reactions.js");
});

$("#postWall").on("click", "#like-button", addReaction);
$("#postWall").on("click", "#love-button", addReaction);
$("#postWall").on("click", "#hate-button", addReaction);
$("#postWall").on("click", "#xd-button", addReaction);
$("#postWall").on("click", ".reaction-count", toggleReactionList);
$("#postWall").on("click", ".react-button", toggleReactions);

function toggleReactions() {
    var btnId = this.getAttribute("Id");
    console.log(btnId);
    $("#reaction-popup").toggleClass("d-none");
    var ref = $("*[id='" + btnId + "']");
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
    hideReactionList();
}

function addReaction() {
    var postId = this.getAttribute("data-post-id");
    var reactionType = this.getAttribute("name");

    reaction = { PostId: postId, Reaction: reactionType };

    $.ajax({
        type: "POST",
        url: "/api/AjaxApi/",
        data: JSON.stringify(reaction),
        contentType: "application/json;charset=UTF-8",
        success: () => {
            Update_Posts(forumType);
            console.log("Added reaction!");
        },
        error: () => {
            alert("Error: Failure to add reaction");
        }
    });
    $("#reaction-popup").toggleClass("d-none");

}

function toggleReactionList() {
    for (var i = 0; i < 2; i++) {
        var postId = this.getAttribute("data-post-id");
        var serviceUrl = "/Forum/GetReactionList/" + postId;
        var request = $.post(serviceUrl);
        request.done(function (data) {
            $("#reaction-list-popup").html(data);
        }).fail(() => {
            console.log("Error: Failure to display reaction information");
        });
        var ref = this;
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
    }
    $("#reaction-list-popup").toggleClass("d-none");
    hideReactions();
}

function hideReactions() {
    if (!$("#reaction-popup").hasClass("d-none")) {
        $("#reaction-popup").addClass("d-none");
    }
}

function hideReactionList() {
    if (!$("#reaction-list-popup").hasClass("d-none")) {
        $("#reaction-list-popup").addClass("d-none");
    }
}