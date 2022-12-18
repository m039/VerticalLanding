using UnityEngine;

namespace SF
{
    public class GateCollider : MonoBehaviour
    {

        PassableGateGroup _gateGroup;

        private void Awake()
        {
            _gateGroup = GetComponentInParent<PassableGateGroup>();
        }

        public bool Consume()
        {
            if (_gateGroup == null)
                return true;

            var isConsumed = _gateGroup.IsConsumed;

            _gateGroup.Consume();

            return !isConsumed;
        }

        public GateColor GetGateColor()
        {
            return transform.parent.GetComponent<PassableGate>().gateColor;
        }
    }
}
