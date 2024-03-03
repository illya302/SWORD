using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private IWeapon _weapon;
    private Player pl;
    private GameObject activeWeapon;
    private bool IsAwaliablePickUp = true;
    private Coroutine pickUp;
    private Coroutine dropDown;


    private string WEAPON_PICK_UP_CONTAINER = "WeaponPickUpContainer";

    private void Awake()
    {
        pl = GetComponent<Player>(); 
    }

    private void Start()
    {
        InputManager.inputSystem.Player.PickUp.performed += PickUp_performed;

        _weapon = pl._weapon;
        activeWeapon = pl.activeWeapon;
    }

    private void PickUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MonoBehaviour playerWeapon = pl._weapon as MonoBehaviour;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(position, position + Vector2.one * 5);

        if (!IsAwaliablePickUp)
        {
            return;
        }

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.name == WEAPON_PICK_UP_CONTAINER && collider.transform.parent.gameObject != playerWeapon.gameObject)
            {
                playerWeapon.transform.parent = null;
                playerWeapon.transform.position = activeWeapon.transform.position;
                playerWeapon.transform.rotation = activeWeapon.transform.rotation;

                if (pickUp != null)
                    StopCoroutine(pickUp);
                if (dropDown != null)
                    StopCoroutine(dropDown);

                pickUp = StartCoroutine(DropWeapon(playerWeapon.transform));
                dropDown = StartCoroutine(PickUpWeapon(collider.transform.parent.transform));
        
                break;
            }
        }
    }
    private IEnumerator DropWeapon(Transform weapon)
    {
        Vector2 aimingVector = InputManager.Instance.GetAimVector();
        Vector3 dropPosition = transform.position + new Vector3(aimingVector.x, aimingVector.y, 0) * 5;
        Quaternion dropRotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));

        pl._weapon.Drop();
        float currentTime = 0;
        while (1 > currentTime)
        {
            currentTime += Time.deltaTime;
            weapon.position = Vector3.Lerp(weapon.position, dropPosition, Time.deltaTime * 5);
            weapon.rotation = Quaternion.Lerp(weapon.rotation, dropRotation, Time.deltaTime * 5);
            yield return null;
        }
    }

    private IEnumerator PickUpWeapon(Transform weapon)
    {
        IsAwaliablePickUp = false;
        IWeapon newWeapon = weapon.GetComponent<IWeapon>();
        float currentTime = 0;
        while (0.2 > currentTime)
        {
            currentTime += Time.deltaTime;
            weapon.position = Vector3.Lerp(weapon.position, activeWeapon.transform.position, Time.deltaTime * 10);
            weapon.rotation = Quaternion.Lerp(weapon.rotation, activeWeapon.transform.rotation, Time.deltaTime * 10);
            yield return null;
        }
        weapon.GetComponent<IWeapon>().PickUp();
        weapon.parent = activeWeapon.transform;
        pl._weapon = newWeapon;
        IsAwaliablePickUp = true;
    }
    private void OnDestroy()
    {
        InputManager.inputSystem.Player.PickUp.performed -= PickUp_performed;
    }
}
