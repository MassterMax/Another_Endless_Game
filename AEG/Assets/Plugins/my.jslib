mergeInto(LibraryManager.library, {

  // Hello: function () {
  //   window.alert("Hello, world!");
  //   console.log("log something");
  // },

  SetToLeaderboard: function(value) {
    ysdk.getLeaderboards()
    .then(lb => {
      lb.setLeaderboardScore('ScoreLeaderboard', value);
    }).catch((err) => { 
      console.log("can not set leaderboard score");
      console.log(err);
    });
  },

  // InitLb: function () {
  //   console.log("try to init leaderboard");
  //   ysdk.getLeaderboards().then(_lb => lb = _lb);
  // },

  LogIn: function () {
    console.log("try to log in");
    ysdk.auth.openAuthDialog().then(() => {
      console.log("auth success :)");
      initPlayer().then(_player => {
        myGameInstance.SendMessage('CallJSlib', 'SetUsername', _player.getName());
        myGameInstance.SendMessage('CallJSlib', 'SetAuthorized', "1");
        _player.getData().then(_data => {
          const myJSON = JSON.stringify(_data);
          console.log(myJSON);
          myGameInstance.SendMessage('CallJSlib', 'LoadUserData', myJSON);
        });

      }).catch(err => {
        console.log("player init error:");
        console.log(err);
      });

    }).catch((err) => {
      console.log("not auth :");
      console.log(err);
    });
  },

  // GetHighscore: function () {
  //   ysdk.getLeaderboards()
  //   .then(lb => lb.getLeaderboardPlayerEntry('GameScoreLeaderboard'))
  //   .then(res => {
  //     console.log(res);
  //     console.log(res.score);
  //     myGameInstance.SendMessage('CallJSlib', 'UpdateHighScore', res.score);
  //   })
  //   .catch(err => {
  //     if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
  //       console.log("LEADERBOARD_PLAYER_NOT_PRESENT");
  //       myGameInstance.SendMessage('CallJSlib', 'UpdateHighScore', 0);
  //     }
  //   });
  // },

  // TryToAuthorize: function() {
  //   tryToAuth();
  //   tryToInitLB();
  // },

  // AllowData: function() {
  //   initPlayer().then(_player => {
  //     myGameInstance.SendMessage('CallJSlib', 'UpdateUsername', _player.getName());
  //   });
  // },

  GetLanguage: function() {
    console.log("get user lang");
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer; 
  },

  SaveExtern: function(data){
    console.log("try to save user data:");
    console.log(data);
    var dataString = UTF8ToString(data);
    var myobj = JSON.parse(dataString);
    player.setData(myobj);
  },

  LoadExtern: function(){
    console.log("try to load user data:");
    player.getData().then(_data => {
      const myJSON = JSON.stringify(_data);
      console.log(myJSON);
      myGameInstance.SendMessage('CallJSlib', 'LoadUserData', myJSON);
    });
  },

  TryToAuthorize: function() {
    console.log("TryToAuthorize player:");
    initPlayer().then(_player => {
      console.log(_player);

      if (player.getMode() === 'lite') {
        console.log("Not authorized yet");
        myGameInstance.SendMessage('CallJSlib', 'SetAuthorized', "0");
      } else {
        console.log("Already authorized");
        myGameInstance.SendMessage('CallJSlib', 'SetUsername', player.getName());
        myGameInstance.SendMessage('CallJSlib', 'SetAuthorized', "1");
      }
      console.log("Successfully SetAuthorized");
    }).catch(err => {
      console.log("player init error:");
      console.log(err);
    });
  },

});