using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeLogic : MonoBehaviour, ICreature, IDamageable
{
    [SerializeField] private int healthPoints = 2;
    [SerializeField] private float attackRate;

    [SerializeField] public float chaseDistance;
    [SerializeField] public float attackDistance;
    [SerializeField] public float scaredDistandce;
    [SerializeField] public float travelDistance;
    [SerializeField] public float travelTimeMax;
    [SerializeField] public float chasingSpeed;
    [SerializeField] public float defaultSpeed;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float heartStartSpeed;
    [SerializeField] private int heartDropChance;

    [SerializeField] private float experienceStartSpeed;
    [SerializeField] private int experienceCount;

    [SerializeField] private GameObject Exp;
    [SerializeField] private GameObject Heart;
    [SerializeField] private GameObject projectile;

    private bool IsReayToStrike = true;
    private float timeSinceAttack = 0;

    public event Action OnTakeDamage;
    private NavMeshAgent agent;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Hero");
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        OnTakeDamage.Invoke();
        AudioManager.instance.Play("Hit1");
        if (healthPoints <= 0) 
        {
            Destroy(gameObject);
            AudioManager.instance.Play("Explosion");
            GiveExperience(experienceCount);
            GiveHeart();
        }
    }

    public void Attack() 
    {
        if (IsReayToStrike)
        {
            IsReayToStrike = false;
            StartCoroutine(Reloading());

            AudioManager.instance.Play("Shoot4");

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

    private IEnumerator Reloading() 
    {
        while (1 / attackRate > timeSinceAttack) 
        {
            timeSinceAttack += Time.deltaTime;
            yield return null;
        }
        timeSinceAttack = 0;
        IsReayToStrike = true;
    }
}
