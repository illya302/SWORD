using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    public event Action OnDeath;
}
