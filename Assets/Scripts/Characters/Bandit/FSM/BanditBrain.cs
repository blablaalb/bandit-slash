using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PER.Common.FSM;
using Characters.Bandit;

namespace Characters.Bandit.FSM
{
    public class BanditBrain : Context<IState>
    {
        private BoxCollider2D _collider;
        private Rigidbody2D _rb;
        private BanditAnimations _animations;
        [SerializeField]
        private AttackState _attackState;
        [SerializeField]
        private IdleState _idleState;
        [SerializeField]
        private DieState _dieState;
        [SerializeField]
        private TakeDamageState _takeDamageState;
        [SerializeField]
        private RunState _runState;
        [SerializeField]
        private DisappearState _disappearState;
        [SerializeField]
        private int _maxHealth;
        private KnightBrain _knight;
        [SerializeField]
        private float _maxKnightXDistance;
        [SerializeField]
        private float _maxKnightYDistance;

        public int Health { get; set; }
        public int MaxHelth => _maxHealth;
        public bool Alive => Health > 0;
        /// <summary>
        /// < 0 - left. > 0 -right
        /// </summary>
        /// <returns></returns>
        public float LeftRight => -transform.GetChild(0).transform.localScale.x;

        internal void Awake()
        {
            _knight = FindObjectOfType<KnightBrain>();
            var poolMember = GetComponent<BanditPoolMember>();
            Health = _maxHealth;
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _animations = GetComponentInChildren<BanditAnimations>();
            _attackState.Initialize(_animations, this, transform);
            _takeDamageState.Initialize(this, _animations);
            _dieState.Initialize(_animations, _collider, _rb, this);
            _idleState.Initialize(_animations, _rb, this);
            _runState.Initialize(transform, _collider, _animations, _rb, this);
            _disappearState.Initialize(poolMember);
        }

        protected override void Update()
        {
            if (transform.DistanceX(_knight.transform) < _maxKnightXDistance)
            {
                if (transform.DistanceY(_knight.transform) < _maxKnightYDistance)
                {
                    if (LookingAtKnight())
                        Attack();
                }
            }

            base.Update();
        }

        public void Attack()
        {
            if (Alive && _currentState != _attackState)
                EnterState(_attackState);
        }

        public void Idle()
        {
            if (Alive && _currentState != _idleState)
                EnterState(_idleState);
        }

        public void Die()
        {
            if (_currentState != _dieState)
                EnterState(_dieState);
        }

        public void TakeDamage(int damage)
        {
            if (Alive && _currentState != _takeDamageState)
            {
                _takeDamageState.Damage = damage;
                EnterState(_takeDamageState);
            }
        }

        public void Run(Vector2 position)
        {
            if (Alive && _currentState != _runState)
            {
                _runState.TargetPosition = position;
                EnterState(_runState);
            }
        }

        public void Disappear()
        {
            if (_currentState != _disappearState)
                EnterState(_disappearState);
        }

        public bool RandomSpotLeft(out Vector2 randomSpot)
        {
            return _runState.RandomSpotLeft(out randomSpot);
        }

        public bool RandomSpotRight(out Vector2 randomSpot)
        {
            return _runState.RandomSpotRight(out randomSpot);
        }

        public bool LookingAtKnight()
        {
            var direction = _knight.transform.position.x - transform.position.x;
            return (direction < 0 && LeftRight < 0) || (direction > 0 && LeftRight > 0);
        }

        public void FlipSprite()
        {
            var scale = _animations.transform.localScale;
            scale.x = -scale.x;
            _animations.transform.localScale = scale;
        }

#if UNITY_EDITOR
        [NaughtyAttributes.Button]
        internal void PrintCurrentState()
        {
            Debug.Log($"{gameObject.name} state {_currentState.StateName}", gameObject);
        }
#endif
    }
}
