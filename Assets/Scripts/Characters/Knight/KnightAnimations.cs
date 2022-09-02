using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnightAnimations : MonoBehaviour
{
    private Animator _animator;

    public UnityEvent CheckHit;
    public UnityEvent AttackFinished;
    public UnityEvent RollFinished;
    public UnityEvent TakeDamageFinished;

    internal void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveLeft()
    {
        _animator.Play("Run");
        SetMirrored(true);
    }

    public void MoveRight()
    {
        _animator.Play("Run");
        SetMirrored(false);
    }

    public void Jump()
    {
        _animator.Play("Jump");
    }

    public void Fall()
    {
        _animator.Play("Fall");
    }

    public void Roll()
    {
        _animator.Play("Roll");
    }

    public void Idle()
    {
        _animator.Play("Idle");
    }

    public void Attack()
    {
        var name = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        var next = "Attack1";
        if (name == "Attack1") next = "Attack2";
        else if (name == "Attack2") next = "Attack3";
        _animator.Play(next);
    }

    public void Die()
    {
        _animator.Play("DIe");
    }

    public void Damage()
    {
        _animator.Play("Damage");
    }

    private void SetMirrored(bool value)
    {
        var scale = transform.localScale;
        scale.x = value == true ? -1f : 1f;
        transform.localScale = scale;
    }

    internal void OnCheckHit()
    {
        CheckHit?.Invoke();
    }

    internal void OnAttackFinished()
    {
        AttackFinished?.Invoke();
    }

    internal void OnRollFinished()
    {
        RollFinished?.Invoke();
    }

    internal void OnTakeDamageFinished()
    {
        TakeDamageFinished?.Invoke();
    }

}
