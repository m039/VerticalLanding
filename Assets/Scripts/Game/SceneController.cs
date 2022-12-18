using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using m039.Common;

namespace SF
{
    public class SceneController : SingletonMonoBehaviour<SceneController>
    {
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
    }
}
