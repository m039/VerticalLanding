mergeInto(LibraryManager.library, {
	GetLang: function() {
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
				  myUnityInstance.SendMessage('AdManager', 'OnAdvClosed', new Boolean(wasShown).toString());
				},
				onError: function(error) {
				}
			}
		});
	},
  });