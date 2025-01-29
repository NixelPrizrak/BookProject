using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RatingScr : MonoBehaviour
{
    public GameObject ScoreGrid, UserScorePref;
    public Color BaseColor, NewUserColor;
    public User UserNew;
    public string ButtonText = "Главное меню";

    void Start()
    {
        transform.Find("MainMenuBtn").Find("Text").GetComponent<Text>().text = ButtonText;
        string json = "";
        if (File.Exists(Application.dataPath + "/UsersScore.json"))
        {
            using (StreamReader reader = new StreamReader(Application.dataPath + "/UsersScore.json"))
            {
                json = reader.ReadToEnd();
            }
        }
        List<User> users = new List<User>();
        UserList us = JsonUtility.FromJson<UserList>(json);
        if (us != null)
        {
            users = us.Users;
        }
        if (UserNew != null)
        {
            users.Add(UserNew);
        }
        if (users.Count == 0)
        {
            GameObject.Find("InfoText").SetActive(true);
        }
        else
        {
            GameObject.Find("InfoText").SetActive(false);
            users.Sort((us1, us2) => us2.Score - us1.Score);
            for (int i = 0; i < users.Count; i++)
            {
                CreateUserScore(users[i], i + 1, users[i] == UserNew ? NewUserColor : BaseColor);
            }
            if (UserNew != null)
            {
                if (users.Count > 10)
                {
                    users.RemoveAt(10);
                }
                json = JsonUtility.ToJson(new UserList(users));

                using (StreamWriter writer = new StreamWriter(Application.dataPath + "/UsersScore.json"))
                {
                    writer.Write(json);
                }
            }
        }
    }

    public void CreateUserScore(User user, int num, Color color)
    {
        GameObject userObj = Instantiate(UserScorePref);
        userObj.transform.SetParent(ScoreGrid.transform, false);
        userObj.transform.Find("NumberText").GetComponent<Text>().text = "№" + num;
        userObj.transform.Find("NameText").GetComponent<Text>().text = user.Name;
        userObj.transform.Find("ScoreText").GetComponent<Text>().text = "Очки: " + user.Score;
        userObj.GetComponent<Image>().color = color;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("menu");
    }
}
