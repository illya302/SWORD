using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFury : Weapon
{
    [SerializeField] private GameObject Fire;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable iDamageable) && !collision.TryGetComponent(out IPlayer p) && collider.enabled) 
        {
            var target = iDamageable as MonoBehaviour;
            iDamageable.TakeDamage(Damage);
            Instantiate(Fire, target.transform);
            //BuffManager.Instance.FlameBuff(target, 1, 2, 1f, flameEffect);
        }
        if (collider.enabled && collision.transform.TryGetComponent(out Rigidbody2D rb) && collision.tag != "Player") 
        {
            Vector2 punch = collision.transform.position - transform.position;
            punch.Normalize();
            rb.AddForce(punch * PunchForce, ForceMode2D.Impulse);
        }
    }
}
