$(document).ready(() => {
    console.log("Script loaded: search.js");
});

$("#SearchField").on("keyup", SearchUsers);

function SearchUsers() {
    var searchString = $("#SearchField").val();

    // Make an array of all users displayed on the page
    var users = document.getElementsByClassName("personal-info");
    var userArray = jQuery.makeArray(users);

    // Remove if name doesn't contain search string
    for (var i = 0; i < userArray.length; i++) {
        if (!userArray[i].innerHTML.toString().toUpperCase().includes(searchString.toUpperCase())) { // Convert to uppercase to avoid case sensitivity
            // Get the card element of the users whose names do not include the search string.
            if (!$(userArray[i]).parents().eq(3).hasClass("d-none")) {
                $(userArray[i]).parents().eq(3).addClass("d-none");
            };
        }
    }

    // Show currently hidden results if they contain search string. Same as a
    for (var i = 0; i < userArray.length; i++) {
        if (userArray[i].innerHTML.toString().toUpperCase().includes(searchString.toUpperCase())) {
            if ($(userArray[i]).parents().eq(3).hasClass("d-none")) {
                $(userArray[i]).parents().eq(3).removeClass("d-none");
            };
        }
    }
}