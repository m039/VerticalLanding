using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class PlusOne : MonoBehaviour
    {
        public static PlusOne Create(Vector3 position)
        {
            return Create(GameConfig.Instance.plusOnePrefab, position);
        }

        public static PlusOne Create(PlusOne prefab, Vector3 position)
        {
            var plusOne = GameObject.Instantiate(prefab, position, Quaternion.identity);
            return plusOne;
        }

        public void EndAnimation()
        {
            Destroy(gameObject);
        }
    }
}
