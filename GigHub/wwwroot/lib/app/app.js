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

function initGigs() {
    $(".js-toggle-attendance").click(function () {
        var button = $(event.target)
        if (button.hasClass("btn-default")) {
            $.postJSON("/api/attendances", { "gigId": button.attr("data-gig-id") },
                function () {
                    button.removeClass("btn-default")
                        .addClass("btn-info")
                        .text("Going");
                })
                .fail(function () {
                    alert("Something failed!");
                });
        }
        else {
            $.ajax({
                url: "/api/attendances/" + button.attr("data-gig-id"),
                method: "DELETE"
            })
                .done(function () {
                            .button.removeClass("btn-info")
                        .addClass("btn-default")
                        .text("Going?");
                })
                .fail(function () {
                    alert("Something failed.");
                })
        }
    });
}

