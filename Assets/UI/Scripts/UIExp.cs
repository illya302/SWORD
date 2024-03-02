using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIExp : MonoBehaviour
{
    [SerializeField] private Player player;
    private TMP_Text exp;

    private void Awake()
    {
        exp = GetComponent<TMP_Text>();
        //player = GameObject.FindGameObjectsWithTag("P")
    }

    void Update()
    {
        exp.text = "EXP - " + player.experience;
    }
}
