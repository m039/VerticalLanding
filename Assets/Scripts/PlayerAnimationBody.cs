using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using m039.Common;

namespace SF
{
    [ExecuteInEditMode]
    public class PlayerAnimationBody : MonoBehaviour
    {
        #region Inspector

        [Range(0f, 1f)]
        public float alpha = 1;

        #endregion

        SpriteRenderer[] _renderers;

        float _previousAlpha;

        void Init(bool force) {
            if (_renderers == null || force)
            {
                _renderers = new[] {
                    "Renderers/Foot1",
                    "Renderers/Foot2",
                    "Renderers/Upper Body",
                    "Renderers/Lower Body",
                }
                .Select(s => transform.Find(s).GetComponent<SpriteRenderer>())
                .ToArray();
            }
        }

        void OnEnable()
        {
            Init(true);
        }

        void Start()
        {
            alpha = 1f;
        }

        void Update()
        {
            Init(false);

            if (_previousAlpha != alpha)
            {
                _renderers.ForEach(r => r.color = r.color.WithAlpha(alpha));
                _previousAlpha = alpha;
            }
        }
    }
}
