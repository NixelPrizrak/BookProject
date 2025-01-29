using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerScr : MonoBehaviour
{
    public GameObject Projectile, SellPref, RadiusTarget;
    public Tower SelfTower;
    public TowerType SelfType;
    public TowerBuildScr SelfBuild;
    public Color BaseColor, CurrColor;
    public int Damage, CostUpgrade, CostSell;
    public int Level;

    private void Start()
    {
        SelfTower = GameControllerScr.AllTowers[(int)SelfType];
        GetComponent<SpriteRenderer>().sprite = SelfTower.spr;
        RadiusTarget.transform.localScale = new Vector3(SelfTower.Range, SelfTower.Range);
        RadiusTarget.SetActive(false);
        Damage = GameControllerScr.AllProjectiles[(int)SelfType].Damage;
        CostSell = SelfTower.Price / 2;
        CostUpgrade = SelfTower.Price / 4;
        Level = 1;

        InvokeRepeating("SearchTarget", 0, .1f);
    }


    void Update()
    {
        if (SelfTower.CurrCooldown > 0)
        {
            SelfTower.CurrCooldown -= Time.deltaTime;
        }
    }

    bool CanShoot()
    {
        if (SelfTower.CurrCooldown <= 0)
        {
            return true;
        }
        return false;
    }

    void SearchTarget()
    {
        if (CanShoot())
        {
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float currDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (currDistance <= SelfTower.Range)
                {
                    if (nearestEnemy == null)
                    {
                        nearestEnemy = enemy;
                    }
                    else if (nearestEnemy.GetComponent<EnemyScr>().StageIndex < enemy.GetComponent<EnemyScr>().StageIndex &&
                    nearestEnemy.GetComponent<EnemyScr>().WayIndex < enemy.GetComponent<EnemyScr>().WayIndex)
                    {
                        nearestEnemy = enemy;
                    }
                }
            }

            if (nearestEnemy != null)
                Shoot(nearestEnemy.transform);
        }
    }

    void Shoot(Transform enemy)
    {
        GetComponent<AudioSource>().Play();
        SelfTower.CurrCooldown = SelfTower.Cooldown;

        GameObject proj = Instantiate(Projectile);
        proj.GetComponent<TowerProjectileScr>().SelfTower = this;
        proj.transform.position = transform.position;
        proj.GetComponent<TowerProjectileScr>().SetTarget(enemy);
    }

    public void DestroyTower()
    {
        GameManagerScr.Instance.GameMoney += CostSell;
        SelfBuild.gameObject.SetActive(true);

        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        if (!GameManagerScr.Instance.TimePause && !GameManagerScr.Instance.ExistWindow)
        {
            GetComponent<SpriteRenderer>().color = CurrColor;
            RadiusTarget.SetActive(true);
            GameManagerScr.Instance.InfoPanel.SetActive(true);
            GameObject.Find("LevelText").GetComponent<Text>().text = "Уровень: " + Level;
            GameObject.Find("DamageText").GetComponent<Text>().text = "Урон: " + Damage;
            GameObject.Find("RadiusText").GetComponent<Text>().text = "Дистанция: " + SelfTower.Range;
            GameObject.Find("CooldownText").GetComponent<Text>().text = "Перезарядка: " + SelfTower.Cooldown + "c";
        }
    }

    private void OnMouseExit()
    {
        if (!GameManagerScr.Instance.TimePause && !GameManagerScr.Instance.ExistWindow)
        {
            GetComponent<SpriteRenderer>().color = BaseColor;
            RadiusTarget.SetActive(false);
            GameManagerScr.Instance.InfoPanel.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!GameManagerScr.Instance.TimePause && !GameManagerScr.Instance.ExistWindow)
        {
            GameManagerScr.Instance.ExistWindow = true;
            GameObject towSell = Instantiate(SellPref);
            towSell.transform.SetParent(GameObject.Find("MenuLayer").transform, false);
            float width = GameControllerScr.SizeNormal(towSell.GetComponent<RectTransform>().rect.width);
            float height = GameControllerScr.SizeNormal(towSell.GetComponent<RectTransform>().rect.height);

            if (GameControllerScr.SizeNormal(Screen.width / 2) + transform.position.x - (width + .1f) < 0)
            {
                width = -width;
            }
            if (GameControllerScr.SizeNormal(Screen.height / 2) + transform.position.y - (height + .1f) < 0)
            {
                height = -height;
            }
            towSell.transform.position = new Vector3(transform.position.x - width,
                                                     transform.position.y - height);
            towSell.GetComponent<SellOrUpgradeTowerScr>().SelfTower = this;

            GetComponent<SpriteRenderer>().color = BaseColor;
            RadiusTarget.SetActive(false);
        }
    }
}
