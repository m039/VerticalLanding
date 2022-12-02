 var WebGLSupportPlugin = {
     isMobile: function()
     {
         return Module.SystemInfo.mobile;
     }
 };

 mergeInto(LibraryManager.library, WebGLSupportPlugin);
