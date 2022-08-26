using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class IdleState : IState
{
    private float _decceleration;
    private Rigidbody2D _rb;

    public string StateName => "Idle";


    public void Initialize(float deceleration, Rigidbody2D rigidBody)
    {
        _decceleration = deceleration;
        _rb = rigidBody;
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
        velocity.x = Mathf.Lerp(velocity.x, 0f, _decceleration);
        _rb.velocity = velocity;
    }
}
