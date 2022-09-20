var FollowingService = function () {

    var createFollowing = function (followId, done, fail) {
        $.postJSON("/api/followings", { "followeeId": followId },
            done)
            .fail(fail);
    }

    var deleteFollowing = function (followId, done, fail) {
        $.ajax({
            url: "/api/followings/" + followId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    }

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    }
}();
