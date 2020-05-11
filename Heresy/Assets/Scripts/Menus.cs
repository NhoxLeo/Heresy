using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menus : MonoBehaviour
{
    public GameObject howToPlayMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlay()
    {
        howToPlayMenu.SetActive(true);
    }  

    public void Quit()
    {
        Application.Quit();
    }
    public void MenuQuit()
    {
        SceneManager.LoadScene(0);
    }

    public void HowToPlayBack()
    {
        howToPlayMenu.SetActive(false);
    }
}
