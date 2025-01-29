using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemScr : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Tower selfTower;
    TowerBuildScr selfBuild;
    public Image TowerImage;
    public Text TowerName, TowerDamage, TowerCooldown, TowerPrice;
    private bool active = true;

    public Color BaseColor, CurrColor, NoActiveColor;

    private void Update()
    {
        if (selfTower.Price > GameManagerScr.Instance.GameMoney)
        {
            if (active)
            {
                GetComponent<Image>().color = NoActiveColor;
            }
            active = false;
        }
        else
        {
            if (!active)
            {
                GetComponent<Image>().color = BaseColor;
            }
            active = true;
        }
    }

    public void SetStartData(Tower tower, TowerBuildScr build)
    {
        selfTower = tower;
        TowerImage.sprite = tower.spr;
        TowerName.text = tower.Name;
        TowerDamage.text = "Урон: " + GameControllerScr.AllProjectiles[tower.Type].Damage;
        TowerCooldown.text = "Перезарядка: " + tower.Cooldown+"c";
        TowerPrice.text = tower.Price.ToString();
        selfBuild = build;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.TimePause && active)
        {
            GetComponent<Image>().color = CurrColor;
            selfBuild.GetComponent<TowerBuildScr>().RadiusTarget.transform.localScale = new Vector3(selfTower.Range, selfTower.Range);
            selfBuild.GetComponent<TowerBuildScr>().RadiusTarget.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.TimePause && active)
        {
            GetComponent<Image>().color = BaseColor;
        }
        selfBuild.GetComponent<TowerBuildScr>().RadiusTarget.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.TimePause && active)
        {
            if (GameManagerScr.Instance.GameMoney >= selfTower.Price)
            {
                GameManagerScr.Instance.ExistWindow = false;
                selfBuild.BuildTower(selfTower);

                GameManagerScr.Instance.GameMoney -= selfTower.Price;
                selfBuild.GetComponent<TowerBuildScr>().RadiusTarget.SetActive(false);
            }
        }
    }
}
