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

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            PositionCamera();
        }

        void PositionCamera()
        {
            if (followAt == null ||
                movableArea == null ||
                _camera == null ||
                Freez)
                return;

            var size = _camera.orthographicSize;

            var top = movableArea.TopY - size;
            var bottom = movableArea.BottomY + size;
            var y = Mathf.Clamp(followAt.position.y, bottom, top);

            var p = transform.position;
            p.y = y;
            transform.position = p;
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
