using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class RotatingPlatform : MonoBehaviour
    {
        #region Inspector

        [SerializeField] Transform _Platform;

        public float rotatingSpeed = 60f;

        [Range(0f, 360f)]
        public float defaultAngle = 0f;

        public bool rotateClockwise = true;

        #endregion

        Rigidbody2D _platformRigidbody;

        float _angle;

        void OnValidate()
        {
            UpdateDefaultRotation();
        }

        private void Awake()
        {
            _platformRigidbody = _Platform.GetComponent<Rigidbody2D>();
            _angle = defaultAngle;
            
        }

        void FixedUpdate()
        {
            UpdateRotation();
        }

        void UpdateDefaultRotation()
        {
            if (_Platform == null || Application.isPlaying)
                return;

            _Platform.rotation = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
        }

        void UpdateRotation()
        {
            var direction = rotateClockwise ? -1 : 1;
            _angle += Time.deltaTime * rotatingSpeed * direction;
            _platformRigidbody.SetRotation(_angle);
        }
    }
}
