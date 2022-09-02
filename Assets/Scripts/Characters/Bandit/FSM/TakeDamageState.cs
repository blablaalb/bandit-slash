using PER.Common.FSM;
using UnityEngine;
using Characters.Bandit;

namespace Characters.Bandit.FSM
{
    [System.Serializable]
    public class TakeDamageState : IState
    {
        private BanditBrain _bandit;
        private BanditAnimations _animations;

        public string StateName => "Take Damage";
        public int Damage { get; set; }

        public void Initialize(BanditBrain bandit, BanditAnimations animations)
        {
            _bandit = bandit;
            _animations = animations;

            animations.TakeDamageFinished.AddListener(NextState);
        }

        public void Enter()
        {
            var health = _bandit.Health - Damage;
            health = Mathf.Max(health, 0);
            _bandit.Health = health;
            _animations.TakeDamage();
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

        private void NextState()
        {
            if (_bandit.Health <= 0f) _bandit.Die();
            else _bandit.Idle();
        }
    }
}