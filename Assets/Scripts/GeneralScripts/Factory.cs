using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;

    [SerializeField] private GameObject surface;
    [SerializeField] private float spawnDelay;
    private float currentTime;
    private bool IsReady = true;

    private void Update()
    {
        if (IsReady) 
        {
            StartCoroutine(Delay());
            IsReady = false;
            Vector2 spawnPosition = new Vector2 (Random.Range(-1 * surface.transform.localScale.x / 2, surface.transform.localScale.x / 2), Random.Range(-1 * surface.transform.localScale.y / 2, surface.transform.localScale.y / 2));
            if (Random.Range(0f, 100f) > 20)
            {
                Instantiate(enemy1, spawnPosition, transform.rotation);
            }
            else 
            {
                Instantiate(enemy2, spawnPosition, transform.rotation);
            }
        }
    }

    private IEnumerator Delay() 
    {
        while (currentTime < spawnDelay) 
        {
            currentTime += Time.deltaTime; 
            yield return null;
        }
        currentTime = 0;
        IsReady = true;
    }
}
