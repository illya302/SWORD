using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonLogic : MonoBehaviour
{
    private Upgrade _upgrade;
    private TMP_Text _titleText;
    private TMP_Text _costText;
    private TMP_Text _bonusText;

    private void Awake()
    {
        _titleText = transform.Find("UpgradeTitleText").GetComponent<TMP_Text>();
        _costText = transform.Find("UpgradeCostText").GetComponent<TMP_Text>();
        _bonusText = transform.Find("UpgradeBonusText").GetComponent<TMP_Text>();

        _upgrade = UpgradeGenerator.GenerateUpgrade();
    }
    private void Start()
    {
        _titleText.text = _upgrade._upgradeType.ToString();
        _costText.text = _upgrade._price.ToString();
        _bonusText.text = _upgrade._value.ToString();
    }
    public void OnClick() 
    {
        if (!UpgradeManager.Instance.UpgradePlayer(_upgrade))
            return;

        _upgrade = UpgradeGenerator.GenerateUpgrade();
        _titleText.text = _upgrade._upgradeType.ToString();
        _costText.text = _upgrade._price.ToString();
        _bonusText.text = _upgrade._value.ToString();
    }
}
