using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LeGrosBeck : MiniGame
{
    private float jaugeBeck;

    [Header("Game Settings")]
    [SerializeField] private float maxJauge = 100f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float TargetA;
    [SerializeField] private float TargetB;
    [SerializeField] private int nbOfPoints = 1;

    private float p1;
    private float p2;

    private bool p1Played;
    private bool p2Played;

    [Header("UI Canvas - Références")]
    [Tooltip("Le conteneur principal de la jauge (détermine la hauteur totale).")]
    [SerializeField] private RectTransform gaugeContainer;

    [Tooltip("Image optionnelle de la jauge qui se remplit (type Filled).")]
    [SerializeField] private Image jaugeBeckFillImage;

    [Header("UI Canvas - Marqueurs Fixes")]
    [Tooltip("Marqueur représentant le 0 (tout en bas).")]
    [SerializeField] private RectTransform startMarker;
    [Tooltip("Marqueur représentant la maxJauge (tout en haut).")]
    [SerializeField] private RectTransform endMarker;
    [Tooltip("Marqueur visuel pour la borne A.")]
    [SerializeField] private RectTransform targetAMarker;
    [Tooltip("Marqueur visuel pour la borne B.")]
    [SerializeField] private RectTransform targetBMarker;

    [Header("UI Canvas - Marqueurs Mobiles")]
    [Tooltip("Le curseur qui monte en temps réel avec la jauge.")]
    [SerializeField] private RectTransform currentPositionMarker;
    [Tooltip("Logo/Icone du joueur 1, s'affiche quand il valide.")]
    [SerializeField] private RectTransform player1Marker;
    [Tooltip("Logo/Icone du joueur 2, s'affiche quand il valide.")]
    [SerializeField] private RectTransform player2Marker;


    public override void Init()
    {
        this.gameObject.SetActive(true);
        jaugeBeck = 0f;
        p1 = 0f;
        p2 = 0f;
        p1Played = false;
        p2Played = false;
        isFinished = false;

        // Reset du remplissage (si utilisé)
        if (jaugeBeckFillImage != null)
        {
            jaugeBeckFillImage.fillAmount = 0f;
        }

        // Masquer les logos des joueurs au départ
        if (player1Marker != null) player1Marker.gameObject.SetActive(false);
        if (player2Marker != null) player2Marker.gameObject.SetActive(false);

        // Positionnement initial de tous les marqueurs fixes
        UpdateMarkerPosition(startMarker, 0f);
        UpdateMarkerPosition(endMarker, maxJauge);
        UpdateMarkerPosition(targetAMarker, TargetA);
        UpdateMarkerPosition(targetBMarker, TargetB);
        
        // Positionnement du curseur actuel au point 0
        UpdateMarkerPosition(currentPositionMarker, 0f);
    }

    private void Update()
    {
        if (isFinished) return;

        // Progression de la jauge
        jaugeBeck = math.lerp(jaugeBeck, maxJauge, speed * Time.deltaTime);

        // Mise à jour visuelle du curseur mobile
        UpdateMarkerPosition(currentPositionMarker, jaugeBeck);

        // Mise à jour visuelle de la barre de remplissage (optionnel)
        if (jaugeBeckFillImage != null && maxJauge > 0)
        {
            jaugeBeckFillImage.fillAmount = math.clamp(jaugeBeck / maxJauge, 0f, 1f);
        }

        // Fin de jeu automatique si la jauge atteint le max
        if (maxJauge - jaugeBeck < 0.1f)
        {
            FinishGame();
        }

        // Action Joueur 1
        if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player1, XboxControllerManager.XboxButton.A))
        {
            if (!p1Played)
            {
                p1 = jaugeBeck;

                if (p1 > TargetA && p1 < TargetB)
                {
                    GlobalScoreManager.Instance.scoreP1 += nbOfPoints;
                }

                p1Played = true;
                ShowPlayerMarker(player1Marker, p1);
            }
        }

        // Action Joueur 2
        if (XboxControllerManager.Instance.GetButtonDown(XboxControllerManager.PlayerID.Player2, XboxControllerManager.XboxButton.A))
        {
            if (!p2Played)
            {
                p2 = jaugeBeck;

                if (p2 > TargetA && p2 < TargetB)
                {
                    GlobalScoreManager.Instance.scoreP2 += nbOfPoints;
                }

                p2Played = true;
                ShowPlayerMarker(player2Marker, p2);
            }
        }
    }

    /// 
    /// Calcule le ratio (0 à 1) et place un RectTransform de bas en haut.
    /// 
    private void UpdateMarkerPosition(RectTransform marker, float value)
    {
        if (marker == null || gaugeContainer == null || maxJauge <= 0) return;

        // Ratio normalisé entre 0 et 1
        float ratio = math.clamp(value / maxJauge, 0f, 1f);

        // Hauteur totale du conteneur en pixels
        float totalHeight = gaugeContainer.rect.height;

        // Application de la position sur l'axe Y
        Vector2 anchoredPos = marker.anchoredPosition;
        anchoredPos.y = ratio * totalHeight;
        marker.anchoredPosition = anchoredPos;
    }

    /// 
    /// Active le logo du joueur et le positionne selon sa valeur enregistrée.
    /// 
    private void ShowPlayerMarker(RectTransform playerMarker, float value)
    {
        if (playerMarker == null) return;

        UpdateMarkerPosition(playerMarker, value);
        playerMarker.gameObject.SetActive(true);
    }

    private void FinishGame()
    {
        isFinished = true;
        
        this.gameObject.SetActive(false);
    }
}