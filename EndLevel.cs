using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{

    public bool CountdownActive = true;
    public GameObject countdown;
    // Start is called before the first frame update
    void Start()
    {
        if (CountdownActive == false)
        {
            countdown.SetActive(false);
        }
        else
        {
            countdown.SetActive(true);
        }
    }

    // Update is called once per frame
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
