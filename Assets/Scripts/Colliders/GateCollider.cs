using UnityEngine;

namespace VL
{
    public class GateCollider : MonoBehaviour
    {

        PassableGateGroup _gateGroup;

        PassableGate _gate;

        private void Awake()
        {
            _gateGroup = GetComponentInParent<PassableGateGroup>();
            _gate = GetComponentInParent<PassableGate>();
        }

        public bool Consume(Player player)
        {
            if (_gateGroup == null)
                return true;

            var isConsumed = _gateGroup.IsConsumed;

            _gateGroup.Consume(player, _gate);

            return !isConsumed;
        }

        public GateColor GetGateColor()
        {
            return _gate.gateColor;
        }
    }
}
