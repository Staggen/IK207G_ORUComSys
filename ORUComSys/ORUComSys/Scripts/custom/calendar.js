$(document).ready(() => {
    console.log("Script loaded: calendar.js");
});

$("#calendar").fullCalendar({
    eventClick: function (calEvent) {
        alert('Description: ' + calEvent.description);
        // change the border color just for fun
        $(this).css('border-color', 'red');
    }
});

$("#private-data-btn").on("click", GetUserSpecificCalendar);
$("#public-data-btn").on("click", GetPublicCalendar);

function GetPublicCalendar() {
    $("#public-data-btn").prop("disabled", true);
    $.ajax({
        type: "GET",
        url: "/Calendar/GetPublicCalendar/",
        data: {},
        contentType: "application/json;charset=UTF-8",
        dataType: "JSON",
        success: function (data) {
            convertIncomingDataToCalendarEvent(data.allEntries);
        },
        error: () => {
            console.log("Error: Failed to load public calendar data!");
        }
    });
}

function GetUserSpecificCalendar() {
    $("#private-data-btn").prop("disabled", true);
    $.ajax({
        type: "GET",
        url: "/Calendar/GetUserSpecificCalendar/",
        data: {},
        contentType: "application/json;charset=UTF-8",
        dataType: "JSON",
        success: function (data) {
            convertIncomingDataToCalendarEvent(data.allEntries);
        },
        error: () => {
            console.log("Error: Failed to load public calendar data!");
        }
    });
}

function convertIncomingDataToCalendarEvent(meetingArray) {
    var event;
    for (var i = 0; i < meetingArray.length; i++) {
        console.log(meetingArray[i].MeetingDateTime);
        if (meetingArray[i].Type == 0) {
            event = {
                id: meetingArray[i].Id,
                title: meetingArray[i].Title,
                description: meetingArray[i].Description,
                start: meetingArray[i].MeetingDateTime,
                end: new Date(meetingArray[i].MeetingDateTime + 1),
                color: "green"
            };
        } else {
            event = {
                id: meetingArray[i].Id,
                title: meetingArray[i].Title,
                description: meetingArray[i].Description,
                start: meetingArray[i].MeetingDateTime,
                end: new Date(meetingArray[i].MeetingDateTime + 1),
                color: "red"
            };
        }
        console.log(event);
        $("#calendar").fullCalendar("renderEvent", event, true); // true: stick = true
    }
}