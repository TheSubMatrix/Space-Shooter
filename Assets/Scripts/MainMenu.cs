using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Level One");
    }
}
