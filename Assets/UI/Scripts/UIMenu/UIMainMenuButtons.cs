using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuButtons : MonoBehaviour
{
    public void PlayButtonClick()
    {
        Debug.Log("PlayButtonClick");
        FindObjectOfType<GameManager>().LoadLevel(0);
    }

    public void SettingsButtonClick()
    {
        Debug.Log("SettingsButtonClick");
    }

    public void QuitButtonClick()
    {
        Debug.Log("QuitButtonClick");
        //Application.Quit();
    }
}
