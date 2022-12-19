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

        public void ShowLevelCompletionScreen()
        {
            static IEnumerator show()
            {
                yield return new WaitForSeconds(1f);

                LevelCompletionScreen.Create(
                    GameConfig.Instance.levelCompletetionScreenPrefab,
                    LevelSelectionManager.Instance.GetCurrentLevel()
                    );
            }

            StartCoroutine(show());
        }

        public void LoadNextLevel(int level)
        {
            Debug.Log("TODO");
        }
    }
}
