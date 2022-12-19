using m039.BasicLocalization;
using m039.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private void Awake()
        {
            _levelFmtText = transform.Find("Screen/Text/LevelFmtText").GetComponent<TMPro.TMP_Text>();
        }

        private void Start()
        {
            _levelFmtText.text = string.Format(BasicLocalization.GetTranslation("level_fmt"), Level);
        }

        public void OnPlayClicked()
        {
            SceneController.Instance.LoadNextLevel();
        }

        public void OnMenuClicked()
        {
            SceneController.Instance.OpenMainMenu();
        }

        public void OnReplayClicked()
        {
            SceneController.Instance.Reload();
        }
    }
}
