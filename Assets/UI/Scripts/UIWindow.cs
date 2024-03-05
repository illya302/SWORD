using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    private GameObject content;

    public void SetWindowState(bool state)
    {
        if(content == null)
        {
            content = transform.GetChild(0).gameObject;
        }
        content.SetActive(state);
    }
}
