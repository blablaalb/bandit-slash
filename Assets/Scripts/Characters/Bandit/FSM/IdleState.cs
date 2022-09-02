using PER.Common.FSM;
using UnityEngine;
using Characters.Bandit;

namespace Characters.Bandit.FSM
{
    [System.Serializable]
    public class IdleState : IState
    {
        private BanditAnimations _animations;
        private Rigidbody2D _rb;
        private BanditBrain _context;
        [SerializeField]
        private float _decceleration;

        public string StateName => "Idle";

        public void Initialize(BanditAnimations animations, Rigidbody2D rb, BanditBrain context)
        {
            _animations = animations;
            _rb = rb;
            _context = context;
        }

        public void Enter()
        {
            _animations.Idle();
        }

        public void Exit()
        {
        }

        public void OnFixedUpdate()
        {
            var velocity = _rb.velocity;
            velocity.x = Mathf.Lerp(velocity.x, 0f, _decceleration);
            _rb.velocity = velocity;
        }

        public void OnUpdate()
        {
            if (!_context.LookingAtKnight()) _context.FlipSprite();
        }
    }
}