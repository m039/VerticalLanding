using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SF
{
    public class PlayerInput : MonoBehaviour
    {
        #region Inspector

        public ControlButton leftButton;

        public ControlButton rightButton;

        public ControlButton upButton;

        #endregion

        void Start()
        {
            var show = WebGLSupport.IsMobile() || DebugConfig.Instance.showMobileControls;
            var buttons = new List<ControlButton>()
                {
                    leftButton,
                    rightButton,
                    upButton,
                };

            buttons.ForEach(b => b.gameObject.SetActive(show));
        }

        public bool IsUpArrowPressed()
        {
            return Keyboard.current != null && Keyboard.current.upArrowKey.isPressed ||
                upButton != null && upButton.isPressed;
        }

        public bool IsLeftArrowPressed()
        {
            return Keyboard.current != null && Keyboard.current.leftArrowKey.isPressed ||
                leftButton != null && leftButton.isPressed;
        }

        public bool IsRightArrowPressed()
        {
            return Keyboard.current != null && Keyboard.current.rightArrowKey.isPressed ||
                rightButton != null && rightButton.isPressed;
        }
    }
}
