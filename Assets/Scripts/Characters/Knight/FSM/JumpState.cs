using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class JumpState : IState
{
    private Rigidbody2D _rb;
    private float _force;

    public string StateName => "Jump";


    public void Initialize(Rigidbody2D rigidbody, float force)
    {
        _rb = rigidbody;
        _force = force;
    }

    public void Enter()
    {
        var force = new Vector2(0, _force);
        _rb.AddForce(force, ForceMode2D.Impulse);
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
}
