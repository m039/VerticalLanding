using System.Collections;
using UnityEngine;

namespace VL
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;

        private Vector3 _originalPos;

        void Awake()
        {
            Instance = this;
        }

        public static void Shake(float duration, float amount)
        {
            Instance._originalPos = Instance.gameObject.transform.localPosition;
            Instance.StopAllCoroutines();
            Instance.StartCoroutine(Instance.ShakeCoroutine(duration, amount, true));
        }

        public IEnumerator ShakeCoroutine(float duration, float amount, bool fade)
        {
            var d = duration;

            while (d > 0)
            {
                var a = fade ? Mathf.SmoothStep(0f, amount, d / duration) : amount;
                var offset = Random.insideUnitCircle * a;
                transform.localPosition = _originalPos + new Vector3(offset.x, offset.y, 0);
                d -= Time.deltaTime;

                yield return null;
            }

            transform.localPosition = _originalPos;
        }
    }
}
