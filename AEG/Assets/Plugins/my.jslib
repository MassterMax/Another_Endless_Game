mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
    console.log("log something");
  },

  SetToLeaderboard: function(value) {
    ysdk.getLeaderboards()
    .then(lb => {
      lb.setLeaderboardScore('GameScoreLeaderboard', value);
    }).catch(() => { console.log("can not set leaderboard score"); });
  },

  InitLb: function () {
    console.log("try to init leaderboard");
    ysdk.getLeaderboards().then(_lb => lb = _lb);
  },

  LogIn: function () {
    console.log("try to log in");
    initPlayer().then(_player => {
      if (_player.getMode() === 'lite') {

        ysdk.auth.openAuthDialog().then(() => {

          initPlayer().catch(err => {

          });
        }).catch(() => {

        });
      }
    }).catch(err => {

    });
  },

  GetHighscore: function () {
    ysdk.getLeaderboards()
    .then(lb => lb.getLeaderboardPlayerEntry('GameScoreLeaderboard'))
    .then(res => {
      console.log(res);
      myGameInstance.SendMessage('CallJsLib', 'UpdateHighScore', res);
    })
    .catch(err => {
      if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
      }
    });
  },

  GetUsername: function () {
      myGameInstance.SendMessage('CallJsLib', 'UpdateUsername', player.getName());
  },

});