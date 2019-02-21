$(document).ready(() => {
    console.log("Script loaded: global-notifications.js");
    GetNumberOfNotifications();
});

$("body").on("click", "#GlobalNotificationButton", ToggleNotificationsDiv);
$(".fa-bell").hover(ToggleBellSolid);

function ToggleBellSolid() {
    if ($(".fa-bell").hasClass("bell-info")) {
        // No notifications
        if ($(".fa-bell").hasClass("far") && !$(".fa-bell").hasClass("fas")) {
            // Make solid
            $(".fa-bell").removeClass("far");
            $(".fa-bell").addClass("fas");
        } else if ($(".fa-bell").hasClass("fas") && !$(".fa-bell").hasClass("far")) {
            // Make hollow
            $(".fa-bell").removeClass("fas");
            $(".fa-bell").addClass("far");
        }
    } else if ($(".fa-bell").hasClass("bell-light")) {
        // Notifications
        if ($(".fa-bell").hasClass("fas") && !$(".fa-bell").hasClass("far")) {
            // Make hollow
            $(".fa-bell").removeClass("fas");
            $(".fa-bell").addClass("far");
        } else if ($(".fa-bell").hasClass("far") && !$(".fa-bell").hasClass("fas")) {
            // Make solid
            $(".fa-bell").removeClass("far");
            $(".fa-bell").addClass("fas");
        }
    }
}

function ToggleNotificationsDiv() {
    if ($("#NotificationPopUpDiv").hasClass("d-none")) {
        GetNotificationsContent();
        $("#GlobalNotificationButton").addClass("focus");
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

function RefreshNotificationsContent() {
    $.ajax({
        type: "GET",
        url: "/Notifications/GetNotificationsContent/",
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#NotificationPopUpDiv").html(data);
            RecreateNotificationsPopper();
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

function RecreateNotificationsPopper() {
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
}

function ToggleNotificationsDisplay() {
    $("#NotificationPopUpDiv").toggleClass("d-none");
}

function GetNumberOfNotifications() {
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
        if ($("#NotificationNumberSpan").hasClass("badge-light")) {
            $("#NotificationNumberSpan").removeClass("badge-light");
        }
        if (!$("#NotificationNumberSpan").hasClass("badge-danger")) {
            $("#NotificationNumberSpan").addClass("badge-danger");
        }
        if ($(".fa-bell").hasClass("bell-info")) {
            $(".fa-bell").removeClass("bell-info");
        }
        if (!$(".fa-bell").hasClass("bell-light")) {
            $(".fa-bell").addClass("bell-light");
        }
        if ($(".fa-bell").hasClass("far")) {
            $(".fa-bell").removeClass("far");
        }
        if (!$(".fa-bell").hasClass("fas")) {
            $(".fa-bell").addClass("fas");
        }
    } else {
        if ($("#NotificationNumberSpan").hasClass("badge-danger")) {
            $("#NotificationNumberSpan").removeClass("badge-danger");
        }
        if (!$("#NotificationNumberSpan").hasClass("badge-light")) {
            $("#NotificationNumberSpan").addClass("badge-light");
        }
        if ($(".fa-bell").hasClass("bell-light")) {
            $(".fa-bell").removeClass("bell-light");
        }
        if (!$(".fa-bell").hasClass("bell-info")) {
            $(".fa-bell").addClass("bell-info");
        }
        if ($(".fa-bell").hasClass("fas")) {
            $(".fa-bell").removeClass("fas");
        }
        if (!$(".fa-bell").hasClass("far")) {
            $(".fa-bell").addClass("far");
        }
    }
}