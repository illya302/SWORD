using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIWindow
{
    [SerializeField] Slider progressSlider;

    private void Awake()
    {
        SetWindowState(false);
    }

    public void SetProgressSlider(float progress)
    {
        progressSlider.value = progress;
    }
}
