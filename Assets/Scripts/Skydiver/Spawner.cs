using UnityEngine;

namespace SF01
{
    public class Spawner : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        MovableArea _MovableArea;

        #endregion

        public void PutBackObstacle(Obstacle obstacle)
        {
            var scale = Random.Range(0.5f, 2f);
            obstacle.transform.localScale = Vector3.one * scale;

            var camera = Camera.main;
            if (camera == null)
                return;

            if (_MovableArea == null)
                return;

            var p = camera.transform.position;

            p.z = 0;

            obstacle.transform.position = p + Vector3.down * (camera.orthographicSize + obstacle.Radius) + _MovableArea.GetRandomAvailableX(obstacle.Radius) * Vector3.right;
        }
    }
}
