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
                DoAwake();
            } else
            {
                Destroy(gameObject);
            }
        }

        void DoAwake()
        {
            SetEnableSound(IsSoundEnabled());
        }

        const string SoundEnabledKey = "sound_enabled";

        public bool IsSoundEnabled()
        {
            return PlayerPrefs.GetInt(SoundEnabledKey, 1) == 1;
        }

        public void SetEnableSound(bool enable)
        {
            PlayerPrefs.SetInt(SoundEnabledKey, enable ? 1 : 0);
            var bus = FMODUnity.RuntimeManager.GetBus("Bus:/Main");
            bus.setMute(!enable);
        }
    }
}
