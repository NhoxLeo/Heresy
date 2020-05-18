using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menus : MonoBehaviour
{
    public GameObject howToPlayMenu;
    public GameObject mainMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlay()
    {
        howToPlayMenu.SetActive(true);
        mainMenu.SetActive(false);
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
        mainMenu.SetActive(true);
    }
}
