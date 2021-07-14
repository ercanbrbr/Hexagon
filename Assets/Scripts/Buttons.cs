using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void back()
    {
        if (FindObjectOfType<MainSoundManager>() != null)
            FindObjectOfType<MainSoundManager>().Play("buttonClick");
        SceneManager.LoadScene("MainMenu");
    }
    public void startGame()
    {
        if (FindObjectOfType<MainSoundManager>() != null)
            FindObjectOfType<MainSoundManager>().Play("buttonClick");
        SceneManager.LoadScene("Game");
    }
    public void quit()
    {
        if (FindObjectOfType<MainSoundManager>() != null)
            FindObjectOfType<MainSoundManager>().Play("buttonClick");
        Application.Quit();
    }
}
