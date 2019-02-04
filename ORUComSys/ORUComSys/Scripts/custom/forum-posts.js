$(document).ready(() => {
    console.log("Script loaded: forum-posts.js");
    var currentUrl = window.location.href;
    var urlArray = currentUrl.split("/Forum/");
    Id = urlArray[1].split("/")[0];
});
var Id;

$("#postSection").on("click", "#SubmitPost", AddPost);
$("#postSection").on("click", ".deleteBtn", DeletePost);
$("#postSection").on("keyup", "#PostText", AdjustCounter);

function AdjustCounter() {
    var number = $("#PostText").val().length;
    if (number <= 0) {
        if ($("#CreatePostCard").hasClass("border-success")) {
            $("#CreatePostCard").removeClass("border-success");
        }
        if (!$("#CreatePostCard").hasClass("border-danger")) {
            $("#CreatePostCard").addClass("border-danger");
        }
    }
    else if (number < 250) {
        if ($("#TextAreaWordCounter").hasClass("badge-danger")) {
            $("#TextAreaWordCounter").removeClass("badge-danger");
        }
        if ($("#TextAreaWordCounter").hasClass("badge-warning")) {
            $("#TextAreaWordCounter").removeClass("badge-warning");
        }
        if (!$("#TextAreaWordCounter").hasClass("badge-success")) {
            $("#TextAreaWordCounter").addClass("badge-success");
        }
        if ($("#CreatePostCard").hasClass("border-danger")) {
            $("#CreatePostCard").removeClass("border-danger");
        }
        if (!$("#CreatePostCard").hasClass("border-success")) {
            $("#CreatePostCard").addClass("border-success");
        }
    }
    if (number >= 250) {
        if ($("#TextAreaWordCounter").hasClass("badge-danger")) {
            $("#TextAreaWordCounter").removeClass("badge-danger");
        }
        if ($("#TextAreaWordCounter").hasClass("badge-success")) {
            $("#TextAreaWordCounter").removeClass("badge-success");
        }
        if (!$("#TextAreaWordCounter").hasClass("badge-warning")) {
            $("#TextAreaWordCounter").addClass("badge-warning");
        }
        if ($("#CreatePostCard").hasClass("border-danger")) {
            $("#CreatePostCard").removeClass("border-danger");
        }
        if (!$("#CreatePostCard").hasClass("border-success")) {
            $("#CreatePostCard").addClass("border-success");
        }
    }
    if (number > 280) {
        if ($("#TextAreaWordCounter").hasClass("badge-warning")) {
            $("#TextAreaWordCounter").removeClass("badge-warning");
        }
        if ($("#TextAreaWordCounter").hasClass("badge-success")) {
            $("#TextAreaWordCounter").removeClass("badge-success");
        }
        if (!$("#TextAreaWordCounter").hasClass("badge-danger")) {
            $("#TextAreaWordCounter").addClass("badge-danger");
        }
        if ($("#CreatePostCard").hasClass("border-success")) {
            $("#CreatePostCard").removeClass("border-success");
        }
        if (!$("#CreatePostCard").hasClass("border-danger")) {
            $("#CreatePostCard").addClass("border-danger");
        }
    }
    $("#TextAreaWordCounter").text(280 - number);
}

function AddPost() {
    if ($("#PostText").val() != "" && $("#PostText").val().length <= 280) {

        post = { Content: $("#PostText").val(), Forum: Id };

        $.ajax({
                type: "POST",
                url: "/api/AjaxApi/AddPost",
                data: JSON.stringify(post),
                contentType: "application/json;charset=UTF-8",
                success: () => {
                    Update_Posts(Id);
                },
                error: () => {
                    alert("Error: Failure to add post");
                }
            });
        } else {
            alert("Your post needs to consist of between 1 and 280 characters.")
        }
    }

    function DeletePost() {
        var PostId = this.getAttribute("data-post-id");

        $.ajax({
            type: "DELETE",
            url: "/api/AjaxApi/" + PostId,
            contentType: "application/json;charset=UTF-8",
            success: () => {
                Update_Posts(Id);
            },
            error: () => {
                alert("Error: Failure to delete post");
            }
        });
        if ($("body").hasClass("modal-open")) {
            $("body").toggleClass("modal-open");
        }
        $(".modal-backdrop").remove(); // To remove the stuff behind the modal after it closes.
    }

    function Update_Posts(type) {
        var serviceUrl = "/Forum/UpdatePosts/" + type;
        var request = $.post(serviceUrl);
        request.done(function (data) {
            $("#postSection").html(data);
        }).fail(() => {
            console.log("Error: Failure to update posts");
        });
    }