using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    internal void Start()
    {
        GameManager.Instance.Knighdied += ShowGameoverScreen;
    }

    internal void OnDestroy()
    {
        try
        {
            GameManager.Instance.Knighdied -= ShowGameoverScreen;
        }
        catch { }
    }

    [SerializeField]
    private GameObject _startScreen;
    [SerializeField]
    private GameObject _gameOverScreen;

    public void HideStartScreen()
    {
        _startScreen.gameObject.SetActive(false);
    }

    public void ShowStartScreen()
    {
        _startScreen.gameObject.SetActive(true);
    }

    public void HideGameoverScreen()
    {
        _gameOverScreen.gameObject.SetActive(false);
    }

    public void ShowGameoverScreen()
    {
        _gameOverScreen.gameObject.SetActive(true);
    }


}
