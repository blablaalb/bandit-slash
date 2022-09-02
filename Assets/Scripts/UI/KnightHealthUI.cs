using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightHealthUI : MonoBehaviour
{
    private Slider _slider;
    private KnightBrain _knight;

    internal void Awake()
    {
        _slider = GetComponent<Slider>();
        _knight = FindObjectOfType<KnightBrain>();
    }

    internal void LateUpdate()
    {
        var health = (float)_knight.Health / _knight.MaxHealth;
        _slider.value = health;
    }
}
