using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;
using NaughtyAttributes;
using System;

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
    private BoxCollider2D _collider;
    private KnightAnimations _animations;
    [SerializeField]
    private float _comboWaitTime;
    [SerializeField]
    private float _attackDistance;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private int _combosCount = 3;

    public IState State => _currentState;

    internal void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponentInChildren<KnightAnimations>();
        _moveLeftState.Initialize(stateName: "Move Left", -_speed, -_acceleration, _rb, _animations);
        _moveRightState.Initialize(stateName: "Move Right", _speed, _acceleration, _rb, _animations);
        _idleState.Initialize(_decceleration, _rb, _animations);
        _jumpState.Initialize(_rb, _jumpForce, _animations);
        _attackState.Initialize(_attackDistance, _animations, transform, _damage, _comboWaitTime, this, _combosCount);
    }

    internal void Start()
    {
        InputManager.Instance.LeftPressed += MoveLeft;
        InputManager.Instance.RightPressed += MoveRight;
        InputManager.Instance.LeftReleased += OnLeftReleased;
        InputManager.Instance.RightReleased += OnRightReleased;
        InputManager.Instance.AttackPressed += Attack;
        InputManager.Instance.JumpstPressed += Jump;
        Idle();
    }

    internal void OnDestroy()
    {
        try
        {
            InputManager.Instance.LeftPressed -= MoveLeft;
            InputManager.Instance.RightPressed -= MoveRight;
            InputManager.Instance.LeftReleased -= OnLeftReleased;
            InputManager.Instance.RightReleased -= OnRightReleased;
            InputManager.Instance.AttackPressed -= Attack;
            InputManager.Instance.JumpstPressed -= Jump;
        }
        catch { }
    }

    override protected void Update()
    {
        if (_currentState == _moveLeftState)
        {
            var hit = _collider.BoxCastLeft(truncasteHeight: 0.5f);
            if (hit != null)
            {
                Idle();
            }
        }
        else if (_currentState == _moveRightState)
        {
            var hit = _collider.BoxCastRight(truncasteHeight: 0.5f);
            if (hit != null)
            {
                Idle();
            }
        }
        else if (_currentState == _jumpState)
        {
            if (IsStanding())
            {
                if (!(_rb.velocity.y > 0))
                    Idle();
            }
        }

        base.Update();
    }

    [Button]
    public void MoveLeft()
    {
        if (_currentState != _moveLeftState)
        {
            var hit = _collider.BoxCastLeft(truncasteHeight: 0.5f);
            if (hit == null)
            {
                EnterState(_moveLeftState);
            }
        }
    }

    [Button]
    public void MoveRight()
    {
        if (_currentState != _moveRightState)
        {
            var hit = _collider.BoxCastRight(truncasteHeight: 0.5f);
            if (hit == null)
            {
                EnterState(_moveRightState);
            }
        }
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
        if (_currentState == _attackState && !_attackState.WaitingForCombo) return;
        if (_currentState == _attackState && _attackState.Combo < _combosCount)
            _currentState.Enter();
        else if (_currentState != _attackState)
            EnterState(_attackState);
    }

    [Button]
    public void Jump()
    {
        if (_currentState != _jumpState)
            if (IsStanding())
                EnterState(_jumpState);
    }

    private bool IsStanding()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, ~(1 << LayerMask.NameToLayer("Knight")));
        return hit.collider != null;
    }

    private void OnLeftReleased()
    {
        if (_currentState == _moveLeftState)
            Idle();
    }

    private void OnRightReleased()
    {
        if (_currentState == _moveRightState)
            Idle();
    }


#if UNITY_EDITOR
    [Button]
    private void PrintCurrentState()
    {
        Debug.Log(_currentState.StateName);
    }
#endif
}
