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
    private float _acceleration;
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
    [SerializeField]
    private RollState _rollState;
    [SerializeField]
    private DieState _dieState;
    [SerializeField]
    private TakeDamageState _takeDamageState;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private KnightAnimations _animations;
    [SerializeField]
    private LayerMask _moveLayerMask;
    [SerializeField]
    private int _health;

    public IState State => _currentState;
    public float Width => _collider.size.x;
    public int Health
    {
        get { return _health; }
        set
        {
            if (value < 0) value = 0;
            _health = value;
        }
    }
    public bool Alive => _health > 0;

    internal void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponentInChildren<KnightAnimations>();
        _moveLeftState.Initialize(stateName: "Move Left", -_speed, -_acceleration, _rb, _animations);
        _moveRightState.Initialize(stateName: "Move Right", _speed, _acceleration, _rb, _animations);
        _idleState.Initialize(_rb, _animations);
        _jumpState.Initialize(_rb, _animations);
        _attackState.Initialize(_animations, transform, this);
        _rollState.Initialize(_animations, _rb, this);
        _dieState.Initialize(_animations);
        _takeDamageState.Initialize(_animations, this);
    }

    internal void Start()
    {
        InputManager.Instance.LeftPressed += MoveLeft;
        InputManager.Instance.RightPressed += MoveRight;
        InputManager.Instance.LeftReleased += OnLeftReleased;
        InputManager.Instance.RightReleased += OnRightReleased;
        InputManager.Instance.AttackPressed += Attack;
        InputManager.Instance.JumpPressed += Jump;
        InputManager.Instance.RollPressed += Roll;
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
            InputManager.Instance.JumpPressed -= Jump;
            InputManager.Instance.RollPressed -= Roll;
        }
        catch { }
    }

    override protected void Update()
    {
        if (_currentState == _moveLeftState)
        {
            var hit = _collider.BoxCastLeft(_moveLayerMask, truncasteHeight: 0.5f);
            if (hit != null)
            {
                Idle();
            }
        }
        else if (_currentState == _moveRightState)
        {
            var hit = _collider.BoxCastRight(_moveLayerMask, truncasteHeight: 0.5f);
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

#if UNITY_EDITOR
    [Button]
#endif
    public void MoveLeft()
    {
        if (_currentState != _moveLeftState)
        {
            var hit = _collider.BoxCastLeft(_moveLayerMask, truncasteHeight: 0.5f);
            if (hit == null)
            {
                EnterState(_moveLeftState);
            }
        }
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void MoveRight()
    {
        if (_currentState != _moveRightState)
        {
            var hit = _collider.BoxCastRight(_moveLayerMask, truncasteHeight: 0.5f);
            if (hit == null)
            {
                EnterState(_moveRightState);
            }
        }
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void Idle()
    {
        if (_currentState != _idleState)
            EnterState(_idleState);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void Attack()
    {
        if (_currentState == _attackState && !_attackState.WaitingForCombo) return;
        if (_currentState == _attackState && !_attackState.AllCombosExecuted)
            _currentState.Enter();
        else if (_currentState != _attackState)
            EnterState(_attackState);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void Jump()
    {
        if (_currentState != _jumpState)
            if (IsStanding())
                EnterState(_jumpState);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void Roll()
    {
        if (_currentState != _rollState)
            EnterState(_rollState);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void Die()
    {
        if (_currentState != _dieState)
            EnterState(_dieState);
    }

    public void TakeDamage(int damage)
    {
        if (_currentState != _takeDamageState && _currentState != _dieState && _health > 0)
        {
            _takeDamageState.Damage = damage;
            EnterState(_takeDamageState);
        }
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
