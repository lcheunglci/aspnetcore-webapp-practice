function GigDetailController(followingService) {

    var followButon;

    var init = function () {
        $(".js-toggle-follow").click(toggleFollowing);
    };

    var toggleFollowing = function () {
        followButton = $(event.target);

        var followeeId = followButon.attr("data-data-id");

        if (followingButton.hasClass("btn-default")) {
            followingService.createFollowing(followeeId, done, fail);
        } else {
            followingService.deleteFollowing(followeeId, done, fail);
        }
    };

    var done = function () {
        var text = (followButon.text() == "Follow") ? "Following" : "Follow";
        followButton.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function () {
        alert("Something failed!");
    };

    return {
        init: init
    }
} (FollowingService);

