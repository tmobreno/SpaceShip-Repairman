using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioManager manager;
    public GameObject mainMenuUI;
    public GameObject worldSelectUI;

    void Start()
    {
        manager.Play("Theme");
        worldSelectUI.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void WorldSelect()
    {
        worldSelectUI.SetActive(true);
        mainMenuUI.SetActive(false);
        
    }

    public void WorldSelect1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void WorldSelect2()
    {
        SceneManager.LoadScene("Level 11");
    }

    public void WorldSelect3()
    {
        SceneManager.LoadScene("Level 21");
    }

    public void backSelect()
    {
        worldSelectUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
