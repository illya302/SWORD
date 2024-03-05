using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponVisual : MonoBehaviour
{
    private Animator animator;
    private RangeWeapon weapon;

    private const string ATTACK = "Attack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weapon = GetComponent<RangeWeapon>();
    }
    private void Start()
    {
        weapon.OnAttack += StartWeaponAttack;
    }

    private void StartWeaponAttack()
    {
        animator.SetBool(ATTACK, true);
    }
    private void StopWeaponAttack()
    {
        animator.SetBool(ATTACK, false);
    }
}
