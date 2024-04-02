using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float pauseEffectSpeed;
    [SerializeField] private float pauseScale;
    private float currentTime = 0.5f;
    private Coroutine currentCoroutine = null;
    private float previousTimeScale = 0.9f;

    [SerializeField] private GameObject upgradeMenu;
    private const string UPGRADE_MENU = "UpgradeMenu";


    [SerializeField] private float FadeInTime;
    [SerializeField] private float FadeOutTime;
    private Light2D globalLight;
    private Light2D playerLight;
    private Bloom globalBloom;
    private float currentFadeTime;

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
        GameObject player = GameObject.Find("Hero");
        player.GetComponent<Player>().OnDeath += GameOver;

        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        playerLight = player.transform.Find("Light 2D").GetComponent<Light2D>();
        GameObject.Find("Global Volume").GetComponent<Volume>().profile.TryGet(out globalBloom);

        StartCoroutine(TurnOnLight());
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

    public void CloseUpgradeMenu() 
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
                Time.timeScale = Mathf.Lerp(previousTimeScale, pauseScale, (currentTime / pauseEffectSpeed));
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
                Time.timeScale = Mathf.Lerp(previousTimeScale, 1, (currentTime / pauseEffectSpeed));
                Debug.Log(Time.timeScale);
                yield return null;
            }
        }
    }
    private void GameOver() 
    {
        StartCoroutine(TurnOffLight());
    }
    private IEnumerator TurnOffLight() 
    {
        float glStartValue = globalLight.intensity;
        float gbStartValue = globalBloom.intensity.value;
        currentFadeTime = 0;
        while (currentFadeTime < FadeInTime) 
        {
            currentFadeTime += Time.deltaTime;
            globalLight.intensity = Mathf.Lerp(glStartValue, 0, currentFadeTime / FadeInTime);
            //playerLight.intensity = Mathf.Lerp(playerLight.intensity, 0, currentFadeTime / FadeInTime);
            globalBloom.intensity.value = Mathf.Lerp(gbStartValue, 0, currentFadeTime / FadeInTime);
            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    private IEnumerator TurnOnLight()
    {
        currentFadeTime = 0;
        globalLight.intensity = 0;
        playerLight.intensity = 0;
        globalBloom.intensity.value = 0;
        while (currentFadeTime < FadeOutTime)
        {
            currentFadeTime += Time.deltaTime;
            globalLight.intensity = Mathf.Lerp(0, 0.04f, currentFadeTime / FadeOutTime);
            playerLight.intensity = Mathf.Lerp(0, 1.6f, currentFadeTime / FadeOutTime);
            globalBloom.intensity.value = Mathf.Lerp(0, 0.54f, currentFadeTime / FadeOutTime);
            yield return null;
        }
    }
    private void OnDestroy()
    {
        InputManager.inputSystem.Player.OpenUpgradeMenu.performed -= OpenUpgradeMenu_performed;
        InputManager.inputSystem.UpgradeMenu.CloseUpgradeMenu.performed -= CloseUpgradeMenu_performed;
    }
}
