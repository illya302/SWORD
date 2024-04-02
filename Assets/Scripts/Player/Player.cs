using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour
{
    [SerializeField] public int healthPoints;
    [SerializeField] public int experience;
    [SerializeField] public float speed;
    [SerializeField] public float dodgeForce;
    [SerializeField] public float defaultReloadTime;
    [HideInInspector] public float reloadTime;


    [HideInInspector] private bool IsAvaliableAttack = true;
    [HideInInspector] public float currentTime;
    [HideInInspector] public float currentDodgeForce;
    [HideInInspector] private Coroutine dodgeForceCoroutine;
    [HideInInspector] public event Action OnDeath;
    [HideInInspector] public event Action OnTakeDamage;

    private Rigidbody2D rb;
    public IWeapon _weapon;

    [SerializeField] public GameObject activeWeapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _weapon = GetComponentInChildren<IWeapon>();
    }

    private void Start()
    {
        InputManager.inputSystem.Player.Attack.started += Attack_started;
        InputManager.inputSystem.Player.Dodge.performed += Dodge_performed;
        InputManager.inputSystem.Player.Dodge.canceled += Dodge_canceled;

        _weapon?.PickUp();
    }

    private void Update()
    {
        HpCheck();
        Move();
    }

    private void Dodge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        dodgeForceCoroutine = StartCoroutine(DodgeForce());
    }

    private void Dodge_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StopCoroutine(dodgeForceCoroutine);
        Vector2 aimVector = InputManager.Instance.GetAimVector();
        rb.AddForce(aimVector*currentDodgeForce, ForceMode2D.Impulse);
        
        if (currentDodgeForce > dodgeForce*0.2)
        {
            AudioManager.instance.Play("Dodge1");
        }

        currentDodgeForce = 0;
    }

    private void Attack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsAvaliableAttack)
            return;
        _weapon.Attack(this);
        StartCoroutine(AttackCooldown());
    }

    private void HpCheck() 
    {
        if (healthPoints <= 0) 
        {
            OnDeath.Invoke();
            Destroy(gameObject);
        }
    }

    public void Move()
    {
        Vector2 movementVector = InputManager.Instance.GetInputVector();

        transform.position += new Vector3(movementVector.x, movementVector.y, transform.position.z) * speed * Time.deltaTime;

        //Debug.Log(Vector3.Magnitude(new Vector3(movementVector.x, movementVector.y, transform.position.z) * speed));

        //if (movementVector != Vector2.zero)
        //{
        //    rb.AddForce(movementVector * 5, ForceMode2D.Force);
        //    if (rb.velocity.magnitude > speed)
        //    {
        //        rb.velocity = rb.velocity.normalized * speed;
        //    }
        //}
        //else 
        //{
        //    rb.AddForce(movementVector * -5, ForceMode2D.Force);
        //}

        //rb.AddForce(movementVector * speed, ForceMode2D.Force); speed 1
        //rb.MovePosition(transform.position += new Vector3(movementVector.x, movementVector.y, transform.position.z) * speed * Time.deltaTime);
    }

    //Temporary upgrade system
    public void TakeExperience(int i)
    {
        experience += i;
        //for (int d = 0; d < i; d++) 
        //{
        //    speed += 0.01f;
        //    if (defaultReloadTime > 0.5f)
        //        defaultReloadTime -= 0.01f;
        //}
    }

    private IEnumerator AttackCooldown() 
    {
        reloadTime = defaultReloadTime * _weapon.AttackSpeedModifier;
        IsAvaliableAttack = false;
        while (currentTime < reloadTime) 
        { 
            currentTime += Time.deltaTime;
            yield return null;
        }
        currentTime = 0;
        IsAvaliableAttack = true;
    }
    private IEnumerator DodgeForce() 
    {
        while (true) 
        {
            if (currentDodgeForce < dodgeForce)
            {
                currentDodgeForce += Time.deltaTime;
            }
            else if (currentDodgeForce > dodgeForce)
            {
                currentDodgeForce = dodgeForce;
            }
            yield return null;
        }
    }
    private void OnDestroy()
    {
        InputManager.inputSystem.Player.Attack.started -= Attack_started;
        InputManager.inputSystem.Player.Dodge.performed -= Dodge_performed;
        InputManager.inputSystem.Player.Dodge.canceled -= Dodge_canceled;
    }
}
