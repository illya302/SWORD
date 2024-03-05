using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu: UIWindow
{
    private GameManager gameManager;

    [Range(0f, 1f)]
    [SerializeField] float pauseTimeScale;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ResumeButtonClick()
    {
        SetWindowState(false);
        gameManager.SetTimeScale(pauseTimeScale);
        Debug.Log("ResumeButtonClick");
    }

    public void PauseButtonClick()
    {
        gameManager.SetTimeScale(1f);
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
