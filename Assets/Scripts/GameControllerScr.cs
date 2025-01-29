using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Tower
{
    public string Name;
    public int Type, Price;
    public float Range, Cooldown, CurrCooldown;
    public Sprite spr;

    public Tower(string name, int type, float range, float cd, int price, string path)
    {
        Name = name;
        Type = type;
        Range = range;
        Cooldown = cd;
        Price = price;
        spr = Resources.Load<Sprite>(path);
        CurrCooldown = 0;
    }
}

public struct TowerProjectile
{
    public float Speed;
    public int Damage;
    public Sprite Spr;

    public TowerProjectile(float speed, int dmg, string path)
    {
        Speed = speed;
        Damage = dmg;
        Spr = Resources.Load<Sprite>(path);
    }
}

public struct Enemy
{
    public float Health, MaxHealth, Speed, StartSpeed;
    public int Bounty;
    public RuntimeAnimatorController Spr;

    public Enemy(float health, float speed, int bounty, string path)
    {
        Health = MaxHealth = health * (GameControllerScr.Complexity + 1) / 2;
        Speed = speed;
        StartSpeed = speed;
        Bounty = bounty * (GameControllerScr.Complexity + 2) / 3;

        Spr = Resources.Load<RuntimeAnimatorController>(path);
    }
}
[Serializable]
public class User
{
    public string Name;
    public int Score;

    public User(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
[Serializable]
public class UserList
{
    public List<User> Users = new List<User>();

    public UserList(List<User> users)
    {
        Users = users;
    }
}

public enum TowerType
{
    FIRST_TOWER,
    SECOND_TOWER,
    THIRD_TOWER
}

public static class GameControllerScr
{
    public static List<Tower> AllTowers = new List<Tower>();
    public static List<TowerProjectile> AllProjectiles = new List<TowerProjectile>();
    public static List<Enemy> AllEnemies = new List<Enemy>();
    public static string UserName = "User";
    public static int Complexity = 1;
    public static float Strength = 1;

    public static float SizeNormal(float size)
    {
        return (float)Screen.width / Screen.height * Camera.main.orthographicSize * (size / Screen.width);
    }
}
