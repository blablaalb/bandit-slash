using PER.Common.FSM;
using UnityEngine;
using Characters.Bandit;
using System.Collections;

namespace Characters.Bandit.FSM
{
    [System.Serializable]
    public class DieState : IState
    {
        private BanditAnimations _animations;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        private BanditBrain _bandit;
        [SerializeField]
        private float _timeout;
        public string StateName => "Die";

        public void Initialize(BanditAnimations animations, Collider2D collider, Rigidbody2D rb, BanditBrain bandit)
        {
            _animations = animations;
            _collider = collider;
            _rb = rb;
            _bandit = bandit;
        }

        public void Enter()
        {
            _animations.Die();
            _rb.simulated = false;
            _collider.enabled = false;
            _animations.StartCoroutine(TimeoutCoroutine());
            GameManager.Instance.OnBanditDied();
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

        private IEnumerator TimeoutCoroutine()
        {
            var wait = new WaitForSeconds(_timeout);
            yield return wait;
            _bandit.Disappear();
        }
    }
}