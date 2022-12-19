using UnityEngine;
using UnityEngine.UI;

namespace VL
{
    public class NextButton : MonoBehaviour
    {
        public Button button;

        Transform _enabledIcon;

        Transform _disableIcon;

        void Awake()
        {
            _enabledIcon = transform.Find("Icon/Enabled");
            _disableIcon = transform.Find("Icon/Disabled");
        }

        public void SetIconVisibility(bool visibility)
        {
            _enabledIcon.gameObject.SetActive(visibility);
            _disableIcon.gameObject.SetActive(!visibility);
        }
    }
}
