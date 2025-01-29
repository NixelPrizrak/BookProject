using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScr : MonoBehaviour
{
    public GameObject ItemPref;
    public Transform ItemGrid;

    public TowerBuildScr SelfBuild;

    void Start()
    {
        foreach (Tower tower in GameControllerScr.AllTowers)
        {
            GameObject tmpItem = Instantiate(ItemPref);
            tmpItem.transform.SetParent(ItemGrid, false);
            tmpItem.GetComponent<ShopItemScr>().SetStartData(tower, SelfBuild);
        }
    }

    public void CloseShop()
    {
        GameManagerScr.Instance.ExistWindow = false;
        Destroy(gameObject);
    }
}
