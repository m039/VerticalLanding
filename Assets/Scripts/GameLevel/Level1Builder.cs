using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    [ExecuteInEditMode]
    public class Level1Builder : MonoBehaviour
    {
        #region Inspector

        [SerializeField] MovableArea _MovableArea;

        [SerializeField] Transform _FinishPlatform;

        [SerializeField] Camera _Camera;

        [SerializeField] float _YOffset;

        #endregion

        void Update()
        {
            if (_MovableArea == null || _FinishPlatform == null || _Camera == null)
                return;

            var height = _Camera.orthographicSize * 2;
            _MovableArea.Height = height;

            var p = _FinishPlatform.transform.position;
            p.y = _MovableArea.TopY - height - _YOffset - _MovableArea.GetBottomOffset();
            _FinishPlatform.transform.position = p;
        }
    }
}
