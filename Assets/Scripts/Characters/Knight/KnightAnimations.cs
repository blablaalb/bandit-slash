using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimations : MonoBehaviour
{
    [SerializeField]
    private GameObject _spriteGO;
    private Animator _animator;

    internal void Awake()
    {
        _animator = _spriteGO.GetComponent<Animator>();
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

    }


    private void SetMirrored(bool value)
    {
        var scale = _spriteGO.transform.localScale;
        scale.x = value == true ? -1f : 1f;
        _spriteGO.transform.localScale = scale;
    }
}
