using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class ClosedDoor : MonoBehaviour
    {
        #region Inspector

        public float fadeOutDuration = 1.5f;

        #endregion

        int _collectableCount = 0;

        TMPro.TMP_Text _text;

        void Awake()
        {
            _text = transform.Find("Number/Text").GetComponent<TMPro.TMP_Text>();
        }

        void Start()
        {
            _text.gameObject.SetActive(false);
        }

        void UpdateTextNumber()
        {
            _text.text = _collectableCount.ToString();
        }

        public void Activate(Collectable[] collectables)
        {
            if (collectables == null || collectables.Length == 0)
            {
                FadeAway();
            } else
            {
                foreach (var c in collectables)
                {
                    c.onCollect += OnCollect;
                }

                _collectableCount = collectables.Length;
                _text.gameObject.SetActive(true);
                UpdateTextNumber();
            }
        }

        void FadeAway()
        {
            IEnumerator fadeOut()
            {
                var initScale = transform.localScale;
                var d = fadeOutDuration;

                while (d > 0)
                {
                     var v = EasingFunction.EaseInCubic(0, 1, d / fadeOutDuration);

                    var s = transform.localScale;
                    s.y = Mathf.Lerp(0, initScale.y, v);
                    transform.localScale = s;

                    d -= Time.deltaTime;
                    yield return null;
                }

                Destroy(gameObject);
            }

            StartCoroutine(fadeOut());
        }

        void OnCollect(Collectable collectable)
        {
            _collectableCount--;
            UpdateTextNumber();

            if (_collectableCount <= 0)
            {
                FadeAway();
            }
        }
    }
}
