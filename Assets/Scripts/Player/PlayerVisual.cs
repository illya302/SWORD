using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player pl;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Light2D light;
    private static GameObject activeWeapon;
    private int healthPoints;
    private bool IsEnableRotation = true;

    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        pl = GetComponent<Player>();
        activeWeapon = GameObject.Find("ActiveWeapon");
        light = GetComponentInChildren<Light2D>();
    }
    private void Start()
    {
        healthPoints = pl.healthPoints;
        EventManager.Instance.OnAttack.AddListener(SwitchRotationAvailability);
    }

    private void Update()
    {
        SetPlayerVisual();
    }

    private void SetPlayerVisual() 
    {
        if(IsEnableRotation)
            SetPlayerRotation();
        SetAnimatorRun();
        SetPlayerTakeDamageEffect();
    }
    private void SetAnimatorRun()
    {
        Vector2 direction = InputManager.Instance.GetInputVector();
        
        if (direction != Vector2.zero)
        {
            animator.SetBool(IS_RUNNING, true);
        }
        else if (direction == Vector2.zero)
        {
            animator.SetBool(IS_RUNNING, false);
        }
    }
    private void SetPlayerTakeDamageEffect() 
    {
        if (pl.healthPoints < healthPoints) 
        {
            ParticleSystem ps = Instantiate(particleSystem, transform.position, transform.rotation);
            Destroy(ps.gameObject, ps.main.duration);
        }
        healthPoints = pl.healthPoints;
    }
    private void SetPlayerRotation()
    {
        float rotation = 0;
        Vector3 mouse = InputManager.Instance.GetMousePosition();
        Vector2 aimingVector = InputManager.Instance.GetAimVector();  
        float angle = Vector2.Angle(aimingVector, Vector2.right);
        activeWeapon.transform.position = Vector3.Lerp(activeWeapon.transform.position, transform.position + new Vector3(0, 1.5f, 0) + new Vector3(aimingVector.x, aimingVector.y, 0), Time.deltaTime * 5); //0.02
        if (mouse.y < gameObject.transform.position.y + 1.5f)
        {
            angle = -angle;
        }

        if (mouse.x > gameObject.transform.position.x)
        {
            rotation = 0;
        }
        else if (mouse.x < gameObject.transform.position.x)
        {
            rotation = 180;
            angle = 180 - angle;
        }
        activeWeapon.transform.rotation = Quaternion.Lerp(activeWeapon.transform.rotation, Quaternion.Euler(0, rotation, angle), Time.deltaTime * 5);//0.02
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), Time.deltaTime*10);//0.05
        light.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    private void SwitchRotationAvailability(bool IsAvailable) 
    {
        IsEnableRotation = IsAvailable;
    }
}
