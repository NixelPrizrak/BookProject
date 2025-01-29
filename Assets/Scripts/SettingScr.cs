using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingScr : MonoBehaviour
{
    public Dropdown WindowModeDrop;
    public Dropdown ResolutionDrop;
    public Slider VolumeSlider;
    public Text VolumeText;
    public Toggle AudioCheck;
    public GameObject MenuPref;

    public void Start()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            WindowModeDrop.value = 0;
        }
        else
        {
            WindowModeDrop.value = 1;
        }

        if (Screen.width == 960)
        {
            ResolutionDrop.value = 0;
        }
        else if (Screen.width == 1440)
        {
            ResolutionDrop.value = 1;
        }
        else
        {
            ResolutionDrop.value = 2;
        }

        VolumeSlider.value = AudioListener.volume;
        AudioCheck.isOn = !AudioListener.pause;
    }

    public void WindowModeChange()
    {
        if (WindowModeDrop.value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    public void ResolutionChange()
    {
        if (ResolutionDrop.value == 0)
        {
            Screen.SetResolution(960, 540, WindowModeDrop.value == 1);
        }
        else if (ResolutionDrop.value == 1)
        {
            Screen.SetResolution(1440, 810, WindowModeDrop.value == 1);
        }
        else
        {
            Screen.SetResolution(1920, 1080, WindowModeDrop.value == 1);
        }
    }

    public void VolumeValueChange()
    {
        AudioListener.volume = VolumeSlider.value;
        VolumeText.text = Mathf.RoundToInt(VolumeSlider.value * 100).ToString();
    }

    public void AudioCheckChange()
    {
        VolumeSlider.interactable = AudioCheck.isOn;
        AudioListener.pause = !AudioCheck.isOn;
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("menu");
    }

    public void MenuMiniBtn()
    {
        GameObject menuObj = Instantiate(MenuPref);
        menuObj.transform.SetParent(GameObject.Find("MenuLayer").transform, false);
        menuObj.transform.position = new Vector3(0, 0);
        Destroy(gameObject);
    }
}
