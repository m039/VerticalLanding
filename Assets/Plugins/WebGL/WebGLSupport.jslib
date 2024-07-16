 var WebGLSupportPlugin = {
     isMobile: function()
     {
        if (typeof ysdk == "undefined") {
            return window.mobileCheck();
        }

        return ysdk.deviceInfo.isMobile() || ysdk.deviceInfo.isTablet();
     }
 };

 mergeInto(LibraryManager.library, WebGLSupportPlugin);
