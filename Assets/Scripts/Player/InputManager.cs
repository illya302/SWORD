using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputSystem inputSystem;
    private static InputManager instance;
    public static InputManager Instance
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
            inputSystem = new InputSystem();
            SetupInputSystem();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<InputManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "InputManager";
            instance = gameObj.AddComponent<InputManager>();
            SetupInputSystem();
            DontDestroyOnLoad(gameObj);
        }
    }

    private static void SetupInputSystem()
    {
        inputSystem = new InputSystem();
        inputSystem.Enable();
        inputSystem.Player.Attack.Enable();
        inputSystem.Player.Dodge.Enable();
    }

    public Vector2 GetInputVector() 
    {
        Vector2 movement = inputSystem.Player.Movement.ReadValue<Vector2>();
        return movement;
    }

    public Vector2 GetAimVector() 
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimingVector = mouse - (transform.position + new Vector3(0, 1.5f, 0));
        aimingVector.Normalize();
        return aimingVector;
    }

    public Vector3 GetMousePosition() 
    { 
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
