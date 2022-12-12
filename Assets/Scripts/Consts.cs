using UnityEngine;

namespace SF
{
    public static class Consts
    {
        public const string MenuItemRoot = "SF";

        public static readonly LayerMask EndPlatformLayerMask = 1 << 6;

        const float MinAspect = 1080 / 1920f;

        const float MaxAspect = 459f / 538f;

        public static float GetAspect()
        {
            return WebGLSupport.IsMobile() ? MaxAspect : MinAspect;
        }
    }
}
