using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class CameraController : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        Transform _FollowAt;

        #endregion

        Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_camera == null || _FollowAt == null)
                return;

            var p = _camera.transform.position;
            p.y = _FollowAt.transform.position.y - _camera.orthographicSize;
            _camera.transform.position = p;
        }
    }
}
