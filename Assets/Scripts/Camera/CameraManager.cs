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

    [Header("Camera Zoom Effect")]
    [SerializeField] private float ZoomEffectSpeed;
    [SerializeField] private float ZoomEffectMaxSize;
    [SerializeField] private float DefaultCameraSize;
    private Coroutine ZoomEffectCoroutine;

    [Header("Camera Dutch Effect")]
    [SerializeField] private float DutchEffectSpeed;
    [SerializeField] private float DutchEffectForce;
    private Coroutine DutchEffectCoroutine;

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
    public IEnumerator CameraZoomEffect()
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        while (virtualCamera.m_Lens.OrthographicSize < ZoomEffectMaxSize)
        {
            float coefficient = 1 - ((virtualCamera.m_Lens.OrthographicSize - startSize)/(ZoomEffectMaxSize - startSize));
            virtualCamera.m_Lens.OrthographicSize += Time.deltaTime * ZoomEffectSpeed * coefficient;
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = ZoomEffectMaxSize;
    }
    public IEnumerator CameraUnzoomEffect()
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        while (virtualCamera.m_Lens.OrthographicSize > DefaultCameraSize)
        {
            float coefficient = 1 - ((virtualCamera.m_Lens.OrthographicSize - startSize) / (DefaultCameraSize - startSize));
            virtualCamera.m_Lens.OrthographicSize -= Time.deltaTime * ZoomEffectSpeed * coefficient;
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = DefaultCameraSize;
    }
    private void Update()
    {
        CameraZoom();
    }
    private void CameraZoom() 
    {
        if (InputManager.Instance.GetInputVector() != Vector2.zero)
        {
            if (ZoomEffectCoroutine != null)
                StopCoroutine(ZoomEffectCoroutine);
            ZoomEffectCoroutine = StartCoroutine(CameraZoomEffect());
        }
        else
        {
            if (ZoomEffectCoroutine != null)
                StopCoroutine(ZoomEffectCoroutine);
            ZoomEffectCoroutine = StartCoroutine(CameraUnzoomEffect());
        }
    }
}
