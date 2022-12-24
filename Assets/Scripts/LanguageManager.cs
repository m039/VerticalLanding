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
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            var code = YandexGamesManager.Instance.GetLangCode();
            var languages = BasicLocalization.GetAvailableLanguages();
            for (int i = 0; i < languages.Count; i++)
            {
                if (languages[i].locale.HasCode(code))
                {
                    BasicLocalization.SelectLanguageAt(i);
                    break;
                }
            }
        }
    }
}
