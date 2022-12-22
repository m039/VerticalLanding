using m039.BasicLocalization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VL
{
    public class LanguageManager : MonoBehaviour
    {
        static public LanguageManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                DoAwake();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [DllImport("__Internal")]
        private static extern string GetLang();

        void DoAwake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var code = GetLang();
            var languages = BasicLocalization.GetAvailableLanguages();
            for (int i = 0; i < languages.Count; i++)
            {
                if (languages[i].locale.HasCode(code))
                {
                    BasicLocalization.SelectLanguageAt(i);
                    break;
                }
            }
#endif
        }
    }
}
