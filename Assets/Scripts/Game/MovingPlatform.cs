using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SF
{
    public class MovingPlatform : MonoBehaviour
    {
        #region Inspector

        public Transform platform;

        [Range(0f, 1f)]
        public float value = 0.5f;

        [SerializeField] Vector2 _Point1 = new Vector2(-1f, 0f);

        [SerializeField] Vector2 _Point2 = new Vector2(1f, 0f);

        public float speed = 1f;

        public float startDelay = 1f;

        public float stayTimeAtEnd = 1f;

        public bool moveRightAtStart = true;

        #endregion

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

        void Start()
        {
            StartCoroutine(DoAnimation());
        }

        IEnumerator DoAnimation()
        {
            yield return new WaitForSeconds(startDelay);

            var direction = moveRightAtStart ? 1 : -1;
            var v = value;
            var rigidbody = platform.GetComponent<Rigidbody2D>();

            while (true)
            {
                v += Time.deltaTime * direction * speed;

                if (direction == 1 && v >= 1f)
                {
                    yield return new WaitForSeconds(stayTimeAtEnd);
                    direction = -1;
                } else if (direction == -1 && v <= 0)
                {
                    yield return new WaitForSeconds(stayTimeAtEnd);
                    direction = 1;
                }

                rigidbody.MovePosition(Vector2.Lerp(Point1, Point2, v));
                yield return null;
            }
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
