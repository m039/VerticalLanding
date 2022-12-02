using UnityEngine;

namespace SF
{
    public class GateCollider : MonoBehaviour
    {
        public GateColor GetGateColor()
        {
            return transform.parent.GetComponent<PassableGate>().gateColor;
        }
    }
}
