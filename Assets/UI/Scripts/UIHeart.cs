using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class UIHeart : MonoBehaviour
{
    [SerializeField] private float heartAttackSpeed = 0.1f;
    [SerializeField] private Player player;
    private Animator animator;
    private int health;

    private const string DAMAGE = "Damage";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        health = player.healthPoints;
    }

    private void Update()
    {
        if (player.healthPoints < health) 
        { 
            animator.SetTrigger(DAMAGE);
            animator.speed += 1;
            StartCoroutine(HeartAttack());
        }
        health = player.healthPoints;
    }
    private IEnumerator HeartAttack() 
    { 
        while (animator.speed > 1) 
        {
            animator.speed -= Time.deltaTime * heartAttackSpeed;
            yield return null; 
        }
        animator.speed = 1;
    }
}
