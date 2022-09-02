using PER.Common.FSM;
using UnityEngine;
using Characters.Bandit;
using System;

namespace Characters.Bandit.FSM
{
    [System.Serializable]
    public class AttackState : IState
    {
        private BanditAnimations _animations;
        private BanditBrain _context;
        private KnightBrain _knight;
        private Transform _transform;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private float _yDistanceThreshold;
        [SerializeField]
        private float _distance;

        public string StateName => "Attack";

        public void Initialize(BanditAnimations animations, BanditBrain context, Transform transform)
        {
            _knight = GameObject.FindObjectOfType<KnightBrain>();
            _animations = animations;
            _animations.AttackFinished.AddListener(OnAttackAnimationFinished);
            _animations.CheckHit.AddListener(CheckHit);
            _context = context;
            _transform = transform;
        }

        public void Enter()
        {
            _animations.Attack();
        }

        public void Exit()
        {
        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {
        }

        private void OnAttackAnimationFinished()
        {
            _context.Idle();
        }

        private void CheckHit()
        {
            var direction = _knight.transform.position.x - _transform.position.x;
            var yDistance = _transform.DistanceY(_knight.transform);
            var xDistance = _transform.DistanceX(_knight.transform);
            if (xDistance > _distance || yDistance > _yDistanceThreshold) return;
            if ((_context.LeftRight < 0 && direction < 0) || (_context.LeftRight > 0 && direction > 0))
                _knight.TakeDamage(_damage);
        }

    }
}