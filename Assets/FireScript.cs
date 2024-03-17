using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class FireScript : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int quantity;
    [SerializeField] private float interval;

    private int thisDamage;
    private int thisQuantity;
    private float thisInterval;

    private bool IsAvaliableToExpand = false;

    private SpriteRenderer parentSpriteRenderer;
    private VisualEffect effect;
    private Collider2D collider;
    private Light2D light;
    private IDamageable parent;
    private void Awake()
    {
        parentSpriteRenderer = GetComponentInParent<SpriteRenderer>();
        effect = GetComponent<VisualEffect>();
        collider = GetComponent<Collider2D>();
        light = GetComponentInChildren<Light2D>();
        parent = GetComponentInParent<IDamageable>();
    }
    private void Start()
    {
        thisDamage = damage;
        thisQuantity = quantity;
        thisInterval = interval;
        if (parentSpriteRenderer != null) 
        {
            effect.SetVector3("ObjectSize", parentSpriteRenderer.bounds.size * 0.5f);
            transform.position += new Vector3(0, parentSpriteRenderer.bounds.size.y / 2, 0);
        }
        StartCoroutine(Fire());
        parent.OnDeath += Parent_OnDeath;
    }

    private void Parent_OnDeath()
    {
        if (this != null)
            transform.parent = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsAvaliableToExpand)
            return;
        if (collision.TryGetComponent(out IDamageable iDamageable) && collision.GetComponentInChildren<FireScript>() == null)
        {
            var target = iDamageable as MonoBehaviour;
            IsAvaliableToExpand = false;
            GameObject fire = Instantiate(this, target.transform).gameObject;
            fire.transform.position = target.transform.position;
        }
    }

    private IEnumerator OffLight() 
    {
        while (light.intensity > 0) 
        {
            light.intensity -= Time.deltaTime * 1.8f;
            yield return null;
        }
    }

    private IEnumerator Fire()
    {
        while (thisQuantity > 0)
        {
            var currentTime = thisInterval;
            while (currentTime > 0)
            {
                if (transform.parent == null)
                    break;
                currentTime -= Time.deltaTime;
                yield return null;
            }
            if (transform.parent == null)
                break;
            parent.TakeDamage(thisDamage);
            thisQuantity -= 1;
            IsAvaliableToExpand = true;
        }
        StartCoroutine(OffLight());
        StartCoroutine(StopEffect(effect));
    }
    private IEnumerator StopEffect(VisualEffect effect)
    {
        effect.Stop();
        while (effect.aliveParticleCount > 0)
        {
            yield return null;
        }
        Destroy(effect.gameObject);
    }
}
