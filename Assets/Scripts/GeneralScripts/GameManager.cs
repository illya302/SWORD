using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    LevelLoader levelLoader;
    
    public void LoadLevel(int sceneIndex)
    {
        if(levelLoader == null)
            levelLoader = GetComponent<LevelLoader>();

        levelLoader.LoadScene(sceneIndex);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
