using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileScr : MonoBehaviour
{
    Transform target;
    public TowerProjectile SelfProjectile;
    public TowerScr SelfTower;

    void Start()
    {
        SelfProjectile = GameControllerScr.AllProjectiles[SelfTower.SelfTower.Type];

        GetComponent<SpriteRenderer>().sprite = SelfProjectile.Spr;
        SelfProjectile.Damage = SelfTower.Damage;
    }

    void Update()
    {
        Move();
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    private void Move()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) < .1f)
            {
                Hit();
            }
            else
            {

                Vector2 dir = target.position - transform.position;
                transform.Translate(dir.normalized * Time.deltaTime * SelfProjectile.Speed);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Hit()
    {
        switch (SelfTower.SelfTower.Type)
        {
            case (int)TowerType.FIRST_TOWER:
                target.GetComponent<EnemyScr>().StartSlow(3, 0.6f, 1);
                target.GetComponent<EnemyScr>().TakeDamage(SelfProjectile.Damage);
                break;
            case (int)TowerType.SECOND_TOWER:
                target.GetComponent<EnemyScr>().TakeDamage(SelfProjectile.Damage);
                break;
            case (int)TowerType.THIRD_TOWER:
                target.GetComponent<EnemyScr>().AOEDamage(2f, SelfProjectile.Damage);
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
