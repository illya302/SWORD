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
}
