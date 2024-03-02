using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealParticleVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem onJumpParticleSystem;
    private void UseOnJumpParticles()
    {
        ParticleSystem particle = Instantiate(onJumpParticleSystem, transform.position - new Vector3(0, 0.2f, 0), Quaternion.Euler(-90,0,0));
        particle.Play();
        AudioManager.instance.Play("HealPickUp1");
        Destroy(particle.gameObject, particle.main.duration);
    }
}
