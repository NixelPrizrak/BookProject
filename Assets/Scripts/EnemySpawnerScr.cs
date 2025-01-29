using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerScr : MonoBehaviour
{
    public float timeToSpawn = 4;
    public int SpawnCount = 0;

    public GameObject EnemyPref;
    public List<GameObject> StartWayPoints1 = new List<GameObject>();
    public List<GameObject> StartWayPoints2 = new List<GameObject>();
    public List<GameObject> SecondWayPoints1 = new List<GameObject>();
    public List<GameObject> SecondWayPoints2 = new List<GameObject>();
    public List<GameObject> SecondWayPoints3 = new List<GameObject>();
    public List<GameObject> EndWayPoints = new List<GameObject>();

    public List<GameObject>[] StartWayPoints = new List<GameObject>[2];
    public List<GameObject>[] SecondWayPoints = new List<GameObject>[3];

    void Start()
    {
        StartWayPoints[0] = StartWayPoints1;
        StartWayPoints[1] = StartWayPoints2;
        SecondWayPoints[0] = SecondWayPoints1;
        SecondWayPoints[1] = SecondWayPoints2;
        SecondWayPoints[2] = SecondWayPoints3;
    }

    void Update()
    {
        if (timeToSpawn <= 0)
        {
            StartCoroutine(SpawnEnemy((SpawnCount + 2) / 2));
            timeToSpawn = 4;
        }

        timeToSpawn -= Time.deltaTime;
    }


    IEnumerator SpawnEnemy(int enemyCount)
    {
        SpawnCount++;
        GameManagerScr.Instance.WavesTxt.text = "Волна: " + SpawnCount;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject tmpEnemy = Instantiate(EnemyPref);
            tmpEnemy.transform.SetParent(gameObject.transform, false);

            tmpEnemy.GetComponent<EnemyScr>().SelfEnemy = GameControllerScr.AllEnemies[Random.Range(0, GameControllerScr.AllEnemies.Count)];
            tmpEnemy.GetComponent<EnemyScr>().SelfEnemy.Health = tmpEnemy.GetComponent<EnemyScr>().SelfEnemy.MaxHealth = tmpEnemy.GetComponent<EnemyScr>().SelfEnemy.Health * GameControllerScr.Strength;
            tmpEnemy.GetComponent<EnemyScr>().PathIndex = Random.Range(0, 2);

            Transform startCellPos = StartWayPoints[tmpEnemy.GetComponent<EnemyScr>().PathIndex][0].transform;
            Vector3 startPos = new Vector3(startCellPos.position.x + startCellPos.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                startCellPos.position.y + startCellPos.GetComponent<SpriteRenderer>().bounds.size.y / 2);

            tmpEnemy.transform.position = startPos;

            yield return new WaitForSeconds(0.6f);
        }


    }
}
