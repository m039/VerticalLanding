using m039.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SF
{
    public class Collectable : MonoBehaviour
    {
        #region Inspector

        public GateColor gateColor;

        public float smileAppearSpeed = 1f;

        public Sprite happySmile;

        public Sprite sadSmile;

        #endregion

        public static GateColor CurrentColor = GateColor.White;

        SpriteRenderer _renderer;

        SpriteRenderer _iconRenderer;

        float _iconAlpha;

        GateColor _prevColor;

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

        private void Awake()
        {
            _iconRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();
            _prevColor = CurrentColor;
        }

        void OnValidate()
        {
            UpdateColor();
        }

        void Update()
        {
            UpdateIcon();
        }

        void UpdateColor()
        {
            if (Renderer == null)
                return;

            Renderer.color = gateColor.ToColor();
        }

        void UpdateIcon()
        {
            if (_iconRenderer == null)
                return;

            if (CurrentColor == GateColor.White)
            {
                _iconRenderer.color = Color.white.WithAlpha(0);
                return;
            }

            if (_prevColor != CurrentColor)
            {
                _iconAlpha = 0f;
                if (CurrentColor == gateColor)
                {
                    _iconRenderer.sprite = happySmile;
                } else
                {
                    _iconRenderer.sprite = sadSmile;
                }

                _prevColor = CurrentColor;
            } else
            {
                _iconAlpha = Mathf.Clamp01(_iconAlpha + Time.deltaTime * smileAppearSpeed);
            }

            _iconRenderer.color = Color.black.WithAlpha(_iconAlpha);
        }

        public void Collect()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/PickUp");
            Destroy(gameObject);
        }
    }
}
