using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;
    private static Player player;
    public static UpgradeManager Instance
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
            SetupUpgradeManager();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<UpgradeManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "UpgradeManager";
            instance = gameObj.AddComponent<UpgradeManager>();
            SetupUpgradeManager();
            DontDestroyOnLoad(gameObj);
        }
    }

    private static void SetupUpgradeManager()
    {
        player = GameObject.Find("Hero").GetComponent<Player>();
    }

    public static bool UpgradePlayer(Upgrade upgrade) 
    {
        if (upgrade._price > player.experience)
        {
            return false;
        }
        else 
        {
            player.experience -= upgrade._price;
        }   

        switch (upgrade._upgradeType) 
        {
            case Upgrade.UpgradeType.HealPotion:
                player.healthPoints += (int)upgrade._value;
                break;
            case Upgrade.UpgradeType.ReloadTime:
                player.reloadTime -= upgrade._value;
                break;
            case Upgrade.UpgradeType.DodgeForce:
                player.dodgeForce += (int)upgrade._value;
                break;
            case Upgrade.UpgradeType.HeroSpeed:
                player.speed += (int)upgrade._value;
                break;
        }
        return true;
    }
}


