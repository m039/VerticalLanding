using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using JetBrains.Annotations;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VL
{
    [ExecuteInEditMode]
    public class MovingPlatform : MonoBehaviour
    {
        #region Inspector

        public Transform platform;

        [Range(0f, 1f)]
        public float value = 0.5f;

        [SerializeField] Vector2 _Point1 = new Vector2(-1f, 0f);

        [SerializeField] Vector2 _Point2 = new Vector2(1f, 0f);

        public float moveTime = 1f;

        public float startDelay = 1f;

        public float stayTimeAtEnd = 1f;

        public bool moveRightAtStart = true;

        #endregion

        LineRenderer _lineRenderer;

        Rigidbody2D _rigidbody;

        float _startDelay;

        float _value;

        float _timeAtEnd;

        int _direction;

        public Vector2 Point1 {
            get
            {
                return (Vector2)transform.position + _Point1;
            }

            set
            {
                _Point1 = value - (Vector2)transform.position;
            }
        }

        public Vector2 Point2
        {
            get
            {
                return (Vector2)transform.position + _Point2;
            }

            set
            {
                _Point2 = value - (Vector2)transform.position;
            }
        }

        private void Awake()
        {
            _rigidbody = platform.GetComponent<Rigidbody2D>();
            _value = value;
            _direction = moveRightAtStart ? 1 : -1;
        }

        void FixedUpdate()
        {
            if (_startDelay < startDelay)
            {
                _startDelay += Time.deltaTime;
                return;
            }

            _value = Mathf.Clamp01(_value + Time.deltaTime / moveTime * _direction);

            if (_direction == 1 && _value >= 1f)
            {
                if (_timeAtEnd > stayTimeAtEnd)
                {
                    _timeAtEnd = 0;
                    _direction = -1;
                } else
                {
                    _timeAtEnd += Time.deltaTime;
                }
            }
            else if (_direction == -1 && _value <= 0)
            {
                if (_timeAtEnd > stayTimeAtEnd)
                {
                    _timeAtEnd = 0;
                    _direction = 1;
                } else
                {
                    _timeAtEnd += Time.deltaTime;
                }
            }

            _rigidbody.MovePosition(Vector2.Lerp(Point1, Point2, _value));
        }

        void Update()
        {
            UpdateLineRenderer();
        }

        void UpdateLineRenderer()
        {
            if (_lineRenderer == null)
                _lineRenderer = transform.Find("Trail").GetComponent<LineRenderer>();

            void setPosition(int index, Vector2 point)
            {
                _lineRenderer.SetPosition(
                    index,
                    new Vector3(point.x, point.y, _lineRenderer.transform.position.z)
                    );
            }

            setPosition(0, Point1);
            setPosition(1, Point2);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatformEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var mp = target as MovingPlatform;

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                UpdatePlatform(mp);

                var list = new List<Object>
                {
                    mp
                };
                if (mp.platform != null)
                {
                    list.Add(mp.platform);
                }
                Undo.RecordObjects(list.ToArray(), "Change in Editor");
            }
        }

        void UpdatePlatform(MovingPlatform mp)
        {
            if (mp.platform != null)
            {
                mp.platform.transform.position = Vector2.Lerp(mp.Point1, mp.Point2, mp.value);
            }
        }

        void OnSceneGUI()
        {
            var mp = target as MovingPlatform;

            Handles.color = Color.blue;
            Handles.DrawLine(mp.Point1, mp.Point2, 0.5f);

            // Point 1
            EditorGUI.BeginChangeCheck();
            var newPosition = Handles.PositionHandle(mp.Point1, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mp, "Point Changed");
                mp.Point1 = newPosition;
                UpdatePlatform(mp);
            }

            // Point 2
            EditorGUI.BeginChangeCheck();
            newPosition = Handles.PositionHandle(mp.Point2, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mp, "Point Changed");
                mp.Point2 = newPosition;
                UpdatePlatform(mp);
            }
        }
    }
#endif
}
