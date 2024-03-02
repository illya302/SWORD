using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeVisual : MonoBehaviour
{
    [SerializeField] private float transparencySpeed;
    [SerializeField] private float transparencyLevel;
    [SerializeField] private SpriteRenderer[] spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem ShakeEffect;
    private Coroutine activeCoroutine;

    private const string DAMAGE = "Damage";
    private const string ENTER = "Enter";

    private void Awake()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ShakeEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p) && spriteRenderer.Length != 0) 
        {
            if (activeCoroutine != null) 
            {
                StopCoroutine(activeCoroutine);
            } 
            activeCoroutine = StartCoroutine(makeTreeTransparent());
        }
        if (collision.TryGetComponent(out IDamaging d)) 
        {
            animator.SetBool(DAMAGE, true);
            ShakeEffect?.Play();
        }
        if (collision.TryGetComponent(out ICreature creature))
        {
            animator.SetBool(ENTER, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p))
        {
            if (activeCoroutine != null && spriteRenderer.Length != 0)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(makeTreeOpaque());
        }
        if (collision.TryGetComponent(out ICreature creature))
        {
            animator.SetBool(ENTER, true);
        }
    }
    private IEnumerator makeTreeTransparent() 
    {
        Color color = new Color(1, 1, 1, spriteRenderer[0].color.a);

        while (spriteRenderer[0].color.a > transparencyLevel)
        {
            color.a -= Time.deltaTime * transparencySpeed;
            foreach (SpriteRenderer renderer in spriteRenderer)
            {
                renderer.color = color;
            }
            yield return null;
        }
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            renderer.color = new Color(1, 1, 1, transparencyLevel);
        }  
    }
    private IEnumerator makeTreeOpaque()
    {
        Color color = new Color(1, 1, 1, spriteRenderer[0].color.a);

        while (spriteRenderer.Length != 0 && spriteRenderer[0].color.a < 1)
        {
            color.a += Time.deltaTime * transparencySpeed;
            foreach (SpriteRenderer renderer in spriteRenderer)
            {
                renderer.color = color;
            }
            yield return null;
        }
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            renderer.color = new Color(1, 1, 1, 1);
        }
    }

    private void DisableAnimation() 
    {
        animator.SetBool(DAMAGE, false);
        animator.SetBool (ENTER, false);
    }
}
