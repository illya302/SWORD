using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGenerator : MonoBehaviour
{
    public static List<Upgrade> upgrades = new List<Upgrade>();

    private static UpgradeGenerator instance;

    private static float speedStartValue = 0.5f;
    private static float reloadStartValue = 0.1f;
    private static float dodgeForceStartValue = 0.5f;
    private static int helthPosionStartValue = 5;

    private static int speedStartPrice = 5;
    private static int reloadStartPrice = 5;
    private static int dodgeForceStartPrice = 5;
    private static int helthPosionStartPrice = 5;

    private int upgradeSpeedTier = 0;
    private int upgradeReloadTier = 0;
    private int upgradeDodgeForceTier = 0;
    private int upgradeHelthPosionTier = 0;
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
            SetupUpgradeGenerator();
            //DontDestroyOnLoad(gameObject);
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
            Instance.SetupUpgradeGenerator();
            //DontDestroyOnLoad(gameObj);
        }
    }
    private void SetupUpgradeGenerator()
    {
        upgradeSpeedTier = 0;
        upgradeReloadTier = 0;
        upgradeDodgeForceTier = 0;
        upgradeHelthPosionTier = 0;
    }

    public static void GenerateUpgrades() 
    {
        for(int i = 0; i < 8; i++) 
        {
            Upgrade upgrade = GenerateUpgrade();
            upgrades.Add(upgrade);
        }
    }
    public static Upgrade GenerateUpgrade() 
    { 
        int upgradeIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Upgrade.UpgradeType)).Length);
        string type = Enum.GetNames(typeof(Upgrade.UpgradeType))[upgradeIndex];

        Upgrade upgrade  = new Upgrade();

        switch (type)
        {
            case "HealPotion":
                Instance.upgradeHelthPosionTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.HealPotion, helthPosionStartValue, helthPosionStartPrice * Instance.upgradeHelthPosionTier);
                break;
               
            case "HeroSpeed":
                Instance.upgradeSpeedTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.HeroSpeed, speedStartValue, speedStartPrice * Instance.upgradeSpeedTier);
                break;

            case "DodgeForce":
                Instance.upgradeDodgeForceTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.DodgeForce, dodgeForceStartValue, dodgeForceStartPrice * Instance.upgradeDodgeForceTier);
                break;

            case "ReloadTime":
                Instance.upgradeReloadTier += 1;
                upgrade = new Upgrade(Upgrade.UpgradeType.ReloadTime, reloadStartValue, reloadStartPrice * Instance.upgradeReloadTier);
                break;
        }
        return upgrade;
    }
}
