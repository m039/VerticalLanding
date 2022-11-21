using UnityEngine;

namespace SF
{
    public class Obstacle : MonoBehaviour
    {
        #region Inspector

        public float radius = 1f;

        #endregion

        public float Radius => transform.localScale.x * radius;
    }
}
