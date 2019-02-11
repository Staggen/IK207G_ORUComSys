$(document).ready(() => {
    console.log("Script loaded: calendar.js");

    $("#calendar").fullCalendar();

    var event = { id: 1, title: "New event", start: new Date("2019-02-09"), end: new Date("2019-02-12") };

    $("#calendar").fullCalendar("renderEvent", event, true); // true: stick = true


    // VIKTIGT!!!!!
    //Så här addar den in från t.ex en funktion / array / URL till kalendern = .fullCalendar(‘addEventSource’, source)

    /*
   var meeting = {id:MeetingId title:Description(ny string i meetingmodels) start:StartTime(ny Date kolumn) end:EndTime?(ny Date kolumn) url:Mötets sida på forumet? (händer vid klick) }
               $('#calendar').fullCalendar('renderEvent', meeting, true);
   */
});