using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class TakeDamageState : IState
{
    private KnightAnimations _animations;
    private KnightBrain _context;

    public int Damage { get; set; }
    public string StateName => "Take Damage";


    public void Initialize(KnightAnimations animations, KnightBrain context)
    {
        _animations = animations;
        _context = context;
        _animations.TakeDamageFinished.AddListener(OnTakeDamageFinished);
    }

    public void Enter()
    {
        _context.Health -= Damage;
        _animations.Damage();
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

    private void OnTakeDamageFinished()
    {
        if (_context.Health <= 0) _context.Die();
        else _context.Idle();
    }

}
