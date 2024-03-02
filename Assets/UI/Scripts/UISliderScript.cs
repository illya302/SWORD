using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class UISliderScript : MonoBehaviour
{
    [SerializeField] private Player player;
    private Slider reloadStatus;

    private void Awake()
    {
        reloadStatus = GetComponentInChildren<Slider>();
        reloadStatus.maxValue = player.reloadTime;
        reloadStatus.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateSlider();
        SetRotation();
    }

    private void SetRotation()
    {
        Vector3 vector = new Vector3();
        Quaternion rotation = vector == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(vector);
        transform.rotation = rotation;
    }
    private void UpdateSlider()
    {
        if (player.currentTime != 0)
        {
            reloadStatus.gameObject.SetActive(true);
            reloadStatus.value = player.currentTime;
        }
        else
        {
            reloadStatus.gameObject.SetActive(false);
        }
    }
}
