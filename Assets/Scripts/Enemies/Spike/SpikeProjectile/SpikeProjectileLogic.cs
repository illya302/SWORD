using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProjectileLogic : MonoBehaviour, IDamaging
{
    public float projectileSpeed;
    public float projectileDistance;
    public int projectileDamage;
    public float rotationSpeed;
    public Vector3 movementVector;
    private Vector3 startPosition;

    [SerializeField] private ParticleSystem onDeathParticle;
    [SerializeField] private GameObject audioSource;
    [SerializeField] private AudioClip audio;

    void Awake() 
    {
        startPosition = transform.position;
    }    
    void FixedUpdate() 
    {
        if (Vector3.Distance(startPosition,transform.position) > projectileDistance) 
        {
            UseParticles();
            UseAudio();
            Destroy(gameObject);
        }
        transform.Rotate(0, 0, rotationSpeed);
        transform.position += movementVector * projectileSpeed * 3;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayer target)) 
        {
            UseParticles();
            UseAudio();

            target.TakeDamage(projectileDamage);

            Destroy(gameObject);
        }
    }

    private void UseParticles() 
    {
        ParticleSystem particle = Instantiate(onDeathParticle, transform.position, transform.rotation);
        Destroy(particle.gameObject, particle.main.duration);
    }

    private void UseAudio()
    {
        AudioSource sound = Instantiate(audioSource, transform.position, transform.rotation).GetComponent<AudioSource>();
        sound.clip = audio;
        sound.Play();
        Destroy(sound.gameObject, audio.length);
    }
}
