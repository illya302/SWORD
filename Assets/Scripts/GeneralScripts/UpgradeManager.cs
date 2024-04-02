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
            instance.SetupUpgradeManager();
            DontDestroyOnLoad(gameObj);
        }
    }

    private void SetupUpgradeManager()
    {
        player = GameObject.Find("Hero").GetComponent<Player>();
        Debug.Log(player);
    }

    public bool UpgradePlayer(Upgrade upgrade) 
    {
        if (player == null)
            player = GameObject.Find("Hero").GetComponent<Player>();
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
                if (player.defaultReloadTime > 0)
                    player.defaultReloadTime -= upgrade._value;
                break;
            case Upgrade.UpgradeType.DodgeForce:
                player.dodgeForce += upgrade._value;
                break;
            case Upgrade.UpgradeType.HeroSpeed:
                player.speed += upgrade._value;
                break;
        }
        return true;
    }
}


