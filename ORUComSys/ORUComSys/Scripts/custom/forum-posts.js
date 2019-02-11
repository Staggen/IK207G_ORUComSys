$(document).ready(() => {
    console.log("Script loaded: forum-posts.js");
    var currentUrl = window.location.href;
    var urlArray = currentUrl.split("/Forum/");
    Id = urlArray[1].split("/")[0];
    Update_Posts(Id);
    $("#SubmitPost").prop('disabled', true);
    ClearBox();

});
var Id;

$("#postWall").on("click", ".deleteBtn", DeletePost);
$("#CreatePostCard").on("keyup", "#Content", AdjustCounter);
$("#postWall").on("click", "#like-button", addReaction);
$("#postWall").on("click", "#love-button", addReaction);
$("#postWall").on("click", "#hate-button", addReaction);
$("#postWall").on("click", "#xd-button", addReaction);
$("#postWall").on("click", ".reaction-count", toggleReactionList);
//$("#postWall").on("focusout", ".reaction-count", hideReactionList);
$("#postWall").on("click", ".react-button", toggleReactions);
//$("#postWall").on("focusout", ".react-button", hideReactions);
$(".filter-checkbox").change(filterPosts)

function AdjustCounter() {
    var number = $("#Content").val().length;
    if (number <= 0) {
        if ($("#CreatePostCard").hasClass("border-success")) {
            $("#CreatePostCard").removeClass("border-success");
        }
        if (!$("#CreatePostCard").hasClass("border-danger")) {
            $("#CreatePostCard").addClass("border-danger");
        }
        $("#SubmitPost").prop('disabled', true);
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
        $("#SubmitPost").prop('disabled', false);
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
        $("#SubmitPost").prop('disabled', false);
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
        $("#SubmitPost").prop('disabled', false);
    }
    $("#TextAreaWordCounter").text(280 - number);
}
function ClearBox() {
    $("#Content").val("");
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
        $("#postWall").html(data);
    }).fail(() => {
        console.log("Error: Failure to update posts");
    });
}

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
            Update_Posts(Id);
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

function filterPosts() {
    console.log(this.checked)
    var posts = document.getElementsByClassName("category-label");
    var postArray = jQuery.makeArray(posts);

    var categoryCheckboxes = document.getElementsByClassName("filter-checkbox");
    var categoryCheckboxArray = jQuery.makeArray(categoryCheckboxes);

    var checkedCategoryCheckboxArray = [];

    for (var i = 0; i < categoryCheckboxArray.length; i++) {
        if (categoryCheckboxArray[i].checked) {
            checkedCategoryCheckboxArray[i] = categoryCheckboxArray[i].getAttribute("name");
        }
    }

    console.log(this.getAttribute("name"));

    // Remove if post doesn't match checked categories
    for (var i = 0; i < postArray.length; i++) {
        console.log($(postArray[i]).parents().eq(2));
        if (!checkedCategoryCheckboxArray.includes(postArray[i].innerHTML)) { 
            if (!$(postArray[i]).parents().eq(2).hasClass("d-none")) {
                $(postArray[i]).parents().eq(2).addClass("d-none");
            };
        }
    }

    // Show currently hidden results if they contain search string. Same as a
    for (var i = 0; i < postArray.length; i++) {
        if (checkedCategoryCheckboxArray.includes(postArray[i].innerHTML)) { // Convert to uppercase to avoid case sensitivity
            // Get the card element of the users whose names do not include the search string.
            if ($(postArray[i]).parents().eq(2).hasClass("d-none")) {
                $(postArray[i]).parents().eq(2).removeClass("d-none");
            };
        }
    }
}

function hideReactions() {
    $("#reaction-popup").addClass("d-none");
}

function hideReactionList() {
    $("#reaction-list-popup").addClass("d-none");
}

function Update_Reactions() {
    //chill for now
}