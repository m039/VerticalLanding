using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using m039.Common;
using UnityEngine.UIElements;
using FMOD.Studio;

namespace VL
{
    public class LockIndicator : SingletonMonoBehaviour<LockIndicator>
    {
        static readonly int VisibilityKey = Animator.StringToHash("Visibility");

        Animator _animator;

        RectTransform _icon;

        TMPro.TMP_Text _number;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _icon = transform.Find("Icon").GetComponent<RectTransform>();
            _number = transform.Find("Icon/Number").GetComponent<TMPro.TMP_Text>();
        }

        public void SetNumber(int number)
        {
            _number.text = number.ToString();
        }

        public void SetIconActive(bool visibility)
        {
            _icon.gameObject.SetActive(visibility);
        }

        public void Appear()
        {
            _icon.gameObject.SetActive(true);
            _animator.SetBool(VisibilityKey, true);
        }

        public void Disappear()
        {
            _animator.SetBool(VisibilityKey, false);
        }
    }
}
