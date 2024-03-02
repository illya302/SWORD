using UnityEngine;
using UnityEngine.UI;
public class UIDodgeSliderScript : MonoBehaviour
{
    [SerializeField] private Player player;
    private Slider reloadStatus;

    private void Awake()
    {
        reloadStatus = GetComponentInChildren<Slider>();
        reloadStatus.maxValue = player.dodgeForce;
        reloadStatus.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateSlider();
        SetRotation();
    }
    private void SetRotation() 
    {
        Vector3 vector = new Vector3();
        Quaternion rotation = vector == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(vector);
        transform.rotation = rotation;
    }
    private void UpdateSlider()
    {
        if (player.currentDodgeForce != 0)
        {
            reloadStatus.gameObject.SetActive(true);
            reloadStatus.value = player.currentDodgeForce;
        }
        else
        {
            reloadStatus.gameObject.SetActive(false);
        }
    }
}
