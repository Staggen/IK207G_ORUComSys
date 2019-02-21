$(document).ready(() => {
    console.log("Script loaded: calendar.js");
    GetPublicCalendar();
    GetUserSpecificCalendar();
});

$("#calendar").fullCalendar({
    eventClick: function (calEvent) {
        $("#meeting-modal-title").text(calEvent.title);
        $("#meeting-modal-description").text(calEvent.description);
        $("#meeting-modal-location").text(calEvent.location);
        $("#meeting-modal-start").text(calEvent.start);
        $("#meeting-modal-end").text(calEvent.end);
        $('#informationModal').modal("toggle");
    }
});

$("#private-data-btn").on("click", GetUserSpecificCalendar);
$("#public-data-btn").on("click", GetPublicCalendar);

function GetPublicCalendar() {
    $.ajax({
        type: "POST",
        url: "/Calendar/GetPublicCalendar/",
        contentType: "application/json;charset=UTF-8",
        success: function (data) {
            if (data.result) {
                CreateCalendarEvent(data.allEntries);
            } else {
                console.log("Controller Error: Unable to get calendar events.");
            }
        },
        error: () => {
            console.log("Error: Failed to load public calendar data!");
        }
    });
}

function GetUserSpecificCalendar() {
    $.ajax({
        type: "POST",
        url: "/Calendar/GetUserSpecificCalendar/",
        contentType: "application/json;charset=UTF-8",
        success: function (data) {
            if (data.result) {
                CreateCalendarEvent(data.allEntries);
            } else {
                console.log("Controller Error: Unable to get calendar events.");
            }
        },
        error: () => {
            console.log("Error: Failed to load public calendar data!");
        }
    });
}

function CreateCalendarEvent(meetingArray) {
    var event;
    var startDateTime;
    var endDateTime;
    for (var i = 0; i < meetingArray.length; i++) {
        startDateTime = new Date(); // While initial time is in UTC, it adjusts it before display to correct time for timezone.
        startDateTime.setTime(meetingArray[i].MeetingDateTime.split("(")[1].split(")")[0], 10);
        endDateTime = new Date(); // 2 hours later than initial time
        endDateTime.setTime(parseInt(meetingArray[i].MeetingDateTime.split("(")[1].split(")")[0], 10) + 7200000);
        if (meetingArray[i].Type == 0) {
            event = {
                id: meetingArray[i].Id,
                title: meetingArray[i].Title,
                description: meetingArray[i].Description,
                location: meetingArray[i].Location,
                start: startDateTime,
                end: endDateTime,
                color: "indianred"
            };
        } else if (meetingArray[i].Type == 1) {
            event = {
                id: meetingArray[i].Id,
                title: meetingArray[i].Title,
                description: meetingArray[i].Description,
                location: meetingArray[i].Location,
                start: startDateTime,
                end: endDateTime,
                color: "lightblue"
            };
        } else if (meetingArray[i].Type == 2) {
            event = {
                id: meetingArray[i].Id,
                title: meetingArray[i].Title,
                description: meetingArray[i].Description,
                location: meetingArray[i].Location,
                start: startDateTime,
                end: endDateTime,
                color: "lightgreen"
            };
        }
        $("#calendar").fullCalendar("renderEvent", event, true); // true: stick = true
    }
}