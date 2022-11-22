using UnityEngine;
using UnityEngine.InputSystem;

namespace SF
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        float _Speed;

        [SerializeField]
        float _MouseInputXScaler = 1f;

        [SerializeField]
        MovableArea _MovableArea;

        #endregion

        Rigidbody2D _rigidBody;

        CircleCollider2D _collider;

        bool _isAlive;

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
        }

        void Start()
        {
            _isAlive = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Obstacle>() != null)
            {
                _isAlive = false;
            }
        }

        void FixedUpdate()
        {
            if (_isAlive)
            {
                var xShift = 0f;

                if (Mouse.current.leftButton.isPressed)
                {
                    xShift = Mouse.current.delta.ReadValue().x;
                    xShift *= _MouseInputXScaler;
                }

                var newPosition = _rigidBody.position + _Speed * Time.deltaTime * Vector2.down + Vector2.right * xShift;

                newPosition.x = _MovableArea.GetConfinedX(newPosition.x, _collider.radius * transform.localScale.x);

                _rigidBody.MovePosition(newPosition);
            }
        }
    }
}
