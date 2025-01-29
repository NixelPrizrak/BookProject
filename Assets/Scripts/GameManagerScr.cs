using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScr : MonoBehaviour
{
    public static GameManagerScr Instance;
    public GameObject BountyPref, MenuPref, RatingPref, InfoPanel;
    public Text MoneyTxt, LivesTxt, WavesTxt, TimeTxt;
    public Button TimeSpeedBtn;
    public int GameMoney, LivesCount;
    private float gameSpeed = 1;
    public Sprite FirstGameSpeed, SecondGameSpeed, ThreeGameSpeed;
    private float timeGame;

    public AudioClip LoseSnd, HitSnd, GameSnd;
    public bool TimePause = false, ExistWindow = false;

    void Awake()
    {
        GameControllerScr.AllTowers.Clear();
        GameControllerScr.AllTowers.Add(new Tower("Книга льда", 0, 2, 2f, 10, "TowerSprites/FTower"));
        GameControllerScr.AllTowers.Add(new Tower("Книга природы", 1, 5, 1.2f, 15, "TowerSprites/STower"));
        GameControllerScr.AllTowers.Add(new Tower("Книга камня", 2, 3, 2f, 20, "TowerSprites/TTower"));

        GameControllerScr.AllProjectiles.Clear();
        GameControllerScr.AllProjectiles.Add(new TowerProjectile(10, 10, "ProjectilesSprites/FProjectile"));
        GameControllerScr.AllProjectiles.Add(new TowerProjectile(10, 15, "ProjectilesSprites/SProjectile"));
        GameControllerScr.AllProjectiles.Add(new TowerProjectile(6, 20, "ProjectilesSprites/TProjectile"));

        GameControllerScr.AllEnemies.Clear();
        GameControllerScr.AllEnemies.Add(new Enemy(80, 1, 8, "Animation/FEnemy"));
        GameControllerScr.AllEnemies.Add(new Enemy(50, 1.5f, 5, "Animation/SEnemy"));
        GameControllerScr.Strength = 1;

        Instance = this;
        FindObjectOfType<EnemySpawnerScr>().SpawnCount = 0;
        FindObjectOfType<EnemySpawnerScr>().timeToSpawn = 4;
        InfoPanel.SetActive(false);
    }

    void Update()
    {
        timeGame += Time.deltaTime;
        GameControllerScr.Strength += Time.deltaTime/100;
        MoneyTxt.text = GameMoney.ToString();
        LivesTxt.text = LivesCount.ToString();
        int minutes = Mathf.FloorToInt(timeGame / 60);
        int seconds = Mathf.FloorToInt(timeGame - minutes * 60);
        TimeTxt.text = (minutes == 0 ? "00" : (minutes < 10 ? "0" : "") + minutes) + ":" + (seconds == 0 ? "00" : ((seconds < 10 ? "0" : "") + seconds));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToMenu();
        }
    }

    public void ToMenu()
    {
        gameSpeed = Time.timeScale;
        TimePause = true;
        Time.timeScale = 0;
        GameObject.Find("TimeSpeedBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("MenuBtn").GetComponent<Button>().enabled = false;
        GameObject menuObj = Instantiate(MenuPref);
        menuObj.transform.SetParent(GameObject.Find("MenuLayer").transform, false);
        menuObj.transform.position = new Vector3(0, 0);
    }

    public void CloseMenu()
    {
        Time.timeScale = gameSpeed;
        TimePause = false;
        GameObject.Find("TimeSpeedBtn").GetComponent<Button>().enabled = true;
        GameObject.Find("MenuBtn").GetComponent<Button>().enabled = true;
        Destroy(FindObjectOfType<MenuScr>().gameObject);
    }

    public void ShowBounty(int bounty, Transform trans)
    {
        GameMoney += bounty;
        GameObject bountyObj = Instantiate(BountyPref);
        Vector2 position = new Vector2(trans.position.x + trans.GetComponent<SpriteRenderer>().bounds.size.x / 2,
                                       trans.position.y);
        bountyObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        bountyObj.transform.position = position;

        bountyObj.GetComponent<BountyEffectScr>().SetParams(bounty);
    }

    public void MinusLive()
    {
        LivesCount--;
        if (LivesCount == 0)
        {
            PlayLoseSnd();
            Time.timeScale = 1;

            TimePause = true;
            Time.timeScale = 0;
            GameObject.Find("TimeSpeedBtn").GetComponent<Button>().enabled = false;
            GameObject.Find("MenuBtn").GetComponent<Button>().enabled = false;
            GameObject ratingObj = Instantiate(RatingPref);
            ratingObj.transform.SetParent(GameObject.Find("MenuLayer").transform, false);
            ratingObj.GetComponent<RatingScr>().UserNew = new User(GameControllerScr.UserName, Mathf.RoundToInt(timeGame) + GameObject.Find("EnemySpawner").GetComponent<EnemySpawnerScr>().SpawnCount * 10 * GameControllerScr.Complexity);
            ratingObj.transform.position = new Vector3(0, 0);
        }
    }

    public void PlayLoseSnd()
    {
        GetComponent<AudioSource>().clip = LoseSnd;
        GetComponent<AudioSource>().Play();
    }

    public void PlayHitSnd()
    {
        GetComponent<AudioSource>().clip = HitSnd;
        GetComponent<AudioSource>().Play();
    }

    public void GameSpeedUpBtn()
    {
        if (Time.timeScale >= 3)
        {
            Time.timeScale = 1;
            TimeSpeedBtn.GetComponent<Image>().sprite = FirstGameSpeed;
        }
        else
        {
            Time.timeScale++;
            if (Time.timeScale == 2)
            {
                TimeSpeedBtn.GetComponent<Image>().sprite = SecondGameSpeed;
            }
            else
            {
                TimeSpeedBtn.GetComponent<Image>().sprite = ThreeGameSpeed;
            }
        }
    }
}
