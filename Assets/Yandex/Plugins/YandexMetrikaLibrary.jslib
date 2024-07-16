mergeInto(LibraryManager.library, {
    YM_isSupported: function() {
		return typeof ymKey != "undefined";
    },

    YM_Hit: function(url) {
        ym(ymKey, 'hit', UTF8ToString(url), {});
    },
  
    YM_ReachGoal: function(target) {
        ym(ymKey, 'reachGoal', UTF8ToString(target));
    }
  });