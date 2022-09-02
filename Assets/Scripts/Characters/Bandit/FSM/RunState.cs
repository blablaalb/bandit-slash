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
        private FloatRange _distanceRange;
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

        public string StateName => "Run";
        public Vector2 TargetPosition { get; set; }

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
            _animations.Run();
        }

        public void Exit()
        {
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
            var layerMask = LayerMask.GetMask("Knight", "Bandit");
            var collision = Physics2D.OverlapBox(point, size, 0f, layerMask);
            if (collision == null)
            {
                DebugHelper.Drawer.Instance.DrawCube(point, size, color: Color.green, time: 1f);
                return true;
            }
            else
            {
                DebugHelper.Drawer.Instance.DrawCube(point, size, color: Color.red, time: 1f);
                return false;
            }
        }

    }
}