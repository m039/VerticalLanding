using UnityEngine;

namespace VL
{
    public static class Consts
    {
        public const string MenuItemRoot = "VL";

        public static readonly LayerMask EndPlatformLayerMask = 1 << 6;

        public const string MainMenuSceneName = "MainMenu";

        const float MinAspect = 1080 / 1920f;

        const float MaxAspect = 459f / 538f;

        public static float GetAspect()
        {
            return WebGLSupport.IsMobile() ? MaxAspect : MinAspect;
        }
    }
}
