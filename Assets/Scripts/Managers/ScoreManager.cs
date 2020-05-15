using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    private static ScoreManager scoreMgrInstance;

    private static readonly object getlock = new object();

    public static ScoreManager Instance
    {
        get
        {
            lock (getlock)
            {
                if (scoreMgrInstance == null)
                {
                    scoreMgrInstance = new ScoreManager();
                }



                return scoreMgrInstance;
            }
        }
    }

    public List<int> Scores { get; protected set; }

    public void InitScores(int numberOfPlayers)
    {
        // Lazy load
        if (Scores == null)
        {
            Scores = new List<int>();
        }

        // Lazy populate
        while (numberOfPlayers > Scores.Count)
        {
            Scores.Add(0);
        }
    }

    public void IncrementScore(int playerNumber)
    { 
        Scores[playerNumber - 1]++;
    }
}
