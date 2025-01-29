using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerBuildScr : MonoBehaviour
{
    public Color BaseColor, CurrColor;

    public GameObject ShopPref, TowerPref, SellPref, RadiusTarget;

    private void Start()
    {
        RadiusTarget.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (!GameManagerScr.Instance.TimePause && !GameManagerScr.Instance.ExistWindow)
        {
            GetComponent<SpriteRenderer>().color = CurrColor;
        }
    }

    private void OnMouseExit()
    {
        if (!GameManagerScr.Instance.TimePause && !GameManagerScr.Instance.ExistWindow)
        {
            GetComponent<SpriteRenderer>().color = BaseColor;
        }
    }

    private void OnMouseDown()
    {
        if (!GameManagerScr.Instance.TimePause && !GameManagerScr.Instance.ExistWindow)
        {
            GameManagerScr.Instance.ExistWindow = true;
            GameObject shopObj = Instantiate(ShopPref);
            shopObj.transform.SetParent(GameObject.Find("MenuLayer").transform, false);
            float width = GameControllerScr.SizeNormal(shopObj.GetComponent<RectTransform>().rect.width);
            float height = GameControllerScr.SizeNormal(shopObj.GetComponent<RectTransform>().rect.height);

            if (GameControllerScr.SizeNormal(Screen.width / 2) + transform.position.x - (width + .1f) < 0)
            {
                width = -width;
            }
            if (GameControllerScr.SizeNormal(Screen.height / 2) + transform.position.y - (height + .1f) < 0)
            {
                height = -height;
            }
            shopObj.transform.position = new Vector3(transform.position.x - width,
                                                     transform.position.y - height);
            shopObj.GetComponent<ShopScr>().SelfBuild = this;

            GetComponent<SpriteRenderer>().color = BaseColor;
        }
    }

    public void BuildTower(Tower tower)
    {
        GameObject tmpTower = Instantiate(TowerPref);
        tmpTower.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tmpTower.transform.position = transform.position;
        tmpTower.GetComponent<TowerScr>().SelfType = (TowerType)tower.Type;
        tmpTower.GetComponent<TowerScr>().SelfBuild = this;
        FindObjectOfType<ShopScr>().CloseShop();
        gameObject.SetActive(false);
    }
}
