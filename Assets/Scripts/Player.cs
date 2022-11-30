using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using m039.BasicLocalization;

namespace SF
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        public float upForce;

        public float rotationSpeed;

        public Sprite fireBig;

        public TMPro.TMP_Text mainLabel;

        #endregion

        Rigidbody2D _rigidBody;

        SpriteRenderer _fireRenderer;

        bool _alive;

        Vector2 _initPosition;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _fireRenderer = transform.Find("Fire").GetComponent<SpriteRenderer>();
            _alive = true;
            _initPosition = transform.position;
        }

        void Start()
        {
            HideMainLabel();
        }

        void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                _rigidBody.position = _initPosition;
                _rigidBody.rotation = 0;
                _rigidBody.velocity = new Vector2(0, 0);
                _rigidBody.angularVelocity = 0;
                _alive = true;
                HideMainLabel();
            }
        }

        void FixedUpdate()
        {
            HandleMovementInput();
        }

        void HandleMovementInput()
        {
            if (!_alive)
                return;

            // Up arrow key.
            if (Keyboard.current.upArrowKey.isPressed)
            {
                _rigidBody.AddForce(transform.up * upForce * Time.deltaTime, ForceMode2D.Force);
                _fireRenderer.sprite = fireBig;
            }
            else
            {
                _fireRenderer.sprite = null;
            }

            // Left and right arrow keys.
            float rotationDirection = 0f;

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                rotationDirection = 1f;
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                rotationDirection = -1f;
            }

            if (rotationDirection != 0)
            {
                _rigidBody.MoveRotation(_rigidBody.rotation + rotationDirection * rotationSpeed * Time.deltaTime);
            }
        }

        const string LoseKey = "lose";

        const string WinKey = "win";

        void HideMainLabel()
        {
            mainLabel.gameObject.SetActive(false);
        }

        void ShowMainLabel(string key)
        {
            mainLabel.gameObject.SetActive(true);
            mainLabel.text = BasicLocalization.GetTranslation(key);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.GetComponent<OffscreenCollider>() != null)
            {
                var alive = false;
                if (_alive != alive)
                {
                    ShowMainLabel(LoseKey);
                }
                _alive = alive;
            }
        }
    }
}
