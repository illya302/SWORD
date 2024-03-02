using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem onTakeDamageParticleSystem;
    [SerializeField] private ParticleSystem onDeathDamageParticleSystem;

    private SpikeLogic spikeLogic;

    private void Awake()
    {
        spikeLogic = GetComponent<SpikeLogic>();
    }
    private void Start()
    {
        spikeLogic.OnTakeDamage += SpikeVisual_OnTakeDamage;
    }

    private void SpikeVisual_OnTakeDamage()
    {
        ParticleSystem particleSystem = Instantiate(onTakeDamageParticleSystem, transform.position, transform.rotation);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }

    private void OnDestroy()
    {
        ParticleSystem particleSystem = Instantiate(onDeathDamageParticleSystem, transform.position, transform.rotation);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }
}
