using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VL
{
    [System.Serializable]
    public class YandexGameData
    {
        public string completedLevels;
    }

    public class YandexManager : MonoBehaviour
    {
        static public YandexManager Instance;

        public System.Action<int[]> onDownloadGameData;

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

        [DllImport("__Internal")]
        private static extern void UploadGameDataInternal(string data);

        [DllImport("__Internal")]
        private static extern void DownloadGameDataInternal();

        [DllImport("__Internal")]
        private static extern string GetLangInternal();
#endif

        void ShowAdv()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Time.timeScale = 0f;
            ShowAdvInternal();
#endif
        }

        public string GetLangCode()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return GetLangInternal();
#else
            return null;
#endif
        }

        public void OnAdvClosed(string wasShown)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Time.timeScale = 1f;
#endif
        }

        public void UploadGameData(int[] completedLevels)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            var json = JsonUtility.ToJson(new YandexGameData
            {
                completedLevels = string.Join(',', completedLevels)
            });
            UploadGameDataInternal(json);
#endif
        }

        public void DownloadGameData()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            DownloadGameDataInternal();
#endif
        }

        public void OnDownloadGameData(string json)
        {
            var data = JsonUtility.FromJson<YandexGameData>(json);
            var completedLevels = new List<int>();

            if (!string.IsNullOrEmpty(data.completedLevels))
            {
                foreach (var level in data.completedLevels.Split(','))
                {
                    if (int.TryParse(level, out int result))
                    {
                        completedLevels.Add(result);
                    }
                }
            }

            onDownloadGameData?.Invoke(completedLevels.ToArray());
        }

        public void ShowAd()
        {
            ShowAdv();
        }

        public void ShowAdvOnDie()
        {
            ShowAdv();
        }

        public void ShowAdvOnCompleteLevel()
        {
            // ShowAdv();
        }

        public void ShowAdvOnStartLevel()
        {
            if (LevelSelectionManager.Instance.GetCurrentLevel() != 1)
            {
                ShowAdv();
            }
        }
    }
}
