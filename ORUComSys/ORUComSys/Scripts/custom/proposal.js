$(document).ready(() => {
    console.log("Script loaded: proposal.js");
});

$("body").on("click", ".accept-proposal-invite-button", AcceptProposal);
$("body").on("click", ".decline-proposal-invite-button", DeclineProposal);
$("body").on("click", ".btn-participant-list", ToggleParticipantsDiv);
$("body").on("click", "#delete-proposal-btn", DeleteProposal);

function AcceptProposal() {
    var proposalId = $(this).data("proposal-id");
    $.ajax({
        url: "/Proposal/AcceptProposalInvite/" + proposalId,
        type: "POST",
        dataType: "JSON",
        success: () => {
            var counter = $($(this).parents().eq(1)[0].childNodes[4]).text();
            $($(this).parents().eq(1)[0].childNodes[4]).text(parseInt(counter, 10) + 1);
            SetBorders($(this).parents().eq(2));
            ButtonGroup($(this).parents().eq(0));
        },
        error: () => {
            console.log("Error: Unable to accept proposal invite");
        }
    });
}

function DeclineProposal() {
    var proposalId = $(this).data("proposal-id");
    $.ajax({
        url: "/Proposal/DeclineProposalInvite/" + proposalId,
        type: "POST",
        dataType: "JSON",
        success: () => {
            $(this).parents().eq(2).addClass("d-none"); // KKona
        },
        error: () => {
            console.log("Error: Unable to decline proposal invite");
        }
    });
}

function ButtonGroup(elementId) {
    if (!$(elementId).hasClass("d-none")) {
        $(elementId).addClass("d-none");
    }
}

function SetBorders(entireElement) {
    if ($(entireElement[0]).hasClass("border-warning")) {
        $(entireElement).removeClass("border-warning");
    }
    if (!$(entireElement[0]).hasClass("border-success")) {
        $(entireElement).addClass("border-success");
    }
    if (!$(entireElement[0].childNodes[1]).hasClass("border-bottom")) {
        $(entireElement[0].childNodes[1]).addClass("border-bottom");
    }
    if (!$(entireElement[0].childNodes[1]).hasClass("border-success")) {
        $(entireElement[0].childNodes[1]).addClass("border-success");
    }
    if (!$(entireElement[0].childNodes[5]).hasClass("border-top")) {
        $(entireElement[0].childNodes[5]).addClass("border-top");
    }
    if (!$(entireElement[0].childNodes[5]).hasClass("border-success")) {
        $(entireElement[0].childNodes[5]).addClass("border-success");
    }
}

function ToggleParticipantsDiv() {
    if ($(this).hasClass("focus")) {
        $(this).removeClass("focus");
    }
    if ($("#ParticipantsDiv").hasClass("d-none")) {
        GetParticipantsContent(this);
    } else {
        ToggleParticipantsDisplay();
    }
}

function GetParticipantsContent(passedThis) {
    var proposalId = $(passedThis).data("proposal-id");
    $.ajax({
        url: "/Proposal/GetParticipantsContent/" + proposalId,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#ParticipantsDiv").html(data);
            CreateParticipantsPopper(passedThis);
        },
        error: () => {
            console.log("Error: Unable to get participants content.");
        }
    });
}

function CreateParticipantsPopper(passedThis) {
    var ref = passedThis;
    var pop = $("#ParticipantsDiv");
    new Popper(ref, pop, {
        placement: "top",
        modifiers: {
            offset: {
                enabled: true,
                offset: "0, 10"
            }
        }
    });
    ToggleParticipantsDisplay();
}

function ToggleParticipantsDisplay() {
    $("#ParticipantsDiv").toggleClass("d-none");
}

function DeleteProposal() {
    var proposalId = this.getAttribute("data-proposal-id");
    var leftProposalDisplay = $("#proposal-div-" + proposalId);
    $.ajax({
        type: "DELETE",
        url: "/api/AjaxApi/DeleteProposal/" + proposalId,
        contentType: "application/json;charset=UTF-8",
        success: () => {
            $($(this).parents()[1]).addClass("d-none");
            leftProposalDisplay.addClass("d-none");
        },
        error: () => {
            alert("Error: Failure to delete meeting");
        }
    });
}