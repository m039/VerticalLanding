using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using m039.BasicLocalization;
using m039.Common;
using System.Linq;

namespace VL
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        public float upForce;

        public float rotationSpeed;

        public CameraController cameraController;

        #endregion

        static bool _sDied = false;

        Animator _animator;

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

        bool _levelCompleted;

        float _initGravityScale;

        RocketFlame _flame;

        public System.Action onLevelCompleted;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _foot1 = transform.Find("Renderers/Foot1").GetComponent<Collider2D>();
            _foot2 = transform.Find("Renderers/Foot2").GetComponent<Collider2D>();
            _bodyColliders = new[] { "Renderers/Upper Body", "Renderers/Lower Body" }
                .Select(s => transform.Find(s).GetComponent<Collider2D>())
                .ToArray();
            _input = GetComponent<PlayerInput>();
            _flame = transform.Find("Renderers/Flame").GetComponent<RocketFlame>();
            _bodyRenderers = new[] { "Renderers/Upper Body", "Renderers/Lower Body", "Renderers/Foot1", "Renderers/Foot2" }
                .Select(s => transform.Find(s).GetComponent<SpriteRenderer>())
                .ToArray();

            _initPosition = transform.position;
            _initGravityScale = _rigidBody.gravityScale;
            _endPlatformCF.useLayerMask = true;
            _endPlatformCF.layerMask = Consts.EndPlatformLayerMask;
        }

        void Start()
        {
            DoReset();

            if (_sDied)
            {
                _animator.SetTrigger("AfterDie");
                _sDied = false;
            }
            else
            {
                _animator.SetTrigger("Appear");
            }

            if (LevelSelectionManager.Instance.GetCurrentLevel() != 1)
            {
                YandexManager.Instance.ShowAdv();
            }
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
            _levelCompleted = false;
            _freezeControls = false;
            cameraController.DoReset();
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
            if (!_alive || _levelCompleted)
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

        void CompleteLevel()
        {
            onLevelCompleted?.Invoke();
            SceneController.Instance.LevelCompleted();
            StopFlame();
            _freezeControls = true;
            _levelCompleted = true;

            FMODUnity.RuntimeManager.PlayOneShot("event:/LevelCompleted");
        }

        public void LoseLevel()
        {
            StopFlame();
            DestroyCapsule();
            _alive = false;
            _sDied = true;

            cameraController.Freez = true;

            IEnumerator reload()
            {
                yield return new WaitForSeconds(3f);

                SceneController.Instance.Reload();

                YandexManager.Instance.ShowAdv();
            }

            StartCoroutine(reload());
            CameraShake.Shake(1f, 0.1f);
        }

        void DestroyCapsule()
        {
            var parts = new[] { "Renderers/Upper Body", "Renderers/Lower Body", "Renderers/Foot1", "Renderers/Foot2" };
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
            } else if (collider.GetComponent<GateCollider>() is GateCollider gate && gate.Consume(this))
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
            _flame.SetOutlineColor(color.ToColor());
        }

        public GateColor GetBodyColor() {
            return _bodyColor;
        }
    }
}
