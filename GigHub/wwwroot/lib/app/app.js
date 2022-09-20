var AttendanceService = function () {

    var createAttendance = function (gigId, done, fail) {
        $.postJSON("/api/attendances", { "gigId": gigId },
            done)
            .fail(fail);
    }

    var deleteAttendance = function (gigId, done, fail) {
        $.ajax({
            url: "/api/attendances/" + gigId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    }

    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendance
    }
}();

var GigsController = function (attendanceService) {
    var button;

    $.postJSON = function (url, data, callback) {
        return jQuery.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            'type': 'POST',
            'url': url,
            'data': JSON.stringify(data),
            'dataType': 'json',
            'success': callback
        });
    };

    var init = function () {
        $(".js-toggle-attendance").click(toggleAttendance)
    };

    var toggleAttendance = function (e) {
        button = $(event.target);

        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default")) {
            attendanceService.createAttendance(gigId, done, fail);
        } else {
            attendanceService.deleteAttendance(gigId, done, fail);
        }
    }

    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-defailt").text(text);
    }

    var fail = function () {
        alert("Something failed.");
    }

    return {
        init: init
    }
}(AttendanceService);
