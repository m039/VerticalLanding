using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using m039.Common;

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
    }
}
