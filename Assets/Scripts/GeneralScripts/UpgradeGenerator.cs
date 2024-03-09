using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGenerator : MonoBehaviour
{
    public static List<Upgrade> upgrades = new List<Upgrade>();

    private static UpgradeGenerator instance;

    [SerializeField] private static int speedStartValue;
    [SerializeField] private static int reloadStartValue;
    [SerializeField] private static int dodgeForceStartValue;
    [SerializeField] private static int helthPosionStartValue;

    [SerializeField] private static int speedStartPrice = 10;
    [SerializeField] private static int reloadStartPrice = 10;
    [SerializeField] private static int dodgeForceStartPrice = 10;
    [SerializeField] private static int helthPosionStartPrice = 10;

    private static int upgradeSpeedTier = 0;
    private static int upgradeReloadTier = 0;
    private static int upgradeDodgeForceTier = 0;
    private static int upgradeHelthPosionTier = 0;
    public static UpgradeGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static void SetupInstance()
    {
        instance = FindObjectOfType<UpgradeGenerator>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "UpgradeGenerator";
            instance = gameObj.AddComponent<UpgradeGenerator>();
            DontDestroyOnLoad(gameObj);
        }
    }
    public static void GenerateUpgrades() 
    {
        for(int i = 0; i < 8; i++) 
        {
            Upgrade upgrade = GenerateUpgrade();
            upgrades.Add(upgrade);
        }
    }
    private static Upgrade GenerateUpgrade() 
    { 
        int upgradeIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Upgrade.UpgradeType)).Length);
        string type = Enum.GetNames(typeof(Upgrade.UpgradeType))[upgradeIndex];

        Upgrade upgrade  = new Upgrade();

        switch (type)
        {
            case "HealPotion":
                upgradeHelthPosionTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.HealPotion, helthPosionStartValue, helthPosionStartPrice * upgradeHelthPosionTier);
                break;
               
            case "HeroSpeed":
                upgradeSpeedTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.HeroSpeed, speedStartValue, speedStartPrice * upgradeSpeedTier);
                break;

            case "DodgeForce":
                upgradeDodgeForceTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.DodgeForce, dodgeForceStartValue, dodgeForceStartPrice * upgradeDodgeForceTier);
                break;

            case "ReloadTime":
                upgradeReloadTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.ReloadTime, reloadStartValue, reloadStartPrice * upgradeReloadTier);
                break;
        }
        return upgrade;
    }
}
