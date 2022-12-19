using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VL
{
    public class MovableArea : MonoBehaviour
    {
        const float Aspect = 1080f / 1920f;

        #region Inspector

        [SerializeField] Vector2 _MovableAreaSize = new Vector2(10f * Aspect, 10f);

        [SerializeField] MovableAreaBottomOffset _BottomOffset;

        #endregion

        public float Width
        {
            get
            {
                return _MovableAreaSize.x;
            }
            set
            {
                _MovableAreaSize.x = value;
            }
        }

        public float Height
        {
            get
            {
                return _MovableAreaSize.y;
            }
            set
            {
                _MovableAreaSize.y = value;
            }
        }

        public float TopY {
            get {
                return transform.position.y;
            }
        }

        public float BottomY
        {
            get
            {
                return TopY - _MovableAreaSize.y + GetBottomOffset();
            }
        }

        public float GetBottomOffset()
        {
            if (_BottomOffset != null &&
                _BottomOffset.gameObject.activeSelf)
            {
                return _BottomOffset.GetOffset();
            }

            return 0f;
        }

        private void Start()
        {
            if (_BottomOffset != null)
            {
                _BottomOffset.gameObject.SetActive(WebGLSupport.IsMobile());
            }
        }

        void OnDrawGizmos()
        {
            var size = _MovableAreaSize;

            Gizmos.color = Color.blue;

            var x = transform.position.x;
            var y = TopY;

            var p1 = new Vector2(x - size.x / 2, y);
            var p2 = new Vector2(x + size.x / 2, y);
            var p3 = new Vector2(x - size.x / 2, BottomY);
            var p4 = new Vector2(x + size.x / 2, BottomY);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p4);
            Gizmos.DrawLine(p4, p3);
            Gizmos.DrawLine(p1, p3);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(MovableArea))]
        public class MovableAreaEditor : Editor
        {
            public void OnSceneGUI()
            {
                var movableArea = target as MovableArea;

                EditorGUI.BeginChangeCheck();
                var newTargetPosition = Handles.PositionHandle(GetBottomPosition(movableArea), Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(movableArea, "Change Bottom Position");
                    SetBottomPosition(movableArea, newTargetPosition);
                }
            }

            Vector3 GetBottomPosition(MovableArea movableArea)
            {
                var p = movableArea.transform.position;
                p.y = movableArea.TopY - movableArea._MovableAreaSize.y;
                return p;
            }

            void SetBottomPosition(MovableArea movableArea, Vector3 position)
            {
                var size = movableArea._MovableAreaSize;
                size.y = movableArea.TopY - position.y;
                movableArea._MovableAreaSize = size;
            }
        }
#endif
    }
}
