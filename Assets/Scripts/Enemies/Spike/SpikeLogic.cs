using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeLogic : MonoBehaviour, ICreature, IDamageable
{
    [Header("Spike's main settings")]
    [SerializeField] private int healthPoints = 2;
    [SerializeField] private float attackRate;

    [Header("Spike's AI settings")]
    [SerializeField] public float chaseDistance;
    [SerializeField] public float attackDistance;
    [SerializeField] public float scaredDistandce;
    [SerializeField] public float travelDistance;
    [SerializeField] public float travelTimeMax;
    [SerializeField] public float chasingSpeed;
    [SerializeField] public float defaultSpeed;

    [Header("Spike's projectile settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float rotationSpeed;

    [Header("Spike's loot settings")]
    [SerializeField] private float heartStartSpeed;
    [SerializeField] private int heartDropChance;

    [SerializeField] private float experienceStartSpeed;
    [SerializeField] private int experienceCount;

    [Header("Spike's resources")]
    [SerializeField] private GameObject Exp;
    [SerializeField] private GameObject Heart;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject onDeathAudioSource;

    [SerializeField] private AudioClip spikeHit;
    [SerializeField] private AudioClip spikeDeath;
    [SerializeField] private AudioClip spikeAttack;

    private bool IsReayToStrike = true;
    private float timeSinceAttack = 0;

    private bool IsReadyTakeDamage = true;
    private float timeTakeDamage = 0;

    public event Action OnTakeDamage;
    private NavMeshAgent agent;
    private GameObject player;
    private AudioSource audio;
    public int lastDamage;

    private void Awake()
    {
        player = GameObject.Find("Hero");
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        audio = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        if (!IsReadyTakeDamage)
            return;

        StartCoroutine(TakeDamageCoolDown());

        lastDamage = damage;
        healthPoints -= damage;
        OnTakeDamage.Invoke();
        audio.clip = spikeHit;
        audio.Play();

        if (healthPoints <= 0) 
        {
            Destroy(gameObject);
            OnDeathAudio();
            GiveExperience(experienceCount);
            GiveHeart();
        }
    }

    public void Attack() 
    {
        if (IsReayToStrike)
        {
            StartCoroutine(Reloading());

            audio.clip = spikeAttack;
            audio.Play();

            Vector3 attackVector = (player.transform.position - transform.position);
            attackVector.Normalize();
            SpikeProjectileLogic spikeProjectile = Instantiate(projectile, transform.position + (Vector3.up * 1.6f) + attackVector * 2.4f, transform.rotation).GetComponent<SpikeProjectileLogic>();
            spikeProjectile.projectileSpeed = projectileSpeed;
            spikeProjectile.projectileDistance = projectileDistance;
            spikeProjectile.projectileDamage = projectileDamage;
            spikeProjectile.rotationSpeed = rotationSpeed;
            spikeProjectile.movementVector = attackVector;
        }
    }

    private void GiveExperience(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            Vector2 direction = new Vector2(UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1, 1));
            direction.Normalize();
            float force = UnityEngine.Random.Range(10, experienceStartSpeed);
            GameObject exp = Instantiate(Exp, transform.position, transform.rotation);
            exp.GetComponentInChildren<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
    private void GiveHeart()
    {
        if (UnityEngine.Random.Range(0, 100) > heartDropChance)
            return;
  
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        direction.Normalize();
        float force = UnityEngine.Random.Range(5, heartStartSpeed);
        GameObject exp = Instantiate(Heart, transform.position, transform.rotation);
        exp.GetComponentInChildren<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
    }
    private void OnDeathAudio() 
    {
        AudioSource fx = Instantiate(onDeathAudioSource, transform.position, transform.rotation).GetComponent<AudioSource>();
        fx.clip = spikeDeath;
        fx.Play();
        Destroy(fx.gameObject, fx.clip.length);
    }

    private IEnumerator Reloading() 
    {
        IsReayToStrike = false;
        while (1 / attackRate > timeSinceAttack) 
        {
            timeSinceAttack += Time.deltaTime;
            yield return null;
        }
        timeSinceAttack = 0;
        IsReayToStrike = true;
    }
    private IEnumerator TakeDamageCoolDown()
    {
        IsReadyTakeDamage = false;
        while (0.3 > timeTakeDamage)
        {
            timeTakeDamage += Time.deltaTime;
            yield return null;
        }
        timeTakeDamage = 0;
        IsReadyTakeDamage = true;
    }
}
