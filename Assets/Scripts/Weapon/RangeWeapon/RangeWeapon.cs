using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    [SerializeField] protected int Damage;
    [SerializeField] protected int PunchForce;
    [SerializeField] protected string Sound;
    [SerializeField] protected Vector3 idleAngle = Vector3.zero;
    [SerializeField] protected float attackSpeedModifier = 1f;
    [SerializeField] protected GameObject Hand;
    protected Animator animator;
    protected new Collider2D collider;
    protected SpriteRenderer spriteRenderer;

    public Vector3 IdleAngle { get => idleAngle; set => idleAngle = value; }
    public float AttackSpeedModifier { get => attackSpeedModifier; set => attackSpeedModifier = value; }

    public event Action OnAttack;
    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (collider != null)
            collider.enabled = false;
    }
    public void Attack(Player s)
    {
        AudioManager.instance.Play(Sound);
        if (collider != null)
            collider.enabled = true;
        OnAttack.Invoke();
    }
    private void DisableDamage()
    {
        if (collider != null)
            collider.enabled = false;
    }
    public void PickUp()
    {
        Hand.SetActive(true);
        animator.enabled = true;
        spriteRenderer.sortingOrder = 10;
    }
    public void Drop()
    {
        Hand.SetActive(false);
        animator.enabled = false;
        spriteRenderer.sortingOrder = 0;
    }
}
