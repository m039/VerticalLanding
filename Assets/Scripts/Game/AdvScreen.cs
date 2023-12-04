using System.Collections;
using UnityEngine;

namespace VL
{
    public class AdvScreen : MonoBehaviour
    {
        public static AdvScreen Create()
        {
            var screen = Instantiate(GameConfig.Instance.advScreenPrefab);
            return screen;
        }

        public event System.Action onEnd;

        TMPro.TMP_Text _number;

        void Awake()
        {
            _number = transform.Find("Screen/Number").GetComponent<TMPro.TMP_Text>();
        }

        void Start()
        {
            IEnumerator coroutine()
            {
                for (int i = 3; i > 0; i--)
                {
                    _number.text = i.ToString();
                    yield return new WaitForSecondsRealtime(1);
                }

                onEnd?.Invoke();
            }

            StartCoroutine(coroutine());
        }
    }
}
