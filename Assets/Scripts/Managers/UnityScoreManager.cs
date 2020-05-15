using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityScoreManager : MonoBehaviour
{
    private ScoreManager scoreMgr;

    [SerializeField, Tooltip("The score a player must reach to win the game.")]
    private int scoreToWin;

    [SerializeField]
    private GameObject restartGO;

    [SerializeField]
    private Text restartText, p1scoreText, p2scoreText, winText;
    
    private Color p1color, p2color;

    private bool scoreReached;

    // Start is called before the first frame update
    void Start()
    {
        scoreMgr = ScoreManager.Instance;

        scoreMgr.InitScores(2);

        restartText.text = "";

        winText.text = "";
        p1color = p1scoreText.color;
        p2color = p2scoreText.color;
        scoreReached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!scoreReached)
        {
            p1scoreText.text = scoreMgr.Scores[0].ToString();
            p2scoreText.text = scoreMgr.Scores[1].ToString();
        }

        UpdateCheckForWinner();
    }

    private void UpdateCheckForWinner()
    {
        if (!scoreReached)
        {
            foreach (int s in scoreMgr.Scores)
            {
                if (s >= scoreToWin)
                {
                    scoreReached = true;

                    p1scoreText.text = "";
                    p2scoreText.text = "";

                    switch (scoreMgr.Scores.IndexOf(s))
                    {
                        case 0:
                            winText.text = "Player 1 wins!";
                            winText.color = p1color;
                            restartText.color = p1color;
                            break;
                        case 1:
                            winText.text = "Player 2 wins!";
                            winText.color = p2color;
                            restartText.color = p2color;
                            break;
                    }

                    restartGO.SetActive(true);
                    restartText.text = "Hit Start to return to Menu";
                }
            }
        }
    }
}
