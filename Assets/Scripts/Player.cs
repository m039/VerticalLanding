using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using m039.BasicLocalization;
using m039.Common;
using System.Linq;

namespace SF
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        public float upForce;

        public float rotationSpeed;

        public TMPro.TMP_Text mainLabel;

        public CameraController cameraController;

        #endregion

        Rigidbody2D _rigidBody;

        SpriteRenderer[] _bodyRenderers;

        Collider2D _foot1;

        Collider2D _foot2;

        Collider2D[] _bodyColliders;

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
            _foot1 = transform.Find("Foot1").GetComponent<Collider2D>();
            _foot2 = transform.Find("Foot2").GetComponent<Collider2D>();
            _bodyColliders = new[] { "Upper Body", "Lower Body" }
                .Select(s => transform.Find(s).GetComponent<Collider2D>())
                .ToArray();
            _input = GetComponent<PlayerInput>();
            _flame = transform.Find("Flame").GetComponent<RocketFlame>();
            _bodyRenderers = new[] { "Upper Body", "Lower Body", "Foot1", "Foot2" }
                .Select(s => transform.Find(s).GetComponent<SpriteRenderer>())
                .ToArray();

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
            _flame.StartFlame();
        }

        void StopFlame()
        {
            _flame.StopFlame();
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

            bool isBodyTouchingPlatform(Collider2D bodyCollider)
            {
                var count = Physics2D.OverlapCollider(bodyCollider, _endPlatformCF, _sColliderBuffer);
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

            if (_bodyColliders.Any(isBodyTouchingPlatform) ||
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
            mainLabel.color = Color.white;
            mainLabel.text = BasicLocalization.GetTranslation(key);
        }

        void HideMainLabel()
        {
            mainLabel.color = Color.white.WithAlpha(0);
        }

        void CompleteLevel()
        {
            ShowMainLabel(WinKey);
            StopFlame();
            _freezeControls = true;
        }

        void LoseLevel()
        {
            ShowMainLabel(LoseKey);
            StopFlame();
            DestroyCapsule();
            _alive = false;

            cameraController.Freez = true;

            IEnumerator reload()
            {
                yield return new WaitForSeconds(3f);

                SceneController.Instance.Reload();
            }

            StartCoroutine(reload());
            CameraShake.Shake(1f, 0.1f);
        }

        void DestroyCapsule()
        {
            var parts = new[] { "Upper Body", "Lower Body", "Foot1", "Foot2" };
            var partsMass = new[] { 5, 3, 1, 1 };

            for (int i = 0; i < parts.Length; i++)
            {
                var tr = transform.Find(parts[i]);
                if (tr == null)
                    continue;

                var rb = tr.gameObject.AddComponent<Rigidbody2D>();

                rb.gravityScale = 0f;
                rb.mass = partsMass[i];
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;

                rb.AddForce(5 * (tr.position - transform.position).normalized, ForceMode2D.Impulse);
            }

            FMODUnity.RuntimeManager.PlayOneShot("event:/CapsuleDestroyed");
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
            _bodyRenderers.ForEach(r => r.color = color.ToColor());

            Collectable.CurrentColor = color;
        }

        public GateColor GetBodyColor() {
            return _bodyColor;
        }
    }
}
