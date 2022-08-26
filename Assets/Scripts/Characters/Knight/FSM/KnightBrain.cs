using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;
using NaughtyAttributes;

public class KnightBrain : Context<IState>
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _decceleration;
    [SerializeField]
    private MoveState _moveLeftState;
    [SerializeField]
    private MoveState _moveRightState;
    [SerializeField]
    private IdleState _idleState;
    [SerializeField]
    private JumpState _jumpState;
    [SerializeField]
    private AttackState _attackState;
    private Rigidbody2D _rb;

    internal void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveLeftState.Initialize(stateName: "Move Left", -_speed, -_acceleration, _rb);
        _moveRightState.Initialize(stateName: "Move Right", _speed, _acceleration, _rb);
        _idleState.Initialize(_decceleration, _rb);
        _jumpState.Initialize(_rb, _jumpForce);
    }

    internal void Start()
    {
        InputManager.Instance.LeftPressed += MoveLeft;
        InputManager.Instance.RightPressed += MoveRight;
        InputManager.Instance.LeftReleased += Idle;
        InputManager.Instance.RightReleased += Idle;
        InputManager.Instance.AttackPressed += Attack;
        InputManager.Instance.JumpstPressed += Jump;
    }

    internal void OnDestroy()
    {
        try
        {
            InputManager.Instance.LeftPressed -= MoveLeft;
            InputManager.Instance.RightPressed -= MoveRight;
            InputManager.Instance.LeftReleased -= Idle;
            InputManager.Instance.RightReleased -= Idle;
            InputManager.Instance.AttackPressed -= Attack;
            InputManager.Instance.JumpstPressed -= Jump;
        }
        catch { }
    }

    override protected void Update()
    {
        if (_currentState == _moveLeftState)
        {
        }
        else if (_currentState == _moveRightState)
        {
        }
        base.Update();
    }

    [Button]
    public void MoveLeft()
    {
        if (_currentState != _moveLeftState)
            EnterState(_moveLeftState);
    }

    [Button]
    public void MoveRight()
    {
        if (_currentState != _moveRightState)
            EnterState(_moveRightState);
    }

    [Button]
    public void Idle()
    {
        if (_currentState != _idleState)
            EnterState(_idleState);
    }

    [Button]
    public void Attack()
    {
        if (_currentState != _attackState)
            EnterState(_attackState);
    }

    [Button]
    public void Jump()
    {
        if (_currentState != _jumpState)
            EnterState(_jumpState);
    }

}
