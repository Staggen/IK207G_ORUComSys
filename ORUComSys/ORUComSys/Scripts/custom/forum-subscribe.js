$(document).ready(() => {
    console.log("Script loaded: forum-subscribe.js");
});

$(".follow-wrapper").on("click", "#subscribe-btn", subscribeToCategories);

function subscribeToCategories() {
    var activeCategorySubVessel; // prepare a variable that will later possibly be converted to an object that will be passed through ajax to a controller as a model... #coherentshit
    var categorySubCheckboxes = document.getElementsByClassName("filter-checkbox"); // get all category filter check boxes in a collection...

    var categorySubArray = jQuery.makeArray(categorySubCheckboxes); // ...make that collection into an array.

    var activeCategorySubArray = [];

    for (var x = 0; x < categorySubArray.length; x++) { // loop the array and identify all active elements
        if (categorySubArray[x].getAttribute("class").search("active") >= 0) {
            activeCategorySubArray[x] = categorySubArray[x].innerText; // get the category names and assign them to another array.
        }
    }
    
    if (activeCategorySubArray != null) {
        activeCategorySubVessel = { CategoriesToSubscribe: activeCategorySubArray }; // set the subscriptions variable as an object containing the array of active categories.
        $.ajax({ // send dat shit via ajax yo!
            type: "POST",
            url: "/Forum/SubscribeToCategories/",
            data: JSON.stringify(activeCategorySubVessel),
            contentType: "application/json;charset=UTF-8",
            success: function (data) {
                if (data.result) {
                    console.log("Success: subscribeToCategories()");
                } else {
                    console.log("Controller error: Failure to subscribe to categories");
                }
            },
            error: () => {
                console.log("Error: Failure to subscribe to categories");
            }
        });
    } else {
        alert("You need to select at least one category!")
    }
}