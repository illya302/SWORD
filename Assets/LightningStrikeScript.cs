using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class LightningStrikeScript : MonoBehaviour
{
    [SerializeField] private int countOfLightningPoints;
    [SerializeField] private float widthOfLightning;

    [SerializeField] private float lightningDestroySpeed;

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject magicSource;
    [SerializeField] private GameObject magicTarget;
    [SerializeField] private GameObject effect;

    private List<Vector3> lightningPointsList;
    private List<LineRenderer> activeLightnings;

    private void Start()
    {
        lightningPointsList = new List<Vector3>();
        activeLightnings = new List<LineRenderer>();
    }

    private void Update()
    {
        followByWeapon();
    }

    public void Attack()
    {
        LineRenderer ligtning = Instantiate(lightningPrefab, MagicStaffPos(), transform.rotation).GetComponent<LineRenderer>();
        ligtning.positionCount = countOfLightningPoints;
        ligtning.startWidth = widthOfLightning;
        ligtning.endWidth = widthOfLightning;

        Vector3 startPosition = new Vector3(MagicStaffPos().x, MagicStaffPos().y, 0);
        Vector3 endPosition = new Vector3(InputManager.Instance.GetMousePosition().x, InputManager.Instance.GetMousePosition().y, 1);

        float length = Vector3.Distance(startPosition, endPosition) / 5;

        Vector2 aimVector = endPosition - startPosition;
        aimVector.Normalize();
        Vector3 perpendicularVector = Quaternion.Euler(0, 0, 90) * aimVector;


        for (int i = 0; i < countOfLightningPoints - 2; i++)
        {
            Vector3 vector = Vector3.Lerp(startPosition, endPosition, Random.Range(0f, 1f));
            vector += perpendicularVector * Random.Range(-length, length);
            lightningPointsList.Add(vector);
        }

        lightningPointsList.Add(startPosition);
        lightningPointsList.Add(endPosition);
        Vector3[] sortedVectors = lightningPointsList.OrderBy(v => v.z).ToArray();

        ligtning.SetPositions(sortedVectors.ToArray());
        lightningPointsList.Clear();

        activeLightnings.Add(ligtning);

        PlayEffect(endPosition);

        SetMagicTarget(endPosition);

        StartCoroutine(DestroyLightning(ligtning));      
    }

    private void followByWeapon() 
    {
        foreach (var lightning in activeLightnings) 
        {
            if (lightning == null)
                return;

            lightning.SetPosition(0, MagicStaffPos());
        }
    }

    private Vector3 MagicStaffPos()
    {
        return magicSource.transform.position;
    }

    private void SetMagicTarget(Vector3 position)
    {
        magicTarget.transform.position = position;
        magicTarget.GetComponent<Collider2D>().enabled = true;
    }
    private void ResetMagicTarget()
    {
        magicTarget.transform.position = transform.position;
        magicTarget.GetComponent<Collider2D>().enabled = false;
    }
    private void PlayEffect(Vector3 position) 
    {
        VisualEffect fx = Instantiate(effect, position, transform.rotation).GetComponent<VisualEffect>();
        fx.Play();
        Destroy(fx.gameObject, 2);
    }

    private IEnumerator DestroyLightning(LineRenderer lightning) 
    {
        while (lightning.startWidth > 0)
        {
            lightning.startWidth -= Time.deltaTime * lightningDestroySpeed;
            lightning.endWidth -= Time.deltaTime * lightningDestroySpeed;
            yield return null;
        }
        activeLightnings.Remove(lightning);
        ResetMagicTarget();
        Destroy(lightning.gameObject);
    }
}
