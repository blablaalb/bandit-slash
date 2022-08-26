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

    public string StateName { get; set; }


    public void Initialize(string stateName, float speed, float acceleration, Rigidbody2D rb)
    {
        _speed = speed;
        _acceleration = acceleration;
        _rb = rb;
        StateName = stateName;
    }

    public void Enter()
    {
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
