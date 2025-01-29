using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScr : MonoBehaviour
{
    public GameObject Object, Object2, HelpPref;

    public void PlayBtn()
    {
        GameObject.Find("PlayBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("RatingBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("SettingBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("QuitBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("HelpBtn").GetComponent<Button>().enabled = false;
        GameObject complexObj = Instantiate(Object2);
        complexObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        complexObj.transform.position = new Vector3(0, 0);
        complexObj.transform.Find("NameText").GetComponent<Text>().text = "Пользователь: " + GameControllerScr.UserName;
        complexObj.name = "ComplexityPref";
    }

    public void ComplexitySelect()
    {
        GameControllerScr.Complexity = Object.GetComponent<Dropdown>().value + 1;
        SceneManager.LoadScene("game");
    }

    public void OpenNameSelected()
    {
        GameObject nameObj = Instantiate(Object2);
        nameObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        nameObj.transform.position = new Vector3(0, 0);
        nameObj.transform.Find("NameInput").GetComponent<InputField>().onValueChanged.AddListener(CharChanged);
        nameObj.GetComponent<MenuScr>().Object2 = gameObject;
        gameObject.SetActive(false);
    }

    public void NameSelect()
    {
        string name = Object.GetComponent<InputField>().text.Trim();
        if (name != "")
        {
            GameControllerScr.UserName = name;
        }
        Object2.SetActive(true);
        GameObject.Find("NameText").GetComponent<Text>().text = "Пользователь: " + GameControllerScr.UserName;
        Destroy(gameObject);
    }

    public void CharChanged(string newText)
    {
        GameObject.Find("NameInput").GetComponent<InputField>().onValueChanged.RemoveListener(CharChanged);
        GameObject.Find("NameInput").GetComponent<InputField>().text = newText.Replace('"', '\0');
        GameObject.Find("NameInput").GetComponent<InputField>().onValueChanged.AddListener(CharChanged);
    }

    public void SettingBtn()
    {
        SceneManager.LoadScene("setting");
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void MainMenuBtn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("menu");
    }

    public void RatingBtn()
    {
        GameObject.Find("PlayBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("RatingBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("SettingBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("QuitBtn").GetComponent<Button>().enabled = false;
        GameObject.Find("HelpBtn").GetComponent<Button>().enabled = false;
        GameObject ratingObj = Instantiate(Object);
        ratingObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        ratingObj.transform.position = new Vector3(0, 0);
        ratingObj.GetComponent<RatingScr>().UserNew = null;
        ratingObj.GetComponent<RatingScr>().ButtonText = "Закрыть";
    }

    public void CloseBtn()
    {
        FindObjectOfType<GameManagerScr>().CloseMenu();
    }

    public void SettingMiniBtn()
    {
        GameObject settingObj = Instantiate(Object);
        settingObj.transform.SetParent(GameObject.Find("MenuLayer").transform, false);
        settingObj.transform.position = new Vector3(0, 0);
        Destroy(gameObject);
    }

    public void HelpBtn()
    {
        GameObject.Find("QuitBtn").GetComponent<Button>().enabled = false;
        GameObject helpObj = Instantiate(HelpPref);
        helpObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        helpObj.transform.position = new Vector3(0, 0);
    }

    public void HelpClose()
    {
        GameObject.Find("QuitBtn").GetComponent<Button>().enabled = true;
        Destroy(gameObject);
    }
}
