using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SF
{
    public class DebugConfig : SingletonScriptableObject<DebugConfig>
    {
#if UNITY_EDITOR
        public bool isWebGLMobile = true;
#else
        public bool isWebGLMobile => false;
#endif

        #region SingletonScriptableObject

        protected override bool UseResourceFolder => true;

        protected override string PathToResource => "DebugConfig";

#endregion

#if UNITY_EDITOR
        [MenuItem(Consts.MenuItemRoot + "/Open DebugConfig")]
        static void Open()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(Instance));
        }
#endif
    }
}
