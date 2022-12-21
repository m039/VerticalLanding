using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public interface IOnStateEnter
    {
        void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }
    
    public class OnStateEnterSMB : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<IOnStateEnter>()?.OnStateEnter(animator, stateInfo, layerIndex);
        }
    }
}
