using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VL
{
    public class AdManager : MonoBehaviour
    {
        static public AdManager Instance;

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

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void ShowAdvInternal();
#endif

        void ShowAdv()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Time.timeScale = 0f;
            ShowAdvInternal();
#endif
        }

        public void OnAdvClosed(string wasShown)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Time.timeScale = 1f;
#endif
        }

        public void ShowAd()
        {
            ShowAdv();
        }

        public void OnDie()
        {
            ShowAdv();
        }

        public void OnCompleteLevel()
        {
            // ShowAdv();
        }

        public void OnStartLevel()
        {
            if (LevelSelectionManager.Instance.GetCurrentLevel() != 1)
            {
                ShowAdv();
            }
        }
    }
}
