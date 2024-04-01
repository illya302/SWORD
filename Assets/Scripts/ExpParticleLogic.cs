using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpParticleLogic : MonoBehaviour
{
    [SerializeField] private float attractionForce;
    [SerializeField] private float attractionDistance;
    [SerializeField] private int experienceAmount;
    [SerializeField] private ParticleSystem particleSystem;
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
        if (collision.TryGetComponent(out Player pl)) 
        {
            //pl.experience += experienceAmount;
            pl.TakeExperience(experienceAmount);
            UseParticles(true);
            Destroy(gameObject);
        }
        if (collision.TryGetComponent(out ThiefLogic thief))
        {
            thief.TakeExperience(experienceAmount);
            UseParticles(false);
            Destroy(gameObject);
        }
    }
    private void UseParticles(bool IsPlayer) 
    {
        ParticleSystem particle = Instantiate(particleSystem, transform.position, transform.rotation);
        particle.Play();
        if (IsPlayer)
            AudioManager.instance.Play("Exp3");
        Destroy(particle.gameObject, particle.main.duration);
    }
}
