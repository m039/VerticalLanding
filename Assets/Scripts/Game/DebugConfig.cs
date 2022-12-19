using m039.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SF
{
    public class DebugConfig : SingletonScriptableObject<DebugConfig>
    {
#if UNITY_EDITOR
        public bool isWebGLMobile = true;
#else
        public bool isWebGLMobile => false;
#endif

#if UNITY_EDITOR
        public bool showLevelSelectionScreen = true;
#else
        public bool showLevelSelectionScreen => false;
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

#if UNITY_EDITOR
        [CustomEditor(typeof(DebugConfig))]
        public class DebugConfigEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (GUILayout.Button("Delete All Preferences"))
                {
                    Debug.Log("All preferences have been deleted.");
                    PlayerPrefs.DeleteAll();
                }
            }
        }
#endif
    }
}
