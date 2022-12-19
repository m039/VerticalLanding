using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VL
{
    public class OffsetOnMouseButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Inspector

        [SerializeField] RectTransform[] _Transforms;

        [SerializeField] float _Offset = -5f;

        #endregion

        float[] _initOffsets;

        bool _enabled;

        void OnEnable()
        {
            _enabled = true;
        }

        void OnDisable()
        {
            _enabled = false;
        }

        void Awake()
        {
            if (_Transforms == null || _Transforms.Length <= 0)
                return;

            _initOffsets = new float[_Transforms.Length];
            for (int i = 0; i < _Transforms.Length; i++)
            {
                _initOffsets[i] = _Transforms[i].anchoredPosition.y;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && _enabled)
            {
                StartOffset();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && _enabled) { 
                EndOffset();
            }
        }

        void StartOffset()
        {
            if (_Transforms == null || _Transforms.Length <= 0)
                return;

            for (int i = 0; i < _Transforms.Length; i++)
            {
                var tr = _Transforms[i];

                var p = tr.anchoredPosition;
                p.y = _initOffsets[i] + _Offset;
                tr.anchoredPosition = p;
            }
        }

        void EndOffset()
        {
            if (_Transforms == null || _Transforms.Length <= 0)
                return;

            for (int i= 0; i < _Transforms.Length; i++)
            {
                var tr = _Transforms[i];

                var p = tr.anchoredPosition;
                p.y = _initOffsets[i];
                tr.anchoredPosition = p;
            }
        }
    }
}
