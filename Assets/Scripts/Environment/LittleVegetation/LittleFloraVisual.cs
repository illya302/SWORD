using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleFloraVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    private Coroutine activeCoroutine;

    private const string DAMAGE = "Damage";
    private const string ENTER = "Enter";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamaging>(out IDamaging d))
        {
            animator.SetBool(DAMAGE, true);
        }
        if (collision.TryGetComponent<ICreature>(out ICreature creature))
        {
            animator.SetBool(ENTER, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICreature>(out ICreature creature))
        {
            animator.SetBool(ENTER, true);
        }
    }
    private void DisableAnimation()
    {
        animator.SetBool(DAMAGE, false);
        animator.SetBool(ENTER, false);
    }
}
