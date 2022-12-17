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

        public float yOffset = 0f;

        #endregion

        SpriteRenderer[] _renderers;

        SpriteRenderer[] _outlineRenderers;

        float _previousAlpha;

        void Init(bool force) {
            if (_renderers == null || _outlineRenderers  == null || force)
            {
                _renderers = new[] {
                    "Renderers/Foot1",
                    "Renderers/Foot2",
                    "Renderers/Upper Body",
                    "Renderers/Lower Body",
                }
                .Select(s => transform.Find(s).GetComponent<SpriteRenderer>())
                .ToArray();

                _outlineRenderers = new[]
                {
                    "Renderers/Upper Body/Upper Body Outline",
                     "Renderers/Lower Body/Lower Body Outline"
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
                _outlineRenderers.ForEach(r => r.enabled = alpha >= 1);
                _previousAlpha = alpha;
            }
        }
    }
}
