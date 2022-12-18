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

        public MovableArea movableArea;

        public void Update()
        {
            if (mainCamera == null ||
                leftCollider == null ||
                topCollider == null ||
                bottomCollider == null ||
                rightCollider == null ||
                movableArea == null)
                return;

            var height = mainCamera.orthographicSize * 2;
            var width = mainCamera.aspect * height;

            var sideOffset = Mathf.Clamp((width - movableArea.Width) / 2f, 0, float.MaxValue);

            // Left collider.

            leftCollider.size = new Vector2(ColliderWidth, height);
            leftCollider.offset = new Vector2(-width / 2 - leftCollider.size.x / 2 + sideOffset, 0);

            // Right collider.

            rightCollider.size = new Vector2(ColliderWidth, height);
            rightCollider.offset = new Vector2(width / 2 + rightCollider.size.x / 2 - sideOffset, 0);

            // Top collider.

            topCollider.size = new Vector2(width, ColliderWidth);
            topCollider.offset = new Vector2(0, height / 2 + topCollider.size.y / 2);

            // Bottom collider.

            var startY = mainCamera.transform.position.y - height / 2;
            var endY = movableArea.BottomY - movableArea.GetBottomOffset();
            var offset = Mathf.Clamp(endY - startY, 0f, float.MaxValue);

            bottomCollider.size = new Vector2(width, ColliderWidth);
            bottomCollider.offset = new Vector2(0, -height / 2 - bottomCollider.size.y / 2 + offset);
        }
    }
}