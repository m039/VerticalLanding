using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using m039.BasicLocalization;
using m039.Common;

namespace SF
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        public float upForce;

        public float rotationSpeed;

        public Sprite fireBig;

        public TMPro.TMP_Text mainLabel;

        public CameraController cameraController;

        #endregion

        Rigidbody2D _rigidBody;

        SpriteRenderer _fireRenderer;

        SpriteRenderer _colorBodyRenderer;

        Collider2D _foot1;

        Collider2D _foot2;

        Collider2D _body;

        bool _alive;

        Vector2 _initPosition;

        ContactFilter2D _endPlatformCF;

        bool _freezeControls;

        GateColor _bodyColor;

        PlayerInput _input;

        bool _started;

        float _initGravityScale;

        RocketFlame _flame;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _fireRenderer = transform.Find("Fire").GetComponent<SpriteRenderer>();
            _foot1 = transform.Find("Foot1").GetComponent<Collider2D>();
            _foot2 = transform.Find("Foot2").GetComponent<Collider2D>();
            _body = transform.Find("Body").GetComponent<Collider2D>();
            _colorBodyRenderer = transform.Find("ColorBody").GetComponent<SpriteRenderer>();
            _input = GetComponent<PlayerInput>();
            _flame = transform.Find("Flame").GetComponent<RocketFlame>();

            _initPosition = transform.position;
            _initGravityScale = _rigidBody.gravityScale;
            _endPlatformCF.useLayerMask = true;
            _endPlatformCF.layerMask = Consts.EndPlatformLayerMask;
        }

        void Start()
        {
            mainLabel.gameObject.SetActive(true);
            mainLabel.enabled = true;

            DoReset();
        }

        void DoReset()
        {
            SetBodyColor(GateColor.White);

            transform.position = _initPosition;
            _rigidBody.position = _initPosition;
            _rigidBody.rotation = 0;
            _rigidBody.velocity = new Vector2(0, 0);
            _rigidBody.angularVelocity = 0;
            _rigidBody.gravityScale = 0;
            _alive = true;
            _started = false;
            _freezeControls = false;
            cameraController.DoReset();
            HideMainLabel();
        }

        void FixedUpdate()
        {
            HandleMovementInput();
            CheckCollisions();
        }

        void HandleMovementInput()
        {
            if (!_alive || _freezeControls)
                return;

            // Up arrow key.
            if (_input.IsUpArrowPressed())
            {
                if (!_started)
                {
                    _rigidBody.gravityScale = _initGravityScale;
                    _started = true;
                }

                _rigidBody.AddForce(Time.deltaTime * upForce * transform.up, ForceMode2D.Force);
                StartFlame();
            }
            else
            {
                StopFlame();
            }

            // Left and right arrow keys.
            float rotationDirection = 0f;

            if (_input.IsLeftArrowPressed())
            {
                rotationDirection = 1f;
            }

            if (_input.IsRightArrowPressed())
            {
                rotationDirection = -1f;
            }

            if (rotationDirection != 0 && _started)
            {
                _rigidBody.angularVelocity = 0f;
                _rigidBody.MoveRotation(_rigidBody.rotation + rotationDirection * rotationSpeed * Time.deltaTime);
            }
        }

        void StartFlame()
        {
            _fireRenderer.sprite = fireBig;
        }

        void StopFlame()
        {
            _fireRenderer.sprite = null;
        }

        static readonly List<Collider2D> _sColliderBuffer = new(16);

        const float HighSpeedThreshold = 1.2f;

        void CheckCollisions()
        {
            if (!_alive)
                return;

            bool isFootOnPlatform(Collider2D foot)
            {
                var count = Physics2D.OverlapCollider(foot, _endPlatformCF, _sColliderBuffer);
                for (int i = 0; i < count; i++)
                {
                    var collider = _sColliderBuffer[i];
                    if (collider.GetComponent<EndPlatformCollider>() != null)
                        return true;
                }

                return false;
            }

            bool isSpeedHigh()
            {
                return _rigidBody.velocity.magnitude > HighSpeedThreshold;
            }

            bool isBodyTouchingPlatform()
            {
                var count = Physics2D.OverlapCollider(_body, _endPlatformCF, _sColliderBuffer);
                for (int i = 0; i < count; i++)
                {
                    var collider = _sColliderBuffer[i];
                    if (collider.GetComponent<EndPlatformCollider>() != null)
                        return true;
                }

                return false;
            }

            var foot1OnPlatform = isFootOnPlatform(_foot1);
            var foot2OnPlatform = isFootOnPlatform(_foot2);

            if (isBodyTouchingPlatform() ||
                (isSpeedHigh() && (foot1OnPlatform || foot2OnPlatform)))
            {
                LoseLevel();
            } else if (foot1OnPlatform && foot2OnPlatform)
            {
                CompleteLevel();
            }
        }

        const string LoseKey = "lose";

        const string WinKey = "win";

        void ShowMainLabel(string key)
        {
            mainLabel.color = Color.black;
            mainLabel.text = BasicLocalization.GetTranslation(key);
        }

        void HideMainLabel()
        {
            mainLabel.color = Color.white.WithAlpha(0);
        }

        void CompleteLevel()
        {
            ShowMainLabel(WinKey);
            _freezeControls = true;
        }

        void LoseLevel()
        {
            ShowMainLabel(LoseKey);
            StopFlame();
            _alive = false;

            cameraController.Freez = true;

            IEnumerator reload()
            {
                yield return new WaitForSeconds(3f);

                SceneController.Instance.Reload();
            }

            StartCoroutine(reload());
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!_alive)
                return;

            if (collider.GetComponent<OffscreenCollider>() != null ||
                collider.GetComponent<ObstacleCollider>())
            {
                LoseLevel();
            } else if (collider.GetComponent<GateCollider>() is GateCollider gate)
            {
                SetBodyColor(gate.GetGateColor());
            } else if (collider.GetComponent<CollectableCollider>() is CollectableCollider collectable)
            {
                if (_bodyColor != collectable.GetGateColor())
                {
                    LoseLevel();
                } else
                {
                    collectable.GetCollectable().Collect();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_alive)
                return;

            if (collision.collider.GetComponent<ObstacleCollider>() != null)
            {
                LoseLevel();
            }
        }

        void SetBodyColor(GateColor color)
        {
            _bodyColor = color;
            _colorBodyRenderer.color = color.ToColor();
        }
    }
}
