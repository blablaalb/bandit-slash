using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class MoveState : IState
{
    private float _speed;
    private float _acceleration;
    private Rigidbody2D _rb;
    private KnightAnimations _animations;

    public string StateName { get; set; }


    public void Initialize(string stateName, float speed, float acceleration, Rigidbody2D rb, KnightAnimations animations)
    {
        StateName = stateName;
        _speed = speed;
        _acceleration = acceleration;
        _rb = rb;
        _animations = animations;
    }

    public void Enter()
    {
        if (_speed < 0) _animations.MoveLeft();
        else _animations.MoveRight();
    }

    public void Exit()
    {
    }

    public void OnUpdate()
    {
    }

    public void OnFixedUpdate()
    {
        var velocity = _rb.velocity;
        velocity.x = velocity.x += _acceleration;
        velocity.x = Mathf.Clamp(velocity.x, -Mathf.Abs(_speed), Mathf.Abs(_speed));
        _rb.velocity = velocity;
    }
}
