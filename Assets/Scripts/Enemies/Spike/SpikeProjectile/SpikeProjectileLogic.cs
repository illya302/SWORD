using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProjectileLogic : MonoBehaviour, IDamaging
{
    [SerializeField] private ParticleSystem onDeathParticle;
    public float projectileSpeed;
    public float projectileDistance;
    public int projectileDamage;
    public float rotationSpeed;
    public Vector3 movementVector;
    private Vector3 startPosition;
    void Awake() 
    {
        startPosition = transform.position;
    }    
    void Update() 
    {
        if (Vector3.Distance(startPosition,transform.position) > projectileDistance) 
        {
            Destroy(gameObject);
        }
        transform.Rotate(0, 0, rotationSpeed);
        transform.position += movementVector * projectileSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPlayer>(out IPlayer target)) 
        {
            target.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        AudioManager.instance.Play("Hit4");
        ParticleSystem particle = Instantiate(onDeathParticle,transform.position,transform.rotation);
        Destroy(particle.gameObject, particle.main.duration);
    }
}
