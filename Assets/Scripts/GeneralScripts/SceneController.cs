using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float pauseEffectSpeed;
    [SerializeField] private float pauseScale;
    private float currentTime = 2;
    private Coroutine currentCoroutine = null;
    private float previousTimeScale = 0.9f;

    [SerializeField] private GameObject upgradeMenu;

    private const string UPGRADE_MENU = "UpgradeMenu";

    private static SceneController instance;
    public static SceneController Instance
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
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            instance.SetupController();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static void SetupInstance()
    {
        instance = FindObjectOfType<SceneController>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "SceneController";
            instance = gameObj.AddComponent<SceneController>();
            instance.SetupController();
            DontDestroyOnLoad(gameObj);
        }
    }

    private void SetupController() 
    {
        InputManager.inputSystem.Player.OpenUpgradeMenu.performed += OpenUpgradeMenu_performed;
        InputManager.inputSystem.UpgradeMenu.CloseUpgradeMenu.performed += CloseUpgradeMenu_performed;
    }

    private void OpenUpgradeMenu_performed(InputAction.CallbackContext obj)
    {
        if (currentCoroutine != null) 
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        currentCoroutine = StartCoroutine(SwitchTimeScale());
        upgradeMenu.SetActive(true);
        InputManager.Instance.SwitchActionMap(InputManager.inputSystem.UpgradeMenu);
    }

    private void CloseUpgradeMenu_performed(InputAction.CallbackContext obj)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        currentCoroutine = StartCoroutine(SwitchTimeScale());
        upgradeMenu.SetActive(false);
        InputManager.Instance.SwitchActionMap(InputManager.inputSystem.Player);
    }

    private IEnumerator SwitchTimeScale() 
    {
        currentTime = pauseEffectSpeed - currentTime;
        if (previousTimeScale < Time.timeScale)
        {
            previousTimeScale = Time.timeScale;
            while (currentTime < pauseEffectSpeed)
            {
                currentTime += Time.deltaTime / Time.timeScale;
                Time.timeScale = Mathf.Lerp(Time.timeScale, pauseScale, currentTime / pauseEffectSpeed);
                Debug.Log(Time.timeScale);
                yield return null;
            }
        }
        else 
        {
            previousTimeScale = Time.timeScale;
            while (currentTime < pauseEffectSpeed)
            {
                currentTime += Time.deltaTime / Time.timeScale;
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1, currentTime / pauseEffectSpeed);
                Debug.Log(Time.timeScale);
                yield return null;
            }
        }
    }
}
