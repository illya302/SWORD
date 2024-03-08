using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfView : MonoBehaviour
{
    private GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Hero"); 
    }
    private void Update()
    {
        Vector3 position = (player.transform.position + (InputManager.Instance.GetMousePosition() + player.transform.position)/2)/2;
        transform.position = position; 
    }
}
