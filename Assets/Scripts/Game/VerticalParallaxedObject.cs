using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class VerticalParallaxedObject : MonoBehaviour
    {
        #region Inspector

        public float speed = 0.1f;

        #endregion

        Vector3 _initPosition;

        Vector3 _cameraInitPosition;

        void Start()
        {
            _initPosition = transform.position;
            _cameraInitPosition = Camera.main.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            var p = Camera.main.transform.position - _cameraInitPosition;
            p.z = 0;

            transform.position = _initPosition + speed * p;
        }
    }
}
