using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SF
{
    public class CameraController : MonoBehaviour
    {
        #region Inspector

        public Transform followAt;

        public MovableArea movableArea;

        public Rect deadzone;

        #endregion

        public bool Freez { get; set; }

        Camera _camera;

        Vector3 _smoothPos;

        Vector3 _currentVelocity;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            DoReset();
        }

        public void DoReset()
        {
            _currentVelocity = Vector3.zero;
            _smoothPos = followAt.position;
            _smoothPos.z = transform.position.z;
            Freez = false;
            PositionCamera(true);
        }

        void Update()
        {
            PositionCamera();
        }

        void PositionCamera(bool force = false)
        {
            if (followAt == null ||
                movableArea == null ||
                _camera == null ||
                Freez)
                return;

            var size = _camera.orthographicSize;

            var top = movableArea.TopY - size;
            var bottom = movableArea.BottomY + size;

            if (force)
            {
                var y = Mathf.Clamp(followAt.position.y, bottom, top);

                var p = transform.position;
                p.y = y;
                transform.position = p;
            } else
            {
                var localY = followAt.position.y - transform.position.y;

                if (localY < deadzone.yMin)
                {
                    _smoothPos.y = followAt.position.y - deadzone.yMin;
                }
                else if (localY > deadzone.yMax)
                {
                    _smoothPos.y = followAt.position.y - deadzone.yMax;
                }

                _smoothPos.y = Mathf.Clamp(_smoothPos.y, bottom, top);

                transform.position = Vector3.SmoothDamp(transform.position, _smoothPos, ref _currentVelocity, 0.1f);
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        private void OnSceneGUI()
        {
            var cc = target as CameraController;

            Vector3[] vs =
            {
                cc.transform.position + new Vector3(cc.deadzone.xMin, cc.deadzone.yMin, 0),
                cc.transform.position + new Vector3(cc.deadzone.xMax, cc.deadzone.yMin, 0),
                cc.transform.position + new Vector3(cc.deadzone.xMax, cc.deadzone.yMax, 0),
                cc.transform.position + new Vector3(cc.deadzone.xMin, cc.deadzone.yMax, 0)
            };

            var transparent = new Color(0, 0, 0, 0);
            Handles.DrawSolidRectangleWithOutline(vs, transparent, Color.red);
        }
    }
#endif
}
