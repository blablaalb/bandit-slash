using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditController : MonoBehaviour
{
    public void TakeHit(float damage)
    {
        Debug.Log($"Bandit {gameObject.name} took damage {damage}", gameObject);
    }
}
