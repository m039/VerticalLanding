using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class CollectableCollider : MonoBehaviour
    {
        public GateColor GetGateColor()
        {
            return GetComponent<Collectable>().gateColor;
        }

        public Collectable GetCollectable()
        {
            return GetComponent<Collectable>();
        }
    }
}
