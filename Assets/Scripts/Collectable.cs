using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class Collectable : MonoBehaviour
    {
        #region Inspector

        public GateColor gateColor;

        #endregion

        SpriteRenderer _renderer;

        SpriteRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = GetComponent<SpriteRenderer>();
                }

                return _renderer;
            }
        }

        void OnValidate()
        {
            UpdateColor();
        }

        void UpdateColor()
        {
            if (Renderer == null)
                return;

            Renderer.color = gateColor.ToColor();
        }

        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}
