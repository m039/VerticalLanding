using System.Runtime.InteropServices;

namespace VL
{
    public static class WebGLSupport
    {
        [DllImport("__Internal")]
        private static extern bool isMobile();

        public static bool IsMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
             return isMobile();
#else
            return DebugConfig.Instance.isWebGLMobile;
#endif
        }
    }
}
