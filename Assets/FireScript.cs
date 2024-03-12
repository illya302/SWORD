using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class FireScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private VisualEffect effect;
    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        effect = GetComponent<VisualEffect>();
    }
    private void Start()
    {
        Debug.Log(spriteRenderer.bounds.size);
        if (spriteRenderer != null)
            effect.SetVector3("ObjectSize", spriteRenderer.bounds.size*0.5f);
    }
}
