using System;
using TMPro;
using UnityEngine;

public class GlobalScoreManager : MonoBehaviour
{
    public static GlobalScoreManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int scoreP1 = 0;
    public int scoreP2 = 0;

    [SerializeField] private TMP_Text[] p1scoreText;
    [SerializeField] private TMP_Text[] p2scoreText;

    private void Update()
    {
        foreach (TMP_Text texte in p1scoreText)
            texte.text = scoreP1.ToString();
        foreach (TMP_Text texte in p2scoreText)
            texte.text = scoreP2.ToString();
    }
}
