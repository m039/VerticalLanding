using m039.Common;
using System.Collections;
using System.Collections.Generic;
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

        public bool IsConsumed { get; private set; }

        void Start()
        {
            _gates = GetComponentsInChildren<PassableGate>();
            _collectables = GetComponentsInChildren<Collectable>();
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
        }
    }
}
