using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class JumpState : IState
{
    private Rigidbody2D _rb;
    private float _force;
    private KnightAnimations _animations;

    public string StateName => "Jump";


    public void Initialize(Rigidbody2D rigidbody, float force, KnightAnimations animations)
    {
        _rb = rigidbody;
        _force = force;
        _animations = animations;
    }

    public void Enter()
    {
        _animations.Jump();
        var force = new Vector2(0, _force);
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Exit()
    {
    }

    public void OnFixedUpdate()
    {
        if (_rb.velocity.y < 0) _animations.Fall();
    }

    public void OnUpdate()
    {
    }
}
