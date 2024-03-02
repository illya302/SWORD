using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Attack(Player sender);
    public void PickUp();
    public void Drop();
}
