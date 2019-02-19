$(document).ready(() => {
    console.log("Script loaded: global-notifications.js");
    SetNumberOfNotifications();
});

$("#GlobalNotificationButton").on("click", ToggleNotificationsDiv);

function ToggleNotificationsDiv() {
    if ($("#NotificationPopUpDiv").hasClass("d-none")) {
        GetNotificationsContent();
    } else {
        ToggleNotificationsDisplay();
    }
}

function GetNotificationsContent() {
    $.ajax({
        type: "GET",
        url: "/Notifications/GetNotificationsContent/",
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#NotificationPopUpDiv").html(data);
            CreateNotificationsPopper();
        },
        error: () => {
            console.log("Error: Failure to load notifications content.");
        }
    });
}

function CreateNotificationsPopper() {
    var ref = $("#GlobalNotificationButton");
    var pop = $("#NotificationPopUpDiv");
    new Popper(ref, pop, {
        placement: "bottom",
        modifiers: {
            offset: {
                enabled: true,
                offset: "0, 10"
            }
        }
    });
    ToggleNotificationsDisplay();
}

function ToggleNotificationsDisplay() {
    $("#NotificationPopUpDiv").toggleClass("d-none");
}

function SetNumberOfNotifications() {
    $.ajax({
        type: "POST",
        url: "/Notifications/GetNumberOfNotifications/",
        dataType: "JSON",
        success: function (data) {
            $("#NotificationNumberSpan").text(data.Number);
            SetNotificationsButtonStyle(data.Number);
        },
        error: () => {
            console.log("Error: Failure to set number of notifications");
        }
    });
}

function SetNotificationsButtonStyle(Number) {
    if (Number >= 1) {
        if ($("#GlobalNotificationButton").hasClass("btn-secondary")) {
            $("#GlobalNotificationButton").removeClass("btn-secondary");
        }
        if (!$("#GlobalNotificationButton").hasClass("btn-primary")) {
            $("#GlobalNotificationButton").addClass("btn-primary");
        }
    } else {
        if ($("#GlobalNotificationButton").hasClass("btn-primary")) {
            $("#GlobalNotificationButton").removeClass("btn-primary");
        }
        if (!$("#GlobalNotificationButton").hasClass("btn-secondary")) {
            $("#GlobalNotificationButton").addClass("btn-secondary");
        }
    }
}