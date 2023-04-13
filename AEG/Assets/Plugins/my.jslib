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
        console.log("not auth yet");
        ysdk.auth.openAuthDialog().then(() => {
          console.log("auth success :)");

          initPlayer().then(_player => {
            myGameInstance.SendMessage('CallJSlib', 'UpdateUsername', _player.getName());
          }).catch(err => {
            console.log("player init error");
          });
        }).catch(() => {
          console.log("not auth :(");
        });
      }
    }).catch(err => {
      console.log("player init error");
    });
  },

  GetHighscore: function () {
    ysdk.getLeaderboards()
    .then(lb => lb.getLeaderboardPlayerEntry('GameScoreLeaderboard'))
    .then(res => {
      console.log(res);
      myGameInstance.SendMessage('CallJSlib', 'UpdateHighScore', res);
    })
    .catch(err => {
      if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
      }
    });
  },


});