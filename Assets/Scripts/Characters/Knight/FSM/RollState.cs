using System;
using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class RollState : IState
{
    private KnightAnimations _animations;
    private Rigidbody2D _rb;
    [SerializeField]
    private float _force;
    private KnightBrain _context;

    public string StateName => "Roll";

    public void Initialize(KnightAnimations animations, Rigidbody2D rb, KnightBrain context)
    {
        _animations = animations;
        _rb = rb;
        _context = context;
        _animations.RollFinished.AddListener(OnRollFinished);
    }

    public void Enter()
    {
        _animations.Roll();
        var force = new Vector2(_force, 0) * _animations.transform.localScale.x;
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Exit()
    {
    }

    public void OnUpdate()
    {
    }

    public void OnFixedUpdate()
    {
    }

    private void OnRollFinished()
    {
        var velocity = _rb.velocity;
        velocity.x = 0;
        _rb.velocity = velocity;
        _context.Idle();
    }
}
