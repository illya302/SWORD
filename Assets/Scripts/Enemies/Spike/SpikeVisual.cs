using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem onTakeDamageParticleSystem;
    [SerializeField] private ParticleSystem onDeathDamageParticleSystem;
    [SerializeField] private GameObject onTakeDamageText;

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
        UseDamageText();
    }

    private void UseDamageText()
    {
        onTakeDamageText.GetComponentInChildren<UIDamage>().damage = -spikeLogic.lastDamage;
        Instantiate(onTakeDamageText, transform.position + new Vector3(0, 4, 0), transform.rotation);
    }

    private void OnDestroy()
    {
        ParticleSystem particleSystem = Instantiate(onDeathDamageParticleSystem, transform.position, transform.rotation);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }
}
