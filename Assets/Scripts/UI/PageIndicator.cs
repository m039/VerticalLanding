using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class PageIndicator : MonoBehaviour
    {
        public static PageIndicator Create(PageIndicator prefab, Transform parent)
        {
            var pageIndicator = Instantiate(prefab, parent);
            return pageIndicator;
        }

        Transform _filled;

        Transform _hollowed;

        public void SetFilled(bool select)
        {
            if (_filled == null)
            {
                _filled = transform.Find("Filled");
            }

            if (_hollowed == null)
            {
                _hollowed = transform.Find("Hollowed");
            }

            _filled.gameObject.SetActive(select);
            _hollowed.gameObject.SetActive(!select);
        }
    }
}
