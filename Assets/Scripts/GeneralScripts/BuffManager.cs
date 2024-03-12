using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.VFX;

public class BuffManager : MonoBehaviour
{
    private static BuffManager instance;
    public static BuffManager Instance
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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<BuffManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "BuffManager";
            instance = gameObj.AddComponent<BuffManager>();
            DontDestroyOnLoad(gameObj);
        }
    }

    //public void FlameBuff(IDamageable target)
    //{
    //    var targ = target as MonoBehaviour;
    //}

    //private IEnumerator Fire(IDamageable target, int damage, int quantity, float timeInterval) 
    //{
    //    while (quantity > 0) 
    //    {
    //        var currentTime = timeInterval;
    //        while (currentTime > 0)
    //        {
    //            currentTime -= Time.deltaTime;
    //            yield return null;
    //        }
    //        if (target != null)
    //        {
    //            target.TakeDamage(damage);
    //            quantity -= 1;
    //        }
    //        else 
    //        {
    //            break;
    //        }
    //    }
    //}
}
