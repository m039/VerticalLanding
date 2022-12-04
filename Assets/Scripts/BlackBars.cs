using UnityEngine;

namespace SF
{
    [ExecuteInEditMode]
    public class BlackBars : MonoBehaviour
    {
        const float Aspect = 1080f / 1920f;

        RectTransform _leftBar;

        RectTransform _rightBar;

        void OnEnable()
        {
            _leftBar = transform.Find("LeftBar")?.GetComponent<RectTransform>();
            _rightBar = transform.Find("RightBar")?.GetComponent<RectTransform>();
        }

        void Update()
        {
            if (_leftBar == null || _rightBar == null)
                return;

            var mainRect = GetComponent<RectTransform>();
            var width = mainRect.sizeDelta.x;
            var height = mainRect.sizeDelta.y;
            var delta = Mathf.Clamp((width - height * Aspect) / 2, 0, float.MaxValue);

            static void setWidth(RectTransform bar, float width)
            {
                var size = bar.sizeDelta;
                size.x = width;
                bar.sizeDelta = size;
            }

            setWidth(_leftBar, delta);
            setWidth(_rightBar, delta);
        }
    }
}
