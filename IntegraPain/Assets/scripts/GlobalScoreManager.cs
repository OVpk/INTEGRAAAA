using UnityEngine;

public class GlobalScoreManager : MonoBehaviour
{
    public static GlobalScoreManager Instance { get; private set; }
    
    private void Awake()
    {
        // Initialisation du Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Persiste entre les scènes
        }
    }

    public int scoreP1 = 0;
    public int scoreP2 = 0;

    
}
