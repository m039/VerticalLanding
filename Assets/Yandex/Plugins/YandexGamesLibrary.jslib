mergeInto(LibraryManager.library, {
	GetLangInternal: function() {
		var lang = ysdk.environment.i18n.lang;
		var bufferSize = lengthBytesUTF8(lang) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(lang, buffer, bufferSize);
		return buffer;
	},
	
	ShowAdvInternal: function() {
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
				  myUnityInstance.SendMessage('YandexGamesManager', 'OnAdvClosed', new Boolean(wasShown).toString());
				},
				onError: function(error) {
				}
			}
		});
	},

	UploadGameDataInternal: function(data) {
    	var dataString = UTF8ToString(data);
    	var myobj = JSON.parse(dataString);
    	player.setData(myobj);
  	},

  	DownloadGameDataInternal: function() {
    	player.getData().then(_data => {
        	const myJSON = JSON.stringify(_data);
        	myUnityInstance.SendMessage('YandexGamesManager', 'OnDownloadGameData', myJSON);
    	});
 	},
	    
	GameReadyInternal: function() {
        if (ysdk.features.LoadingAPI) {
            ysdk.features.LoadingAPI.ready();
        }
    },

	IsInitializedInternal: function() {
        return typeof ysdk != "undefined" && typeof player != "undefined";
    },

	YG_setLeaderboardScore: function(leaderboard, number) {
        var name = UTF8ToString(leaderboard);

        ysdk.getLeaderboards().then(lb => {
            lb.setLeaderboardScore(name, number);
        });
    },
  });