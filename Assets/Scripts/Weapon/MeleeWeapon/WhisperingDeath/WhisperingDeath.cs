using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WhisperingDeath : Weapon
{
    [SerializeField] private GameObject Poisoning;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable target) && !collision.TryGetComponent(out IPlayer p) && collider.enabled) 
        {
            target.TakeDamage(Damage);
            BuffManager.Instance.PoisoningBuff(target, 1, 4, 1.5f, Poisoning);
        }
        if (collider.enabled && collision.transform.TryGetComponent(out Rigidbody2D rb) && collision.tag != "Player") 
        {
            Vector2 punch = collision.transform.position - transform.position;
            punch.Normalize();
            rb.AddForce(punch * PunchForce, ForceMode2D.Impulse);
        }
    }
}
