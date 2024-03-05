using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMagicStaff : RangeWeapon
{
    private LightningStrikeScript lightningStrike;

    private void Awake()
    {
        lightningStrike = GetComponent<LightningStrikeScript>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponentInChildren<Collider2D>();
    }
    private void Start()
    {
        OnAttack += LightningMagicStaff_OnAttack;
    }

    private void LightningMagicStaff_OnAttack()
    {
        lightningStrike.Attack();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable target) && !collision.TryGetComponent(out IPlayer p) && collider.enabled)
        {
            target.TakeDamage(Damage);
        }
        if (collider.enabled && collision.transform.TryGetComponent(out Rigidbody2D rb) && collision.tag != "Player")
        {
            Vector2 punch = collision.transform.position - transform.position;
            punch.Normalize();
            rb.AddForce(punch * PunchForce, ForceMode2D.Impulse);
        }
    }
}
