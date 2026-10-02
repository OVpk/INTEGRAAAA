using System;
using Unity.Mathematics;
using UnityEngine;

public class LeGrosBeck : MiniGame
{
    private float jaugeBeck;

    [SerializeField] private float maxJauge;

    [SerializeField] private float speed;

    [SerializeField] private float Target;

    private float p1;
    private float p2;

    private bool p1Played;
    private bool p2Played;

    [SerializeField] private int nbOfPoints;

    
    public override void Init()
    {
        jaugeBeck = 0f;
        p1 = 0f;
        p2 = 0f;
        p1Played = false;
        p2Played = false;
        
        isFinished = false;

    }

    private void Update()
    {
        if (isFinished) return;

        jaugeBeck = math.lerp(jaugeBeck, maxJauge, speed * Time.deltaTime);
        
        if (maxJauge - jaugeBeck < 0.1f)
        {
            FinishGame();
        }
        
        if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player1,
                XboxControllerManager.XboxButton.A))
        {
            if (!p1Played)
            {
                p1 = jaugeBeck;
                p1Played = true;
            }
        }
        
        if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player2,
                XboxControllerManager.XboxButton.A))
        {
            if (!p2Played)
            {
                p2 = jaugeBeck;
                p2Played = true;
            }
        }
    }

    private void FinishGame()
    {
        float diffP1 = Mathf.Abs(Target - p1);
        float diffP2 = Mathf.Abs(Target - p2);

        if (diffP1 < diffP2)
        {
            GlobalScoreManager.Instance.scoreP1 += nbOfPoints;
        }
        else
        {
            GlobalScoreManager.Instance.scoreP2 += nbOfPoints;
        }

        isFinished = true;
    }
}
