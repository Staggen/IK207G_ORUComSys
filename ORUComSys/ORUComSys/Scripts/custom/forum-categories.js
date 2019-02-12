$(document).ready(() => {
    console.log("Script loaded: forum-categories.js");
});

$(".filter-checkbox").mouseup(filterPosts);

function filterPosts() {
    $(this).button("toggle"); // Because the buttons don't behave, we have to fire it once the first thing we do, and once more the last thing we do.
    var posts = document.getElementsByClassName("category-label");
    var postArray = jQuery.makeArray(posts);

    var categoryCheckboxes = document.getElementsByClassName("filter-checkbox");
    var categoryArray = jQuery.makeArray(categoryCheckboxes);

    var activeArray = [];

    for (var i = 0; i < categoryArray.length; i++) {
        if (categoryArray[i].getAttribute("class").search("active") > 0) {
            activeArray[i] = categoryArray[i].innerText;
        }
    }

    // Remove if post doesn't match checked categories
    for (var i = 0; i < postArray.length; i++) {
        if (!activeArray.includes(postArray[i].innerText)) {
            if (!$(postArray[i]).parents().eq(2).hasClass("d-none")) {
                $(postArray[i]).parents().eq(2).addClass("d-none");
            };
        }
    }

    // Show currently hidden results if they contain search string. Same as a
    for (var i = 0; i < postArray.length; i++) {
        if (activeArray.includes(postArray[i].innerHTML)) { // Convert to uppercase to avoid case sensitivity
            // Get the card element of the users whose names do not include the search string.
            if ($(postArray[i]).parents().eq(2).hasClass("d-none")) {
                $(postArray[i]).parents().eq(2).removeClass("d-none");
            };
        }
    }
    $(this).button("toggle"); // Because the buttons don't behave, we have to fire it once the first thing we do, and once more the last thing we do.
}