using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using m039.Common;
using UnityEngine.UIElements;
using FMOD.Studio;

namespace VL
{
    public class LockIndicator : SingletonMonoBehaviour<LockIndicator>, IOnStateEnter
    {
        static readonly int VisibilityKey = Animator.StringToHash("Visibility");

        Animator _animator;

        RectTransform _group;

        TMPro.TMP_Text _number;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _group = transform.Find("Group").GetComponent<RectTransform>();
            _number = transform.Find("Group/Number").GetComponent<TMPro.TMP_Text>();
        }

        void Start()
        {
            _group.gameObject.SetActive(true);
        }

        public void SetNumber(int number)
        {
            _number.text = number.ToString();
        }

        public void Appear()
        {
            _animator.SetBool(VisibilityKey, true);
        }

        public void Disappear()
        {
            _animator.SetBool(VisibilityKey, false);
        }

        void IOnStateEnter.OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //_group.gameObject.SetActive(false);
        }
    }
}