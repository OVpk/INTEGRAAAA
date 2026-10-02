using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct OneTurn
{
    [SerializeField] public Sprite visual;
    [SerializeField] public XboxControllerManager.XboxButton goodAnswer;
    [SerializeField] public MiniGame miniGame;
}

public class Kahoot : MonoBehaviour
{
    private bool isLock = true;

    private XboxControllerManager.XboxButton P1Input;
    private XboxControllerManager.XboxButton P2Input;

    [SerializeField] private OneTurn[] allTurns;

    private int currentTurnIndex = 0;

    private float timer;

    [SerializeField]
    private float timePerQuestion;

    [SerializeField] private float endTimerBeforeNext;

    [SerializeField] private Image horloge;
    
    [SerializeField] private Image background;



    public void Init()
    {
        this.gameObject.SetActive(true);
        
        P1Input = XboxControllerManager.XboxButton.RB;
        P2Input = XboxControllerManager.XboxButton.RB;
        
        P1PosLogo.gameObject.SetActive(false);
        P2PosLogo.gameObject.SetActive(false);
        GoodAnswerLogo.gameObject.SetActive(false);

        background.sprite = allTurns[currentTurnIndex].visual;

        timer = timePerQuestion;
        isLock = false;
        
    }

    private IEnumerator EndTurn()
    {
        GoodAnswerLogo.gameObject.SetActive(true);

        if (P1Input == allTurns[currentTurnIndex].goodAnswer)
        {
            GlobalScoreManager.Instance.scoreP1++;
        }
        if (P2Input == allTurns[currentTurnIndex].goodAnswer)
        {
            GlobalScoreManager.Instance.scoreP2++;
        }
        
        yield return new WaitForSeconds(endTimerBeforeNext);

        if (allTurns[currentTurnIndex].miniGame != null)
        {
            allTurns[currentTurnIndex].miniGame.Init();
            yield return new WaitUntil( ()=> allTurns[currentTurnIndex].miniGame.isFinished == true);
        }
        
        if (currentTurnIndex+1 == allTurns.Length)
        {
            //stop game
        }
        else
        {
            currentTurnIndex++;
            Init();
        }
    }
    
    private void Update()
    {
        if (isLock) return;

        if (timer <= 0)
        {
            isLock = true;
            StartCoroutine(EndTurn());
        }
        else
        {
            timer -= Time.deltaTime;
        }

        horloge.fillAmount = timer / timePerQuestion;
        
        //P1 inputs
        if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player1,
                XboxControllerManager.XboxButton.A))
        {
            P1Input = XboxControllerManager.XboxButton.A;
            P1PosLogo.gameObject.SetActive(true);
        }
        else if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player1,
                     XboxControllerManager.XboxButton.B))
        {
            P1Input = XboxControllerManager.XboxButton.B;
            P1PosLogo.gameObject.SetActive(true);

        }
        else if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player1,
                     XboxControllerManager.XboxButton.X))
        {
            P1Input = XboxControllerManager.XboxButton.X;
            P1PosLogo.gameObject.SetActive(true);
        }
        else if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player1,
                     XboxControllerManager.XboxButton.Y))
        {
            P1Input = XboxControllerManager.XboxButton.Y;
            P1PosLogo.gameObject.SetActive(true);

        }
        //P2 inputs
        if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player2,
                XboxControllerManager.XboxButton.A))
        {
            P2Input = XboxControllerManager.XboxButton.A;
            P2PosLogo.gameObject.SetActive(true);

        }
        else if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player2,
                     XboxControllerManager.XboxButton.B))
        {
            P2Input = XboxControllerManager.XboxButton.B;
            P2PosLogo.gameObject.SetActive(true);

        }
        else if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player2,
                     XboxControllerManager.XboxButton.X))
        {
            P2Input = XboxControllerManager.XboxButton.X;
            P2PosLogo.gameObject.SetActive(true);

        }
        else if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player2,
                     XboxControllerManager.XboxButton.Y))
        {
            P2Input = XboxControllerManager.XboxButton.Y;
            P2PosLogo.gameObject.SetActive(true);

        }
        
        //DRAW
        PlaceToGoodPos();
    }

    [SerializeField] private Transform Aposition;
    [SerializeField] private Transform Bposition;
    [SerializeField] private Transform Xposition;
    [SerializeField] private Transform Yposition;
    
    [SerializeField] private Transform P1PosLogo;
    [SerializeField] private Transform P2PosLogo;
    
    [SerializeField] private Transform GoodAnswerLogo;


    private void PlaceToGoodPos()
    {
        P1PosLogo.position = P1Input switch
        {
            XboxControllerManager.XboxButton.A => Aposition.position,
            XboxControllerManager.XboxButton.B => Bposition.position,
            XboxControllerManager.XboxButton.X => Xposition.position,
            XboxControllerManager.XboxButton.Y => Yposition.position,
            _ => P1PosLogo.position
        };
        
        P2PosLogo.position = P2Input switch
        {
            XboxControllerManager.XboxButton.A => Aposition.position,
            XboxControllerManager.XboxButton.B => Bposition.position,
            XboxControllerManager.XboxButton.X => Xposition.position,
            XboxControllerManager.XboxButton.Y => Yposition.position,
            _ => P2PosLogo.position
        };
        
        GoodAnswerLogo.position = allTurns[currentTurnIndex].goodAnswer switch
        {
            XboxControllerManager.XboxButton.A => Aposition.position,
            XboxControllerManager.XboxButton.B => Bposition.position,
            XboxControllerManager.XboxButton.X => Xposition.position,
            XboxControllerManager.XboxButton.Y => Yposition.position,
            _ => GoodAnswerLogo.position
        };
    }


}
