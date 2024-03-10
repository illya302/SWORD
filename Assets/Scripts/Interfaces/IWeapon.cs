using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public float AttackSpeedModifier { get; set; }
    public Vector3 IdleAngle {get; set;}
    void Attack(Player sender);
    public void PickUp();
    public void Drop();
}
