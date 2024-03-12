using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamage : MonoBehaviour
{
    public int damage;
    private TMP_Text tpmText;
    private Rigidbody2D rb;
    private void Awake()
    {
        tpmText = GetComponent<TMP_Text>();
        rb = GetComponent<Rigidbody2D>();
        tpmText.text = damage.ToString();
        StartCoroutine(Life());
    }
    public IEnumerator Life() 
    {
        Vector3 movementVector =  new Vector3 (Random.Range(-0.8f,0.8f), 1, 0);
        movementVector.Normalize();
        rb.AddForce(movementVector * 15f, ForceMode2D.Impulse);
        while (tpmText.alpha > 0) 
        {
            tpmText.alpha -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
