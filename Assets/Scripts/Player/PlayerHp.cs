using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHp : MonoBehaviour, IPlayer
{
    private Player player;
    public event Action OnTakeDamage;
    public event Action OnDeath;
    void Awake() 
    {
        player = GetComponentInParent<Player>();
    }
    public void TakeDamage(int damage)
    {
        AudioManager.instance.Play("Loss1");

        OnTakeDamage.Invoke();
        //StartCoroutine(CameraManager.Instance.CameraNoiseEffect());
        //StartCoroutine(GlobalVolumeManager.Instance.TakeDamageEffect());
        player.healthPoints -= damage;
    }
    public void TakeHeal(int heal)
    {
        AudioManager.instance.Play("Up2");
        player.healthPoints += heal;
    }
}
