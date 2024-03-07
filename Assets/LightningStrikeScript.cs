using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class LightningStrikeScript : MonoBehaviour
{
    [SerializeField] private int countOfLightningPoints;
    [SerializeField] private int countOfChildLightningPoints = 2;
    [SerializeField] private float widthOfLightning;
    [SerializeField] private int lightningJumpDistance;
    [SerializeField] private float lightningDestroySpeed;

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject magicTarget;
    [SerializeField] private GameObject magicSource;
    [SerializeField] private GameObject effect;

    private LightningMagicStaff magicStaff;
    private List<Vector3> lightningPointsList;
    private List<LineRenderer> activeLightnings;
    private List<Collider2D> activeTargets;
    private int currentCountOfLightningPoints;

    private Coroutine destroyCoroutine;

    private void Start()
    {
        lightningPointsList = new List<Vector3>();
        activeLightnings = new List<LineRenderer>();
        activeTargets = new List<Collider2D>();
        magicStaff = GetComponent<LightningMagicStaff>();
        currentCountOfLightningPoints = countOfLightningPoints;
    }

    private void Update()
    {
        //followByWeapon();
    }

    public void Attack()
    {

        Vector3 startPosition = new Vector3(MagicStaffPos().x, MagicStaffPos().y, 0);
        Vector3 endPosition = new Vector3(InputManager.Instance.GetMousePosition().x, InputManager.Instance.GetMousePosition().y, 1);

        LightningStrike(startPosition, endPosition);
    }

    public void LightningStrike(Vector3 startPosition, Vector3 endPosition)
    {
        if (activeLightnings.Count != 0)
            currentCountOfLightningPoints = countOfChildLightningPoints;

        LineRenderer ligtning = Instantiate(lightningPrefab, MagicStaffPos(), transform.rotation).GetComponent<LineRenderer>();
        ligtning.positionCount = currentCountOfLightningPoints;
        ligtning.startWidth = widthOfLightning;
        ligtning.endWidth = widthOfLightning;

        float width = Vector3.Distance(startPosition, endPosition) / 5;
        Vector2 aimVector = endPosition - startPosition;
        aimVector.Normalize();
        Vector3 perpendicularVector = Quaternion.Euler(0, 0, 90) * aimVector;


        for (int i = 0; i < currentCountOfLightningPoints - 2; i++)
        {
            Vector3 vector = Vector3.Lerp(startPosition, endPosition, Random.Range(0f, 1f));
            vector += perpendicularVector * Random.Range(-width, width);
            lightningPointsList.Add(vector);
        }
        lightningPointsList.Add(startPosition);
        lightningPointsList.Add(endPosition);
        Vector3[] sortedVectors = lightningPointsList.OrderBy(v => v.z).ToArray();
        ligtning.SetPositions(sortedVectors.ToArray());
        lightningPointsList.Clear();

        activeLightnings.Add(ligtning);

        SetMagicTargetPos(endPosition);

        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(endPosition, endPosition + new Vector3(1,1,0) * lightningJumpDistance);
        List<Collider2D> colliders = new List<Collider2D>();
        foreach (Collider2D collider in hitColliders) 
        {
            if (collider.TryGetComponent(out IDamageable idamageable) && !activeTargets.Contains(collider)) 
            {
                colliders.Add(collider);
            }
        }

        Collider2D[] sortedColiders = colliders.OrderBy(v => Vector3.Distance(endPosition, v.transform.position)).ToArray();

        if (sortedColiders.Length != 0)
        {
            magicStaff.OnTriggerEnter2D(sortedColiders[0]);
            activeTargets.Add(sortedColiders[0]);
            LightningStrike(endPosition, sortedColiders[0].transform.position);
        }

        PlayEffect(endPosition);

        if (destroyCoroutine == null)
        {
            destroyCoroutine = StartCoroutine(DestroyLightning());
        } 
    }
    private void followByWeapon() 
    {
        if (activeLightnings.Count == 0)
            return;
        activeLightnings[0].SetPosition(0, MagicStaffPos());   
    }

    private Vector3 MagicStaffPos()
    {
        return magicSource.transform.position;
    }
    private void SetMagicTargetPos(Vector3 position) 
    {
        if (position == transform.position)
        {
            magicTarget.GetComponent<Collider2D>().enabled = false;
        }
        else 
        {
            magicTarget.GetComponent<Collider2D>().enabled = true;
        }
        magicTarget.transform.position = position;
    }
    private void PlayEffect(Vector3 position) 
    {
        VisualEffect fx = Instantiate(effect, position, transform.rotation).GetComponent<VisualEffect>();
        fx.Play();
        Destroy(fx.gameObject, 2);
    }

    private IEnumerator DestroyLightning() 
    {
        while (activeLightnings[0].startWidth > 0)
        {
            foreach (var lightning in activeLightnings) 
            {
                lightning.startWidth -= Time.deltaTime * lightningDestroySpeed;
                lightning.endWidth -= Time.deltaTime * lightningDestroySpeed;
            }
            yield return null;
        }
        foreach (var lightning in activeLightnings) 
        {
            Destroy(lightning.gameObject);
        }
        activeLightnings.Clear();
        activeTargets.Clear();
        destroyCoroutine = null;
        currentCountOfLightningPoints = countOfLightningPoints;
        SetMagicTargetPos(transform.position);
    }
}
