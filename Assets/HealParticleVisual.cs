using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HealParticleVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem onJumpParticleSystem;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void UseOnJumpParticles()
    {
        ParticleSystem particle = Instantiate(onJumpParticleSystem, transform.position - new Vector3(0, 0.27f, 0), Quaternion.Euler(-90,0,0));
        particle.Play();
        audioSource.Play();
        Destroy(particle.gameObject, particle.main.duration);
    }
}
