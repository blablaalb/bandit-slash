using System;
using System.Collections;
using System.Collections.Generic;
using PER.Common;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action LeftPressed;
    public event Action LeftReleased;
    public event Action RightPressed;
    public event Action RightReleased;
    public event Action AttackPressed;
    public event Action JumpstPressed;

    internal void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftPressed?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            LeftReleased?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightPressed?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            RightReleased?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            AttackPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpstPressed?.Invoke();
        }
    }


}
