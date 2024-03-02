using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IMeleeWeapon
{
    [SerializeField] protected int Damage;
    [SerializeField] protected int PunchForce;
    [SerializeField] protected string Sound;
    [SerializeField] protected GameObject Hand;
    [SerializeField] protected TrailRenderer Trail;
    protected Animator animator;
    protected new Collider2D collider;
    protected SpriteRenderer spriteRenderer;
    public event Action OnAttack;
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        collider.enabled = false;
    }
    public void Attack(Player s)
    {
        AudioManager.instance.Play(Sound);
        collider.enabled = true;
        OnAttack.Invoke();
    }
    private void DisableDamage()
    {
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
        Trail.enabled = false;
        spriteRenderer.sortingOrder = 0;
    }
}