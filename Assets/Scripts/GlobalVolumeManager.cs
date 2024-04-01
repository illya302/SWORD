using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeManager : MonoBehaviour
{
    [Header("Camera Lens Distortion Effect")]
    [SerializeField] private float LensDistortionEffectSpeed;
    [SerializeField] private float LensDistortionEffectStartValue;
    private Coroutine DutchEffectCoroutine;

    private PlayerHp player;
    private Volume globalVolume;
    private LensDistortion lens;
    public static GlobalVolumeManager Instance;

    private void Awake()
    {
        Instance = this;
        globalVolume = GetComponent<Volume>();
        globalVolume.profile.TryGet(out lens);
        player = GameObject.Find("Hero").GetComponentInChildren<PlayerHp>();
    }

    private void Start()
    {
        player.OnTakeDamage += Player_OnTakeDamage;
    }

    private void Player_OnTakeDamage()
    {
        StartCoroutine(TakeDamageEffect());
    }

    public IEnumerator TakeDamageEffect() 
    {
        lens.intensity.value = LensDistortionEffectStartValue;
        while (lens.intensity.value < 0) 
        {
            lens.intensity.value += Time.deltaTime * LensDistortionEffectSpeed;
            yield return null;
        }
    }
}
