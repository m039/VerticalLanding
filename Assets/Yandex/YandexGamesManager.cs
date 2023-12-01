using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace VL
{
    public class YandexGamesManager : MonoBehaviour
    {
        const float MinTimeBetweenAdv = 60f;

        [System.Serializable]
        class YandexGameData
        {
            public string completedLevels;
        }

        static public YandexGamesManager Instance;

        public System.Action<int[]> onDownloadGameData;

        float _timeBetweenAdv;

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

        [DllImport("__Internal")]
        private static extern void GameReadyInternal();

        [DllImport("__Internal")]
        private static extern bool IsInitializedInternal();
#endif

        void Start()
        {
            _timeBetweenAdv = Time.realtimeSinceStartup + MinTimeBetweenAdv / 2;
        }

        public void ShowAdv()
        {
            if (_timeBetweenAdv >= Time.realtimeSinceStartup)
            {
                return;
            }

            _timeBetweenAdv = Time.realtimeSinceStartup + MinTimeBetweenAdv;

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

        [Preserve]
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

        [Preserve]
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

        public void GameReady()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            GameReadyInternal();
#endif
        }

        public bool IsInitialized()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IsInitializedInternal();
#else
            return true;
#endif
        }
    }
}
