using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellOrUpgradeTowerScr : MonoBehaviour
{
    public TowerScr SelfTower;
    public Text CaptionTxt, UpgradeTxt, SellTxt;
    public Button UpgradeBtn;

    private void Start()
    {
        CaptionTxt.text = SelfTower.SelfTower.Name + " Ур." + SelfTower.Level;
        if (SelfTower.Level == 10)
        {
            UpgradeTxt.text = "Макс. уровень";
        }
        else
        {
            UpgradeTxt.text = "Улучшить за " + SelfTower.CostUpgrade;
        }
        SellTxt.text = "Продать за " + SelfTower.CostSell;
    }

    private void Update()
    {
        if (SelfTower.CostUpgrade > GameManagerScr.Instance.GameMoney || SelfTower.Level == 10)
        {
            UpgradeBtn.interactable = false;
        }
        else
        {
            UpgradeBtn.interactable = true;
        }
    }

    public void Sell()
    {
        GameManagerScr.Instance.ExistWindow = false;
        SelfTower.DestroyTower();

        Cancel();
    }

    public void Upgrade()
    {
        GameManagerScr.Instance.ExistWindow = false;
        SelfTower.SelfTower.Cooldown = Mathf.Round(SelfTower.SelfTower.Cooldown * 0.8f * 10) / 10;
        SelfTower.SelfTower.Range = Mathf.Round(SelfTower.SelfTower.Range * 1.1f * 10) / 10;
        SelfTower.Damage = Mathf.RoundToInt(SelfTower.Damage * 1.2f);
        SelfTower.RadiusTarget.transform.localScale = new Vector3(SelfTower.SelfTower.Range, SelfTower.SelfTower.Range);

        GameManagerScr.Instance.GameMoney -= SelfTower.CostUpgrade;
        SelfTower.CostSell += SelfTower.CostUpgrade / 2;
        SelfTower.CostUpgrade = Mathf.RoundToInt(SelfTower.CostUpgrade * 1.5f);
        SelfTower.Level += 1;

        Cancel();
    }

    public void Cancel()
    {
        GameManagerScr.Instance.ExistWindow = false;
        Destroy(gameObject);
    }
}
