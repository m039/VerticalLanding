using m039.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VL
{
    public class PassableGateGroup : MonoBehaviour
    {
        #region Inspector

        public float fadeOutDuration = 1f;

        #endregion

        PassableGate[] _gates;

        Collectable[] _collectables;

        ClosedDoor _closedDoor;

        public bool IsConsumed { get; private set; }

        void Start()
        {
            _gates = GetComponentsInChildren<PassableGate>();
            _collectables = GetComponentsInChildren<Collectable>();
            _closedDoor = GetComponentInChildren<ClosedDoor>();
            IsConsumed = false;
        }

        public void Consume(PassableGate gate)
        {
            if (IsConsumed)
                return;

            IsConsumed = true;

            // Update gates.

            IEnumerator fadeOut(PassableGate gate)
            {
                var initScale = gate.transform.localScale;
                var d = fadeOutDuration;

                while (d > 0)
                {
                    gate.transform.localScale =
                        Vector3.Lerp(
                            initScale,
                            Vector3.zero,
                            EasingFunction.EaseInCubic(1, 0, d / fadeOutDuration)
                            );
                    d -= Time.deltaTime;
                    yield return null;
                }

                Destroy(gate.gameObject);
            }

            _gates.ForEach(g => StartCoroutine(fadeOut(g)));

            // Update collectables.

            _collectables.ForEach(c => c.CurrentColor = gate.gateColor);

            // Update a closed door.

            if (_closedDoor != null)
            {
                _closedDoor.Activate(_collectables.Where(c => c.gateColor == gate.gateColor).ToArray());
            }
        }
    }
}
