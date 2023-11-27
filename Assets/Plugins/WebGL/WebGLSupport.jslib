 var WebGLSupportPlugin = {
     isMobile: function()
     {
        return ysdk.deviceInfo.isMobile() || ysdk.deviceInfo.isTablet();
     }
 };

 mergeInto(LibraryManager.library, WebGLSupportPlugin);
