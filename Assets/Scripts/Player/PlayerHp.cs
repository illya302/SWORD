using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHp : MonoBehaviour, IPlayer
{
    private Player player;
    void Awake() 
    {
        player = GetComponentInParent<Player>();
    }
    public void TakeDamage(int damage)
    {
        AudioManager.instance.Play("Loss1");
        StartCoroutine(CameraManager.Instance.CameraNoiseEffect());
        player.healthPoints -= damage;
    }
    public void TakeHeal(int heal)
    {
        AudioManager.instance.Play("Up2");
        player.healthPoints += heal;
    }
}
