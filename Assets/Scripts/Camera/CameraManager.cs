using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Noise Effect")]
    [SerializeField] private float NoiseEffectStrength;
    [SerializeField] private float NoiseEffectDecreaseSpeed;

    [Header("CM Settings")]
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cameraNoise;
    public static CameraManager Instance;

    private void Awake()
    {
        Instance = this;   
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        camera = Camera.main;
        cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator CameraNoiseEffect()
    {
        cameraNoise.m_AmplitudeGain += NoiseEffectStrength;
        cameraNoise.m_FrequencyGain += NoiseEffectStrength;
        while (cameraNoise.m_AmplitudeGain > 0) 
        {
            cameraNoise.m_AmplitudeGain -= NoiseEffectDecreaseSpeed * Time.deltaTime;
            cameraNoise.m_FrequencyGain += NoiseEffectDecreaseSpeed * Time.deltaTime;
            yield return null;
        }
        cameraNoise.m_AmplitudeGain = 0;
        cameraNoise.m_FrequencyGain = 0;
    }
}
