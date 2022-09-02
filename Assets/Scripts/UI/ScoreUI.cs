using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI _tmPro;

    internal void Awake()
    {
        _tmPro = GetComponent<TextMeshProUGUI>();
    }

    internal void Start()
    {
        GameManager.Instance.BanditKilled += OnBanditKilled;
    }

    internal void OnDestory()
    {
        try
        {
            GameManager.Instance.BanditKilled -= OnBanditKilled;
        }
        catch { }
    }

    private void OnBanditKilled()
    {
        _tmPro.SetText(GameManager.Instance.Score.ToString());
    }
}
