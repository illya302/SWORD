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
        trailRender.enabled = false;
    }

    private void StartWeaponAttack()
    {
        animator.SetBool(ATTACK, true);
        trailRender.enabled = true;
    }
    private void StopWeaponAttack()
    {
        animator.SetBool(ATTACK, false);
        trailRender.enabled = false;
    }
}
