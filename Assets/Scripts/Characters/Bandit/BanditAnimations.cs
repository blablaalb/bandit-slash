using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Bandit
{
    public class BanditAnimations : MonoBehaviour
    {
        private Animator _animator;

        public UnityEvent TakeDamageFinished;
        public UnityEvent AttackFinished;
        public UnityEvent CheckHit;

        internal void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage()
        {
            _animator.Play("Damage");
        }

        public void Die()
        {
            _animator.Play("DIe");
        }

        public void Idle()
        {
            _animator.Play("Idle");
        }

        public void Run()
        {
            _animator.Play("Run");
        }

        public void Attack()
        {
            _animator.Play("Attack", -1, 0f);
        }

        internal void OnTakeDamageFinished()
        {
            TakeDamageFinished?.Invoke();
        }

        internal void OnAttackFinished()
        {
            AttackFinished?.Invoke();
        }

        internal void OnCheckHit()
        {
            CheckHit?.Invoke();
        }

    }
}