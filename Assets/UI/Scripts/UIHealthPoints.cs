using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPoints : MonoBehaviour
{
    [SerializeField] private Player player;
    private TMP_Text health;

    private void Awake()
    {
        health = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        health.text = player.healthPoints.ToString();
    }
}
