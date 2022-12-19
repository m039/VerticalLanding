using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF
{
    public class PauseScreen : MonoBehaviour
    {

        public static PauseScreen Create(PauseScreen prefab)
        {
            var pauseScreen = Instantiate(prefab);
            return pauseScreen;
        }

        SoundButton _soundButton;

        Animator _animator;

        void Awake()
        {
            _soundButton = GetComponentInChildren<SoundButton>();
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            Pause();

            _animator.SetTrigger("Appear");
            _soundButton.enabled = SoundManager.Instance.IsSoundEnabled();
        }

        void Pause()
        {
            Time.timeScale = 0f;
        }

        void Resume()
        {
            Time.timeScale = 1f;
        }

        public void Close()
        {
            _animator.SetTrigger("Disappear");
        }

        public void EndAnimation()
        {
            Resume();
            Destroy(gameObject);
        }

        public void OnPlayClicked()
        {
            Close();
        }

        public void OnMenuClicked()
        {
            Resume();
            SceneController.Instance.OpenMainMenu();
        }

        public void OnReplayClicked()
        {
            Resume();
            SceneController.Instance.Reload();
        }

        public void OnSoundEnableClicked()
        {
            var soundEnabled = !SoundManager.Instance.IsSoundEnabled();
            SoundManager.Instance.SetEnableSound(soundEnabled);
            _soundButton.enabled = soundEnabled;
        }
    }
}
