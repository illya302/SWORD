using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealParticleLogic : MonoBehaviour
{
    [SerializeField] private float attractionForce;
    [SerializeField] private float attractionDistance;
    [SerializeField] private int healthPointAmount = 1;
    [SerializeField] private ParticleSystem onDestroyParticleSystem;
    private GameObject player;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < attractionDistance)
        {
            Vector3 direction = player.transform.position - transform.position;
            rb.AddForce(direction * attractionForce * (attractionDistance - distance) / distance);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHp pl))
        {
            pl.TakeHeal(healthPointAmount);
            UseOnDestroyParticles();
            Destroy(gameObject);
        }
    }
    private void UseOnDestroyParticles()
    {
        ParticleSystem particle = Instantiate(onDestroyParticleSystem, transform.position, transform.rotation);
        particle.Play();
        AudioManager.instance.Play("Exp1");
        Destroy(particle.gameObject, particle.main.duration);
    }
}
