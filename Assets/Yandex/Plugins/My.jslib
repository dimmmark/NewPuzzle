mergeInto(LibraryManager.library, {

  //Hello: function () {
  //      window.alert("Hello, world!");
  //      console.log("Hello world");
  //},

    SetToLeaderBoard: function (value) {
        ysdk.getLeaderboards()
            .then(lb => {          
                lb.setLeaderboardScore('Levels', value); 
            });
    },

    ShowAddInternal: function () {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function (wasShown) {
                    console.log("------------- closed --------------");
                    // some action after close
                    window.focus();
                },
                onError: function (error) {
                    // some action on error
                }
            }
        });
    },

    

});