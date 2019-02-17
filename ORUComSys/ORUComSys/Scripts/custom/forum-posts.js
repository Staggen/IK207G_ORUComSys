$(document).ready(() => {
    console.log("Script loaded: forum-posts.js");
    var currentUrl = window.location.href;
    var urlArray = currentUrl.split("/Forum/");
    forumType = urlArray[1].split("/")[0];
    Update_Posts(forumType);
    $("#SubmitPost").prop("disabled", true);
    ClearBox();
});

var forumType;

$("#postWall").on("click", ".deleteBtn", DeletePost);
$("#CreatePostCard").on("keyup", "#Content", AdjustCounter);
$("#postWall").on("click", ".attachment-label", DownloadAttachmentStepOne);
$("#AttachedFile").bind("change", ResetFileUpload);

function ResetFileUpload() {
    for (var i = 0; i < this.files.length; i++) {
        var fileSize = this.files[i].size;
        if (fileSize > 4096000) {
            alert("The maximum allowed file size is 4MB.");
            $("#AttachedFile").wrap('<form>').closest('form').get(0).reset();
            $("#AttachedFile").unwrap();
        }
    }
}

function DownloadAttachmentStepOne() {
    var attachmentName = $(this).text();
    var attachmentId = this.getAttribute("data-attachment-id");
    var filetype = this.getAttribute("data-file-type").substring(1);

    var serviceUrl = "/api/AjaxApi/GetAttachment/" + attachmentId;
    var request = $.get(serviceUrl);
    request.done(function (data) {
        var binaryString = window.atob(data);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        DownloadAttachmentStepTwo(attachmentName, filetype, bytes);
    }).fail(() => {
        console.log("Error: Failure to download file.");
    });
}

function DownloadAttachmentStepTwo(attachmentName, filetype, bytes) {
    var blob = new Blob([bytes], { type: "application/" + filetype });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = attachmentName;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    setTimeout(function () {
        document.body.removeChild(link);
    }, 100);
}

function AdjustCounter() {
    var number = $("#Content").val().length;
    if (number <= 0) {
        if ($("#CreatePostCard").hasClass("border-success")) {
            $("#CreatePostCard").removeClass("border-success");
        }
        if (!$("#CreatePostCard").hasClass("border-danger")) {
            $("#CreatePostCard").addClass("border-danger");
        }
        $("#SubmitPost").prop("disabled", true);
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
        $("#SubmitPost").prop("disabled", false);
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
        $("#SubmitPost").prop("disabled", false);
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
        $("#SubmitPost").prop("disabled", true);
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
            Update_Posts(forumType);
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