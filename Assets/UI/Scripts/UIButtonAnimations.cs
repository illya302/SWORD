using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAnimations : MonoBehaviour
{
    public void SizeChange(float size)
    {
        GetComponent<RectTransform>().localScale = new Vector2(size, size);
    }
}
