using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class TimerIndicator : SingletonMonoBehaviour<TimerIndicator>, IOnStateEnter
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

        public void Appear()
        {
            //_group.gameObject.SetActive(true);
            _animator.SetBool(VisibilityKey, true);
        }

        public void Disappear()
        {
            _animator.SetBool(VisibilityKey, false);
        }

        public void SetNumber(int number)
        {
            _number.text = number.ToString();
        }

        void IOnStateEnter.OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //_group.gameObject.SetActive(false);
        }
    }
}
