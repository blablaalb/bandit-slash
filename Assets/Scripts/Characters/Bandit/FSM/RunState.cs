using PER.Common.FSM;
using UnityEngine;
using Characters.Bandit;
using Common;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Characters.Bandit.FSM
{
    [System.Serializable]
    public class RunState : IState
    {
        [SerializeField]
        private float _speed;
        private KnightBrain _knight;
        private Transform _transform;
        private BoxCollider2D _collider;
        private float _maxX;
        private float _minX;
        private BanditAnimations _animations;
        private Rigidbody2D _rb;
        [SerializeField]
        private float _stopThreshold;
        private BanditBrain _context;
        private GameObject _placeholder;

        public string StateName => "Run";
        public Vector2 TargetPosition { get; private set; }

        public void Initialize(Transform transform, BoxCollider2D collider, BanditAnimations animations, Rigidbody2D rb, BanditBrain context)
        {
            _knight = GameObject.FindObjectOfType<KnightBrain>();
            _transform = transform;
            _collider = collider;
            _animations = animations;
            _rb = rb;
            _context = context;
            _minX = GameManager.Instance.LeftBound.transform.position.x;
            _minX += GameManager.Instance.LeftBound.size.x * 0.5f;
            _maxX = GameManager.Instance.RightBound.transform.position.x;
            _maxX += GameManager.Instance.RightBound.size.x * 0.5f;
        }

        public void Enter()
        {
            _context.LookAt(TargetPosition);
            _animations.Run();
        }

        public void Exit()
        {
            if (_placeholder != null)
                _placeholder.SetActive(false);
        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {
            if (_transform.DistanceX(TargetPosition) >= _stopThreshold)
            {
                var direction = TargetPosition.x - _transform.position.x;
                direction = Mathf.Clamp(direction, -1, 1) * _speed;
                var velocity = _rb.velocity;
                velocity.x = direction;
                _rb.velocity = velocity;
            }
            else
            {
                _placeholder.SetActive(false);
                _context.Idle();
            }
        }

        /// <summary>
        /// Looks for random spot the the right from the knight
        /// </summary>
        /// <param name="randomSpot"></param>
        /// <returns>True if found</returns>
        public bool RandomSpotRight(out Vector2 randomSpot)
        {
            randomSpot = new Vector2();
            var x = _knight.transform.position.x + _knight.Width * 0.5f + _collider.size.x * 0.5f;
            var spot = new Vector2(x, _transform.position.y);
            while (!SpotAvailable(spot) && spot.x < _maxX)
            {
                spot.x += 0.1f;
            }
            if (SpotAvailable(spot))
            {
                randomSpot = spot;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Looks for random spot the the left from the knight
        /// </summary>
        /// <param name="randomSpot"></param>
        /// <returns>True if found</returns>
        public bool RandomSpotLeft(out Vector2 randomSpot)
        {
            randomSpot = new Vector2();
            var x = _knight.transform.position.x - _knight.Width * 0.5f - _collider.size.x * 0.5f;
            var spot = new Vector2(x, _transform.position.y);
            while (!SpotAvailable(spot) && spot.x > _minX)
            {
                spot.x -= 0.1f;
            }
            if (SpotAvailable(spot))
            {
                randomSpot = spot;
                return true;
            }
            return false;
        }

        public bool SpotAvailable(Vector2 spot)
        {
            var boxHeight = _collider.size.y;
            var boxWidth = _collider.size.x;
            var size = new Vector2(boxWidth, boxHeight);
            var point = new Vector2(spot.x, spot.y + boxHeight * 0.5f);
            var layerMask = LayerMask.GetMask("Knight", "Bandit", "Bandit Placeholder");
            var collisions = Physics2D.OverlapBoxAll(point, size, 0, layerMask);
            collisions = collisions.Where(c =>
            {
                var bandit = c.GetComponent<BanditBrain>();
                return bandit == null || bandit.CurrentState == "Attack" || bandit.CurrentState == "Idle";
            }).ToArray();
            if (collisions == null || collisions.Length == 0)
            {
#if UNITY_EDITOR
                DebugHelper.Drawer.Instance.DrawCube(point, size, color: Color.green, time: 1f);
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                DebugHelper.Drawer.Instance.DrawCube(point, size, color: Color.red, time: 1f);
#endif
                return false;
            }
        }

        public void SetTargetPosition(Vector2 position)
        {
            position.x = ClampXWithinScene(position.x);
            if (_placeholder == null)
            {
                _placeholder = new GameObject();
                var collider = _placeholder.AddComponent<BoxCollider2D>();
                collider.size = _collider.size;
                _placeholder.layer = LayerMask.NameToLayer("Bandit Placeholder");
                _placeholder.name = "Bandit Placeholder";
            }
            else
            {
                _placeholder.gameObject.SetActive(true);
            }
            position.y += _collider.size.y * 0.5f;
            _placeholder.transform.position = position;
            TargetPosition = position;
        }

        public float ClampXWithinScene(float x)
        {
            var min = GameManager.Instance.LeftBound.bounds.center.x + GameManager.Instance.LeftBound.size.x * 0.5f + _collider.size.x * 0.5f;
            var max = GameManager.Instance.RightBound.bounds.center.x - GameManager.Instance.RightBound.size.x * 0.5f - _collider.size.x * 0.5f;
            return Mathf.Clamp(x, min, max);
        }
    }
}