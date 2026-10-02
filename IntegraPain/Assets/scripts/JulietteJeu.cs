using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JulietteJeu : MiniGame
{
    [Header("Durée du jeu")]
    [Tooltip("Temps en secondes avant que le mini-jeu ne se termine")]
    [SerializeField] private float EndTimeGlobal = 30f; // <-- NOUVELLE VARIABLE

    [Header("Objet UI à déplacer")]
    [SerializeField] private RectTransform objectToMove;

    [Header("Positions d'ancrage (X, Y)")]
    [SerializeField] private Vector2 positionA;
    [SerializeField] private Vector2 positionB;

    [Header("Réglages")]
    [SerializeField] private float speed = 500f;     
    
    [Tooltip("Temps de pause une fois arrivé au point B")]
    [SerializeField] private float waitTimeAtPointB = 1f; 
    
    [Tooltip("Temps d'attente MINIMUM avant un nouvel aller-retour")]
    [SerializeField] private float minWaitTime = 1f;
    
    [Tooltip("Temps d'attente MAXIMUM avant un nouvel aller-retour")]
    [SerializeField] private float maxWaitTime = 5f;

    [SerializeField] private TMP_Text textee;

    [SerializeField] private string[] lesmots;
    
    
    private bool canMove;
    private bool isLock;
    
    private float timerP1;
    private float timerP2;

    [SerializeField] private Image pc1;
    [SerializeField] private Image pc2;
    
    [SerializeField] private Sprite pc1narmal;
    [SerializeField] private Sprite pc2narmal;
    
    [SerializeField] private Sprite pc1fortnite;
    [SerializeField] private Sprite pc2fortnite;


    public override void Init()
    {
        this.gameObject.SetActive(true);
        
        // CORRECTION : Retire le "bool" sinon cela crée une variable locale
        // au lieu d'utiliser la variable globale de la classe
        canMove = true; 
        isLock = false;
        
        isFinished = false;

        objectToMove.anchoredPosition = positionA;
        
        // On lance le mouvement
        StartCoroutine(MoveRoutine());
        
        // <-- NOUVEAU : On lance le compte à rebours de fin de jeu
        StartCoroutine(GameTimerRoutine());
    }
    
    // <-- NOUVELLE COROUTINE
    private IEnumerator GameTimerRoutine()
    {
        // On attend la durée définie par EndTimeGlobal
        yield return new WaitForSeconds(EndTimeGlobal);
        
        // Une fois le temps écoulé, on lance la fonction de fin
        FinishGame();
    }

    private void FinishGame()
    {
        isLock = true;
        isFinished = true;
        
        // <-- NOUVEAU : On coupe toutes les coroutines en cours (mouvement, etc.)
        StopAllCoroutines();
        
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isLock) return;
    
        // Joueur 1
        if (XboxControllerManager.Instance.GetButton(XboxControllerManager.PlayerID.Player1, XboxControllerManager.XboxButton.A))
        {
            timerP1 += Time.deltaTime;
            if (timerP1 >= 1f)
            {
                // Ajoute 1 si canMove est true, sinon retire 10
                GlobalScoreManager.Instance.scoreP1 += canMove ? 1 : -10;
                timerP1 -= 1f;
            }
            pc1.sprite = pc1fortnite;
        }
        else
        {
            timerP1 = 0f; // Remet le compteur à zéro si le bouton est relâché
            pc1.sprite = pc1narmal;
        }

        // Joueur 2
        if (XboxControllerManager.Instance.GetButton(XboxControllerManager.PlayerID.Player2, XboxControllerManager.XboxButton.A))
        {
            timerP2 += Time.deltaTime;
            if (timerP2 >= 1f)
            {
                // Ajoute 1 si canMove est true, sinon retire 10
                GlobalScoreManager.Instance.scoreP2 += canMove ? 1 : -10;
                timerP2 -= 1f;
            }
            pc2.sprite = pc2fortnite;
        }
        else
        {
            timerP2 = 0f; // Remet le compteur à zéro si le bouton est relâché
            pc2.sprite = pc2narmal;
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Déplacement de A vers B
            textee.text = lesmots[Random.Range(0, lesmots.Length)];
            yield return MoveToTarget(positionB);

            // Pause au point B avec le temps défini dans l'inspecteur
            canMove = false;
            yield return new WaitForSeconds(waitTimeAtPointB);
            canMove = true;
            
            // Déplacement de B vers A (retour complet au point de départ)
            yield return MoveToTarget(positionA);
            
            // Pause aléatoire entre deux allers-retours avant de recommencer
            float randomWaitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    private IEnumerator MoveToTarget(Vector2 target)
    {
        while (Vector2.Distance(objectToMove.anchoredPosition, target) > 0.1f)
        {
            objectToMove.anchoredPosition = Vector2.MoveTowards(
                objectToMove.anchoredPosition,
                target,
                speed * Time.deltaTime
            );
            yield return null;
        }

        // Calage exact sur la position cible à la fin
        objectToMove.anchoredPosition = target;
    }
}