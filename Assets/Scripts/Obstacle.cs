using UnityEngine;

namespace SF
{
    public class Obstacle : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        float _Radius = 1f;

        #endregion

        public float Radius => transform.localScale.x * _Radius;
    }
}
