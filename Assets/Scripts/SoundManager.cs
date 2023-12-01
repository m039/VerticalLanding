using System.Collections;
using UnityEngine;

namespace VL
{
    public class SoundManager : MonoBehaviour
    {
        static SoundManager _sInstance;

        public static SoundManager Instance {
            get
            {
                if (_sInstance == null)
                {
                    var gameObject = new GameObject(nameof(SoundManager) + " (Singleton)");
                    _sInstance = gameObject.AddComponent<SoundManager>();
                    DontDestroyOnLoad(gameObject);
                }

                return _sInstance;
            }
        }

        private void Awake()
        {
            if (_sInstance == null)
            {
                _sInstance = this;
                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            IEnumerator loadAsync()
            {
                while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
                {
                    yield return null;
                }

                SetEnableSound(IsSoundEnabled());
            }

            StartCoroutine(loadAsync());
        }

        const string SoundEnabledKey = "sound_enabled";

        public bool IsSoundEnabled()
        {
            return PlayerPrefs.GetInt(SoundEnabledKey, 1) == 1;
        }

        public void SetEnableSound(bool enable)
        {
            PlayerPrefs.SetInt(SoundEnabledKey, enable ? 1 : 0);
            var bus = FMODUnity.RuntimeManager.GetBus("bus:/Main");
            bus.setMute(!enable);
        }
    }
}
