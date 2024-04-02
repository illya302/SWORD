using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    private Animator animator;
    private Weapon weapon;
    private TrailRenderer trailRender;

    private const string ATTACK = "Attack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        trailRender = GetComponentInChildren<TrailRenderer>();
    }
    private void Start()
    {
        weapon.OnAttack += StartWeaponAttack;
        if(trailRender != null)
            trailRender.enabled = false;
    }

    private void StartWeaponAttack()
    {
        EventManager.Instance.OnAttack?.Invoke(false);
        animator.SetBool(ATTACK, true);
        if (trailRender != null)
            trailRender.enabled = true;
    }
    private void StopWeaponAttack()
    {
        EventManager.Instance.OnAttack?.Invoke(true);
        animator.SetBool(ATTACK, false);
        if (trailRender != null)
            trailRender.enabled = false;
    }
}
