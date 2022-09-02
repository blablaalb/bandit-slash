using System;
using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class DieState : IState
{
    private KnightAnimations _animations;

    public string StateName => "Die";


    public void Initialize(KnightAnimations animations)
    {
        _animations = animations;
    }

    public void Enter()
    {
        _animations.Die();
        throw new NotImplementedException("Die state is not implemented");
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
}
