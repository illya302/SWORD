using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    [SerializeField] private int healthPoints;

    public void TakeDamage(int d)
    {
        healthPoints -= d;
    }
}
