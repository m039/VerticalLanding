using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class MovableArea : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        float _HorizontalPaddings;

        #endregion

        public float GetRandomAvailableX(float radius)
        {
            var camera = Camera.main;
            if (camera == null)
                return 0;

            var halfWidth = camera.orthographicSize * camera.aspect - _HorizontalPaddings - radius;
            return Random.Range(-halfWidth, halfWidth);
        }

        public float GetConfinedX(float x, float radius)
        {
            var camera = Camera.main;
            if (camera == null)
                return x;

            var halfWidth = camera.orthographicSize * camera.aspect - _HorizontalPaddings - radius;

            if (x > halfWidth)
                return halfWidth;
            else if (x < -halfWidth)
                return -halfWidth;
            else
                return x;
        }

        void OnDrawGizmosSelected()
        {
            var camera = Camera.main;
            if (camera == null)
                return;

            Gizmos.color = Color.blue;

            var halfWidth = camera.orthographicSize * camera.aspect - _HorizontalPaddings;

            var p1 = camera.transform.position - camera.orthographicSize * Vector3.up;
            var p2 = camera.transform.position + camera.orthographicSize * Vector3.up;

            Gizmos.DrawLine(p1 + halfWidth * Vector3.right, p2 + halfWidth * Vector3.right);
            Gizmos.DrawLine(p1 + halfWidth * Vector3.left, p2 + halfWidth * Vector3.left);
        }
    }
}
