using System.Collections;
using System.Collections.Generic;
using PER.Common.FSM;
using UnityEngine;

[System.Serializable]
public class AttackState : IState
{
    public string StateName => "Attack";

    public void Enter()
    {
        throw new System.NotImplementedException("Attack state is not implemented");
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
