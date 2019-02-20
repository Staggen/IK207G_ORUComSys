$(document).ready(() => {
    console.log("Script loaded: forum-comments.js");
});

$("#postWall").on("keyup", ".commentTextArea", AdjustTextArea);
$("#postWall").on("click", ".comment-button", AddComment);

function AdjustTextArea(id) {
    if (typeof (id) == "object") { // Actively adjust size as one is typing.
        id.currentTarget.style.height = "1px";
        id.currentTarget.style.height = (id.currentTarget.scrollHeight) + "px";
    } else if (typeof (id) == "number") { // Reset size on submitting comment and clearing the box.
        var activeElement = document.getElementById(("comment-area-" + id));
        activeElement.style.height = "1px";
        activeElement.style.height = (activeElement.scrollHeight) + "px";
    }
}

function AddComment() {
    var postId = $(this).data("post-id");
    var content = $("#comment-area-" + postId).val();
    var comment = { PostId: postId, Content: content };
    if (content != "") {
        $.ajax({
            type: "POST",
            url: "/Forum/AddComment/",
            data: JSON.stringify(comment),
            contentType: "application/json;charset=UTF-8",
            success: function (data) {
                if (data.result) {
                    Update_Comment_Section(postId);
                    ClearCommentBox(postId);
                } else {
                    console.log("Controller Error: Unable to add comment!");
                }
            },
            error: () => {
                console.log("Error: Unable to add comment!");
            }
        });
    }
}

function ClearCommentBox(id) {
    $("#comment-area-" + id).val("");
    AdjustTextArea(id);
}

function Update_Comment_Section(id) {
    var serviceUrl = "/Forum/GetAllCommentsByPostId/" + id;
    var request = $.post(serviceUrl);
    request.done(function (data) {
        $("#comment-section-" + id).html(data);
    }).fail(() => {
        console.log("Error: Failure to update comment section!");
    });
}