using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu: UIWindow
{
    public void ResumeButtonClick()
    {
        Debug.Log("ResumeButtonClick");
    }

    public void PauseButtonClick()
    {
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
