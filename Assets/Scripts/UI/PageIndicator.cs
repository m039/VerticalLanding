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

        void Awake()
        {
            _filled = transform.Find("Filled");
            _hollowed = transform.Find("Hollowed");
        }

        public void SetFilled(bool select)
        {
            _filled.gameObject.SetActive(select);
            _hollowed.gameObject.SetActive(!select);
        }
    }
}
