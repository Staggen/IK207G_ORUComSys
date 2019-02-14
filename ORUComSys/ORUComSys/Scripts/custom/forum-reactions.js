$(document).ready(() => {
    console.log("Script loaded: forum-reactions.js");
});

$("#postWall").on("click", ".like-button", addReaction);
$("#postWall").on("click", ".love-button", addReaction);
$("#postWall").on("click", ".hate-button", addReaction);
$("#postWall").on("click", ".xd-button", addReaction);
$("#postWall").on("click", ".reaction-count", toggleReactionListStepOne);
$("#postWall").on("click", ".react-button", toggleReactions);

var interactingWithPostId;

function toggleReactions() {
    var postId = this.getAttribute("data-post-id");
    interactingWithPostId = postId;
    $("#reaction-popup").toggleClass("d-none");
    var ref = this;
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
    var reactionType = this.getAttribute("name");
    console.log("Reaction type: " + reactionType)

    reaction = { PostId: interactingWithPostId, Reaction: reactionType };

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

function toggleReactionListStepOne() { // Inexplicably extremely important to split this process into two steps
    var postId = this.getAttribute("data-post-id");
    toggleReactionList(postId, this);
}

function toggleReactionList(postId, reference) {
    var serviceUrl = "/Forum/GetReactionList/" + postId;
    var request = $.post(serviceUrl);
    request.done(function (data) {
        $("#reaction-list-popup").html(data);

        var ref = reference;
        var pop = $("#reaction-list-popup");
        // Popper creation in the request.done block, in combination with the two-step process, is necessary for
        // popper to appear in the correct position on the first click.
        new Popper(ref, pop, {
            placement: "top",
            modifiers: {
                offset: {
                    enabled: true,
                    offset: "0, 10"
                }
            }
        });
        $("#reaction-list-popup").toggleClass("d-none");
        hideReactions();
    }).fail(() => {
        console.log("Error: Failure to display reaction information");
    });

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