$(document).ready(() => {
    console.log("Script loaded: post-category-subscriptions.js");
});

$(".follow-wrapper").on("click", "#subscribe-btn", SubscribeToPostCategories);

function SubscribeToPostCategories() {
    var vesselModel;
    // Get all checkbuttons
    var buttonList = document.getElementsByClassName("filter-checkbox");
    // Make them into an array
    var buttonArray = jQuery.makeArray(buttonList);
    // Set active categories into array
    var activeButtonArray = [];
    for (var x = 0; x < buttonArray.length; x++) {
        if (buttonArray[x].getAttribute("class").search("active") >= 0) {
            activeButtonArray.push(buttonArray[x].innerText);
        }
    }
    // Ajax it into the database
    vesselModel = { CategoriesToFollow: activeButtonArray };
    $.ajax({
        type: "POST",
        url: "/Forum/SubscribeToPostCategories/",
        data: JSON.stringify(vesselModel),
        contentType: "application/json;charset=UTF-8",
        error: () => {
            console.log("Error: Unable to subscribe to selected post categories.");
        }
    });
}