using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

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

        public event System.Action onAdvClosed;

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
        private static extern bool YG_isSupported();

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

        [DllImport("__Internal")]
        private static extern void YG_setLeaderboardScore(string leaderboard, int number);
#endif

        void Start()
        {
            _timeBetweenAdv = Time.realtimeSinceStartup + MinTimeBetweenAdv / 2;
        }

        public bool IsAdvReady()
        {
            return _timeBetweenAdv < Time.realtimeSinceStartup;
        }

        public void ShowAdv()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (!YG_isSupported()) {
                return;
            }
#endif

            if (!IsAdvReady())
            {
                return;
            }

            _timeBetweenAdv = Time.realtimeSinceStartup + MinTimeBetweenAdv;

#if !UNITY_EDITOR && UNITY_WEBGL
            Time.timeScale = 0f;
            ShowAdvInternal();
#else
            OnAdvClosed("false");
#endif
        }

        public string GetLangCode()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (!YG_isSupported()) {
                return null;
            }
#endif

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
            if (!YG_isSupported()) {
                return;
            }
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
            Time.timeScale = 1f;
#endif

            onAdvClosed?.Invoke();
        }

        public void UploadGameData(int[] completedLevels)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (!YG_isSupported()) {
                return;
            }
#endif

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
            if (!YG_isSupported()) {
                return;
            }
#endif

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
            if (!YG_isSupported()) {
                return;
            }
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
            GameReadyInternal();
#endif
        }

        public bool IsInitialized()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (!YG_isSupported()) {
                return true;
            }
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
            return IsInitializedInternal();
#else
            return true;
#endif
        }

        public void SetLeaderboardScore(string leaderboard, int number)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (!YG_isSupported()) {
                return;
            }
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
            YG_setLeaderboardScore(leaderboard, number);
#endif
        }
    }
}
