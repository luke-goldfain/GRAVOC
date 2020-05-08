using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityScoreManager : MonoBehaviour
{
    private ScoreManager scoreMgr;

    [SerializeField]
    private Text p1scoreText, p2scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreMgr = ScoreManager.Instance;

        scoreMgr.InitScores(2);
    }

    // Update is called once per frame
    void Update()
    {
        p1scoreText.text = scoreMgr.Scores[0].ToString();
        p2scoreText.text = scoreMgr.Scores[1].ToString();
    }
}
