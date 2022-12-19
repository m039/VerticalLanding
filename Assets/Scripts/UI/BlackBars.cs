using UnityEngine;

namespace VL
{
    [ExecuteInEditMode]
    public class BlackBars : MonoBehaviour
    {
        #region Inspector

        [SerializeField] Camera _Camera;

        #endregion

        RectTransform _leftBar;

        RectTransform _rightBar;

        void OnEnable()
        {
            _leftBar = transform.Find("LeftBar")?.GetComponent<RectTransform>();
            _rightBar = transform.Find("RightBar")?.GetComponent<RectTransform>();
        }

        float GetAspect()
        {
            return Mathf.Clamp(_Camera.aspect, 0, Consts.GetAspect());
        }

        void Update()
        {
            if (_leftBar == null || _rightBar == null)
                return;

            var mainRect = GetComponent<RectTransform>();
            var width = mainRect.sizeDelta.x;
            var height = mainRect.sizeDelta.y;
            var delta = Mathf.Clamp((width - height * GetAspect()) / 2, 0, float.MaxValue);

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
