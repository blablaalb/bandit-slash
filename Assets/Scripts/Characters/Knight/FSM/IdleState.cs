using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class IdleState : IState
{
    [SerializeField]
    private float _decceleration;
    private Rigidbody2D _rb;
    private KnightAnimations _animations;

    public string StateName => "Idle";


    public void Initialize( Rigidbody2D rigidBody, KnightAnimations animations)
    {
        _rb = rigidBody;
        _animations = animations;
    }

    public void Enter()
    {
        _animations.Idle();
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
