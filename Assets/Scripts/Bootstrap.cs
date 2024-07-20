using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VL
{
    public class Bootstrap : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitUntil(YandexGamesManager.Instance.IsInitialized);
            SceneManager.LoadScene(Consts.MainMenuSceneName);
        }
    }
}
