using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventManager : MonoBehaviour
{
    //Attack Fix
    public UnityEvent<bool> OnAttack;

    //Game over solution
    //public UnityEvent OnGameOver;

    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //SetupInputSystem();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<EventManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "InputManager";
            instance = gameObj.AddComponent<EventManager>();
            //SetupInputSystem();
            DontDestroyOnLoad(gameObj);
        }
    }
}
