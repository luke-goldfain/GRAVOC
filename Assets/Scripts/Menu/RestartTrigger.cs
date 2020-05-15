using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartTrigger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("P1Start") || Input.GetButtonDown("P2Start"))
        {
            ReturnToMenu();
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
