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
            DontDestroyOnLoad(gameObject);
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

    public void PoisoningBuff(IDamageable target, int damage, int quantity, float timeInterval, GameObject visualEffect)
    {
        if (target is Tree)
            return;
        var targ = target as MonoBehaviour;
        GameObject effect = Instantiate(visualEffect, targ.transform);
        //Destroy(effect, timeInterval * quantity);
        effect.GetComponent<VisualEffect>().Play();
        StartCoroutine(instance.IntervalDamage(target, damage, quantity, timeInterval, effect.GetComponent<VisualEffect>()));
    }
    private IEnumerator IntervalDamage(IDamageable target, int damage, int quantity, float timeInterval, VisualEffect effect)
    {
        void Target_OnDeath()
        {
            if (effect != null)
                effect.gameObject.transform.parent = null;
        }
        target.OnDeath += Target_OnDeath;
        
        while (quantity > 0)
        {
            var currentTime = timeInterval;
            while (currentTime > 0)
            {
                if (effect.gameObject.transform.parent == null)
                    break;
                currentTime -= Time.deltaTime;
                yield return null;
            }
            if (effect.gameObject.transform.parent == null)
                break;
            target.TakeDamage(damage);
            quantity -= 1;
        }
        StartCoroutine(StopEffect(effect));
    }

    private IEnumerator StopEffect(VisualEffect effect) 
    {
        effect.Stop();
        while (effect.aliveParticleCount > 0) 
        {
            yield return null;
        }
        Destroy(effect.gameObject);
    }
}
