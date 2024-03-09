using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public enum UpgradeType {HealPotion, HeroSpeed, DodgeForce, ReloadTime}
    public UpgradeType _upgradeType;
    public float _value;
    public int _price;

    public Upgrade(UpgradeType upgradeType, float value, int price) 
    {
        _upgradeType = upgradeType;
        _value = value;
        _price = price;
    }
    public Upgrade(){}
}
