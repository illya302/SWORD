using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPauseMenu: UIWindow
{
    private Action OnResume;
    private void Start()
    {
        OnResume += SceneController.Instance.CloseUpgradeMenu;
    }
    public void ResumeButtonClick()
    {
        OnResume?.Invoke();
        //SetWindowState(false);
        Debug.Log("ResumeButtonClick");
    }

    public void PauseButtonClick()
    {
        Time.timeScale = 0f;
        SetWindowState(true);
        Debug.Log("PauseButtonClick");
    }

    public void RestartButtonClick()
    {
        Debug.Log("RestartButtonClick");
    }

    public void QuitButtonClick()
    {
        Application.Quit();
    }
}
