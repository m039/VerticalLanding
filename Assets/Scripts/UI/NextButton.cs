using UnityEngine;
using UnityEngine.UI;

namespace VL
{
    public class NextButton : MonoBehaviour
    {
        public Button button;

        Transform _enabledIcon;

        Transform _disableIcon;

        public void SetIconVisibility(bool visibility)
        {
            if (_enabledIcon == null)
            {
                _enabledIcon = transform.Find("Icon/Enabled");
            }

            if (_disableIcon == null)
            {
                _disableIcon = transform.Find("Icon/Disabled");
            }

            _enabledIcon.gameObject.SetActive(visibility);
            _disableIcon.gameObject.SetActive(!visibility);
        }
    }
}
