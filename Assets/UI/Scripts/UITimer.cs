using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private float startTime;
    private float currentTime;
    private TMP_Text timer;
    void Awake()
    {
        timer = GetComponent<TMP_Text>();
    }

    void Update()
    {
        timer.text = Time.timeSinceLevelLoad.ToString("F2");
    }
}
