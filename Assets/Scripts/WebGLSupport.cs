using System.Runtime.InteropServices;

namespace SF
{
    public static class WebGLSupport
    {
        [DllImport("__Internal")]
        private static extern bool isMobile();

        public static bool IsMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
             return isMobile();
#endif
            return false;
        }
    }
}
