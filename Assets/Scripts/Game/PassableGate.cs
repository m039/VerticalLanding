using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class PassableGate : MonoBehaviour
    {
        #region Inspector

        public GateColor gateColor;

        #endregion

        SpriteRenderer _gate;

        SpriteRenderer Gate
        {
            get
            {
                if (_gate == null)
                {
                    _gate = transform.Find("Gate")?.GetComponent<SpriteRenderer>();
                }

                return _gate;
            }
        }

        void OnValidate()
        {
            UpdateColor();
        }

        void UpdateColor()
        {
            if (Gate == null)
                return;

            Gate.color = gateColor.ToColor();
        }
    }
}
