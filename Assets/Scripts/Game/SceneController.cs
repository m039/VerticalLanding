using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using m039.Common;
using UnityEngine;
using System.Collections;

namespace SF
{
    public class SceneController : SingletonMonoBehaviour<SceneController>
    {
        PauseScreen _pauseScreen;

        void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Reload();
            }
        }

        public void OpenMainMenu()
        {
            SceneManager.LoadScene(Consts.MainMenuSceneName);
        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnPauseButtonClicked()
        {
            if (_pauseScreen == null)
            {
                _pauseScreen = PauseScreen.Create(GameConfig.Instance.pauseScreenPrefab);
            } else
            {
                _pauseScreen.Close();
                _pauseScreen = null;
            }
        }

        public void LevelCompleted()
        {
            var currentLevel = LevelSelectionManager.Instance.GetCurrentLevel();
            LevelSelectionManager.Instance.SetLevelCompleted(currentLevel);

            // Show a completion screen.
            IEnumerator show()
            {
                yield return new WaitForSeconds(1f);

                LevelCompletionScreen.Create(
                    GameConfig.Instance.levelCompletetionScreenPrefab,
                    currentLevel
                    );
            }

            StartCoroutine(show());
        }

        public void LoadNextLevel()
        {
            var manager = LevelSelectionManager.Instance;
            var currentLevel = manager.GetCurrentLevel();
            var nextLevel = currentLevel + 1;
            if (nextLevel > manager.MaxLevels ||
                manager.IsLevelCompleted(nextLevel))
            {
                OpenMainMenu();
            } else
            {
                manager.OpenLevelScene(nextLevel);
            }
        }
    }
}
