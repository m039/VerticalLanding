using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class ObstacleRetriveArea : MonoBehaviour
    {
        public UnityEvent<Obstacle> OnRetriveObstacle;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Obstacle>() is Obstacle obstacle)
            {
                OnRetriveObstacle?.Invoke(obstacle);
            }
        }

        //public bool IsBack(float y, float radius)
        //{
        //    var camera = Camera.main;
        //    if (camera == null)
        //        return false;

        //    return camera.transform.position.y - camera.orthographicSize - radius > y;
        //}
    }
}
