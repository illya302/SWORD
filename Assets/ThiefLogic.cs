using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefLogic : MonoBehaviour, ICreature
{
    [Header("Thief's main settings")]
    [SerializeField] private int healthPoints = 5;
    [SerializeField] private float attackRate;

    [Header("Thief's AI settings")]
    [SerializeField] public float chaseDistance;
    [SerializeField] public float lootingDistance;
    [SerializeField] public float attackDistance;
    [SerializeField] public float scaredDistandce;
    [SerializeField] public float travelDistance;
    [SerializeField] public float travelTimeMax;
    [SerializeField] public float chasingSpeed;
    [SerializeField] public float defaultSpeed;

    [HideInInspector] public GameObject lootTarget;

    [Header("Thief's projectile settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float rotationSpeed;

    [Header("Thief's loot settings")]
    [SerializeField] private float heartStartSpeed;
    [SerializeField] private int heartDropChance;

    [SerializeField] private float experienceStartSpeed;
    [SerializeField] private int experienceCount;

    [Header("Thief's resources")]
    [SerializeField] private GameObject Exp;
    [SerializeField] private GameObject Heart;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject onDeathAudioSource;

    [Header("Thief's upgrade system")]
    [SerializeField] private float projectileSpeedBonus;
    [SerializeField] private float projectileDistanceBonus;
    [SerializeField] private float attackRateBonus;
    [SerializeField] private int projectileDamageBonus;

    private int expSinceLastUpgrade;

    [Header("References")]
    [SerializeField] private AudioClip thiefHit;
    [SerializeField] private AudioClip thiefDeath;
    [SerializeField] private AudioClip thiefAttack;

    private bool IsReayToStrike = true;
    private float timeSinceAttack = 0;

    private bool IsReadyTakeDamage = true;
    private float timeTakeDamage = 0;

    public event Action OnTakeDamage;
    public event Action OnTakeExp;
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

    public void TakeHeal(int heal) 
    {
        healthPoints += heal;
    }
    public void TakeExperience(int exp)
    {
        experienceCount += exp;
        Upgrade(exp);
        OnTakeExp?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (!IsReadyTakeDamage)
            return;

        StartCoroutine(TakeDamageCoolDown());

        lastDamage = damage;
        healthPoints -= damage;
        OnTakeDamage?.Invoke();
        audio.clip = thiefHit;
        audio.Play();

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
            OnDeathAudio();
            GiveExperience(experienceCount);
            GiveHeart();
        }
    }

    private void Upgrade(int exp)
    {
        expSinceLastUpgrade += exp;
        for (int i = 0; i < exp; i++) 
        {
            projectileSpeed += projectileSpeedBonus;
            projectileDistance += projectileDistanceBonus;
            attackRate += attackRateBonus;
        }
        if (expSinceLastUpgrade > 10) 
        {
            expSinceLastUpgrade -= 10;
            projectileDamage += projectileDamageBonus;
        }
    }

    public void Attack()
    {
        if (IsReayToStrike)
        {
            StartCoroutine(Reloading());

            audio.clip = thiefAttack;
            audio.Play();

            Vector3 attackVector = (player.transform.position - transform.position);
            attackVector.Normalize();
            ProjectileLogic spikeProjectile = Instantiate(projectile, transform.position + (Vector3.up * 1.6f) + attackVector * 2.4f, transform.rotation).GetComponent<ProjectileLogic>();
            spikeProjectile.projectileSpeed = projectileSpeed;
            spikeProjectile.projectileDistance = projectileDistance;
            spikeProjectile.projectileDamage = projectileDamage;
            spikeProjectile.rotationSpeed = rotationSpeed;
            spikeProjectile.movementVector = attackVector;
        }
    }

    private void OnDeathAudio()
    {
        AudioSource fx = Instantiate(onDeathAudioSource, transform.position, transform.rotation).GetComponent<AudioSource>();
        fx.clip = thiefDeath;
        fx.Play();
        Destroy(fx.gameObject, fx.clip.length);
    }

    private void GiveExperience(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log(UnityEngine.Random.Range(-1f, 1f));
            Debug.Log(UnityEngine.Random.Range(-1f, 1f));
            Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
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

        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        direction.Normalize();
        float force = UnityEngine.Random.Range(5, heartStartSpeed);
        GameObject exp = Instantiate(Heart, transform.position, transform.rotation);
        exp.GetComponentInChildren<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
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
