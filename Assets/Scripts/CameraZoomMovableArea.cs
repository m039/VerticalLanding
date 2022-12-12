using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SF
{
    [ExecuteInEditMode]
    public class CameraZoomMovableArea : MonoBehaviour
    {
        #region Inspector

        [SerializeField] MovableArea _MovableArea;

        #endregion

        Camera _camera;

        void OnEnable()
        {
            _camera = GetComponent<Camera>();
            Update();
        }

        public void Update()
        {
            if (_camera == null || _MovableArea == null)
                return;

            var aspect = Mathf.Clamp(_camera.aspect, 0, Consts.GetAspect());

            _camera.orthographicSize = _MovableArea.Width / (aspect * 2);
        }
    }
}
