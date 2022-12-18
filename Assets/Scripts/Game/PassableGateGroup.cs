using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class PassableGateGroup : MonoBehaviour
    {
        #region Inspector

        public float fadeOutDuration = 1f;

        #endregion

        PassableGate[] _gates;

        public bool IsConsumed { get; private set; }

        void Start()
        {
            _gates = GetComponentsInChildren<PassableGate>();
            IsConsumed = false;
        }

        public void Consume()
        {
            if (IsConsumed)
                return;

            IsConsumed = true;

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
        }
    }
}
