using m039.BasicLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF
{
    public class LevelCompletionScreen : MonoBehaviour
    {
        public static LevelCompletionScreen Create(LevelCompletionScreen prefab, int level)
        {
            var levelCompletionScreen = Instantiate(prefab);
            levelCompletionScreen.Level = level;
            return levelCompletionScreen;
        }

        public int Level { get; private set; }

        TMPro.TMP_Text _levelFmtText;

        Animator _animator;

        private void Awake()
        {
            _levelFmtText = transform.Find("Screen/Text/LevelFmtText").GetComponent<TMPro.TMP_Text>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _levelFmtText.text = string.Format(BasicLocalization.GetTranslation("level_fmt"), Level);
            _animator.SetTrigger("Appear");
        }

        public void OnPlayClicked()
        {
            SceneController.Instance.LoadNextLevel(Level);
        }

        public void OnMenuClicked()
        {
            SceneManager.LoadScene(Consts.MainMenuSceneName);
        }

        public void OnReplayClicked()
        {
            SceneController.Instance.Reload();
        }
    }
}
