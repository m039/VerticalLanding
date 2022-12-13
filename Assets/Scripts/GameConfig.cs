using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using m039.Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SF
{
    public class GameConfig : SingletonScriptableObject<GameConfig>
    {
        #region SingletonScriptableObject

        protected override bool UseResourceFolder => true;

        protected override string PathToResource => "GameConfig";

        #endregion

#if UNITY_EDITOR
        [MenuItem(Consts.MenuItemRoot + "/Open GameConfig")]
        static void Open()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(Instance));
        }
#endif
    }
}