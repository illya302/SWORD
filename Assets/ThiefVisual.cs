using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem onTakeDamageParticleSystem;
    [SerializeField] private ParticleSystem onDeathDamageParticleSystem;
    [SerializeField] private GameObject onTakeDamageText;

    private Material material;
    private ThiefLogic thiefLogic;

    private void Awake()
    {
        thiefLogic = GetComponent<ThiefLogic>();
        material = GetComponent<SpriteRenderer>().material;
    }
    private void Start()
    {
        thiefLogic.OnTakeDamage += ThiefVisual_OnTakeDamage;
        thiefLogic.OnTakeExp += ThiefLogic_OnTakeExp;
    }

    private void ThiefLogic_OnTakeExp()
    {
        float currentIntencity = material.GetFloat("_Intencity");
        material.SetFloat("_Intencity", currentIntencity + 1);
    }

    private void ThiefVisual_OnTakeDamage()
    {
        ParticleSystem particleSystem = Instantiate(onTakeDamageParticleSystem, transform.position, transform.rotation);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
        UseDamageText();
    }

    private void UseDamageText()
    {
        onTakeDamageText.GetComponentInChildren<UIDamage>().damage = -thiefLogic.lastDamage;
        Instantiate(onTakeDamageText, transform.position + new Vector3(0, 4, 0), transform.rotation);
    }

    private void OnDestroy()
    {
        ParticleSystem particleSystem = Instantiate(onDeathDamageParticleSystem, transform.position, transform.rotation);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }
}
