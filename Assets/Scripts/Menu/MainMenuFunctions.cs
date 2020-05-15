using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    [SerializeField]
    private Button playBtn, backBtn;

    [SerializeField]
    private GameObject mainPanel, creditsPanel;

    [SerializeField]
    private string mainSceneName;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.Select();
    }

    public void DisplayCredits()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);

        backBtn.Select();
    }

    public void DisplayMainMenu()
    {

        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);

        playBtn.Select();
    }

    public void LoadMainGameScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
