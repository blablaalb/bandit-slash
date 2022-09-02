using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseScreen;

    public void HidePauseScreen()
    {
        _pauseScreen.gameObject.SetActive(false);
    }

    public void ShowPauseScreen()
    {
        _pauseScreen.gameObject.SetActive(true);
    }
}
