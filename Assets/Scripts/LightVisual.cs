using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LightVisual : MonoBehaviour
{
    [SerializeField] float onDeathLightIntencity;
    [SerializeField] float onDeathLightSpeed;

    private Light2D globalLight;
    private Player player;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
        player = GameObject.Find("Hero").GetComponent<Player>();
    }
    private void Start()
    {
        player.OnDeath += Player_OnDeath;
    }

    private void Player_OnDeath()
    {
        //StartCoroutine(IncreaseLightIntencity());
    }
    
    private IEnumerator IncreaseLightIntencity() 
    { 
        while(onDeathLightIntencity > globalLight.intensity) 
        {
            globalLight.intensity += Time.deltaTime * onDeathLightSpeed;
            yield return null;
        }
    }
    private IEnumerator DecreaseLightIntencity()
    {
        globalLight.intensity = onDeathLightIntencity;
        while (0.03 < globalLight.intensity)
        {
            globalLight.intensity -= Time.deltaTime * onDeathLightSpeed*2;
            yield return null;
        }
    }
}
