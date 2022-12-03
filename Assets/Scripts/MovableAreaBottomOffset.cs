using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class MovableAreaBottomOffset : MonoBehaviour
    {
        #region Inspector

        [SerializeField] Camera _Camera;

        #endregion

        public float GetOffset()
        {
            if (_Camera == null)
                return 0f;

            var y1 =_Camera.ScreenToWorldPoint(transform.position).y;
            var y2 = _Camera.transform.position.y;

            return y2 - _Camera.orthographicSize - y1;
        }
    }
}
