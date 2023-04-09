mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
    console.log("log something");
  },

  SetToLeaderboard: function(value) {
    ysdk.getLeaderboards()
    .then(lb => {
      lb.setLeaderboardScore('GameScoreLeaderboard', value);
    });
  },

});