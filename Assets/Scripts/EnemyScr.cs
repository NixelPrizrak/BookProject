using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScr : MonoBehaviour
{
    public List<GameObject> WayPoints = new List<GameObject>();

    public Enemy SelfEnemy;

    public int WayIndex = 0, PathIndex, StageIndex;

    public Color NoHPCol, FullHPCol, BaseColor, SlowColor;
    public Image HealthBar;
    public float Health;

    void Start()
    {
        WayPoints = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnerScr>().StartWayPoints[PathIndex];
        StageIndex = 1;

        GetComponent<Animator>().runtimeAnimatorController = SelfEnemy.Spr;
    }

    void Update()
    {
        Health = SelfEnemy.Health;
        Move();
    }

    private void Move()
    {
        Transform currWayPoint = WayPoints[WayIndex].transform;
        Vector3 currWayPos = new Vector3(currWayPoint.position.x + currWayPoint.GetComponent<SpriteRenderer>().bounds.size.x / 2,
            currWayPoint.position.y - currWayPoint.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        Vector3 dir = currWayPos - transform.position;
        if (dir.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            dir.x = -dir.x;
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        transform.Translate(dir.normalized * Time.deltaTime * SelfEnemy.Speed);

        if (Vector3.Distance(transform.position, currWayPos) < 0.3f)
        {
            if (WayIndex < WayPoints.Count - 1)
            {
                WayIndex++;
            }
            else
            {
                if (StageIndex == 1)
                {
                    int rand = Random.Range(0, 2);

                    var points = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnerScr>().SecondWayPoints;

                    if (rand == 1)
                    {
                        StageIndex = 2;
                        WayPoints = new List<GameObject>();
                        WayPoints.AddRange(points[2]);

                        if (PathIndex == 1)
                        {
                            PathIndex = 0;
                            WayPoints.Reverse();
                        }
                        else
                        {
                            PathIndex = 1;
                        }
                    }
                    else
                    {
                        StageIndex = 3;
                        WayPoints = points[PathIndex];
                    }
                }
                else if (StageIndex == 3)
                {
                    StageIndex = 4;
                    WayPoints = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnerScr>().EndWayPoints;
                }
                else if (StageIndex == 2)
                {
                    StageIndex = 3;
                    WayPoints = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnerScr>().SecondWayPoints[PathIndex];
                }
                else
                {
                    GameManagerScr.Instance.MinusLive();
                    Destroy(gameObject);
                }
                WayIndex = 0;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        SelfEnemy.Health -= damage;
        StopCoroutine("HealthBarUpdate");

        StartCoroutine(HealthBarUpdate(SelfEnemy.Health + damage));
        //HealthBar.fillAmount = selfEnemy.health / selfEnemy.MaxHealth;
        //HealthBar.color = Color.Lerp(NoHPCol, FullHPCol, HealthBar.fillAmount);

        CheckIsAlive();
    }

    IEnumerator HealthBarUpdate(float oldHP)
    {
        float procentHP = SelfEnemy.MaxHealth / 100;
        while (true)
        {
            oldHP -= procentHP;

            if (oldHP < SelfEnemy.Health)
            {
                HealthBar.fillAmount = SelfEnemy.Health / SelfEnemy.MaxHealth;
                HealthBar.color = Color.Lerp(NoHPCol, FullHPCol, HealthBar.fillAmount);
                break;
            }

            HealthBar.fillAmount = oldHP / SelfEnemy.MaxHealth;
            HealthBar.color = Color.Lerp(NoHPCol, FullHPCol, HealthBar.fillAmount);

            yield return new WaitForSeconds(0.005f);
        }
    }

    public void CheckIsAlive()
    {
        if (SelfEnemy.Health <= 0)
        {
            GameManagerScr.Instance.ShowBounty(SelfEnemy.Bounty, transform);
            GameManagerScr.Instance.PlayHitSnd();
            Destroy(gameObject);
        }
    }

    public void StartSlow(float duration, float slowValue, float range)
    {
        List<EnemyScr> enemies = new List<EnemyScr>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector2.Distance(transform.position, go.transform.position) <= range)
            {
                if (go != gameObject)
                {
                    enemies.Add(go.GetComponent<EnemyScr>());
                }
            }
        }

        enemies.Add(this);
        foreach (EnemyScr es in enemies)
        {
            es.GetComponent<SpriteRenderer>().color = SlowColor;
            es.StopCoroutine("GetSlow");
            es.SelfEnemy.Speed = SelfEnemy.StartSpeed;
            es.StartCoroutine(es.GetSlow(duration, slowValue));
        }
    }

    IEnumerator GetSlow(float duration, float slowValue)
    {
        SelfEnemy.Speed = SelfEnemy.Speed * (1 - slowValue);
        yield return new WaitForSeconds(duration);
        SelfEnemy.Speed = SelfEnemy.StartSpeed;
        GetComponent<SpriteRenderer>().color = BaseColor;
    }

    public void AOEDamage(float range, float damage)
    {
        List<EnemyScr> enemies = new List<EnemyScr>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector2.Distance(transform.position, go.transform.position) <= range)
            {
                if (go != gameObject)
                {
                    enemies.Add(go.GetComponent<EnemyScr>());
                }
            }
        }

        TakeDamage(damage);
        foreach (EnemyScr es in enemies)
        {
            es.TakeDamage(damage);
        }
    }
}
