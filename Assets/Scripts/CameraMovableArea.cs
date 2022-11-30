using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    [ExecuteInEditMode]
    public class CameraMovableArea : MonoBehaviour
    {
        const float ColliderWidth = 1f;

        public Camera mainCamera;

        public BoxCollider2D leftCollider;

        public BoxCollider2D rightCollider;

        public BoxCollider2D topCollider;

        public BoxCollider2D bottomCollider;

        void Update()
        {
            if (mainCamera == null)
                return;

            if (leftCollider == null ||
                topCollider == null ||
                bottomCollider == null ||
                rightCollider == null)
                return;

            var height = mainCamera.orthographicSize * 2;
            var width = mainCamera.aspect * height;

            leftCollider.size = new Vector2(ColliderWidth, height);
            leftCollider.offset = new Vector2(-width / 2 - leftCollider.size.x / 2, 0);

            rightCollider.size = new Vector2(ColliderWidth, height);
            rightCollider.offset = new Vector2(width / 2 + rightCollider.size.x / 2, 0);

            topCollider.size = new Vector2(width, ColliderWidth);
            topCollider.offset = new Vector2(0, height / 2 + topCollider.size.y / 2);

            bottomCollider.size = new Vector2(width, ColliderWidth);
            bottomCollider.offset = new Vector2(0, -height / 2 - bottomCollider.size.y / 2);
        }
    }
}