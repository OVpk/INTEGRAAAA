using UnityEngine;

public class XboxControllerManager : MonoBehaviour
{
    // Instance globale accessible depuis n'importe quel script
    public static XboxControllerManager Instance { get; private set; }

    public enum PlayerID 
    { 
        Player1 = 1, 
        Player2 = 2 
    }

    // Mapping standard des boutons Xbox sur Windows
    public enum XboxButton
    {
        A = 0, 
        B = 1, 
        X = 2, 
        Y = 3,
        LB = 4, 
        RB = 5,
        Back = 6, 
        Start = 7,
        L3 = 8, // Clic joystick gauche
        R3 = 9  // Clic joystick droit
    }

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

    /// 
    /// Convertit le joueur et le bouton en KeyCode Unity
    /// 
    private KeyCode GetKeyCode(PlayerID player, XboxButton button)
    {
        int joystickNum = (int)player;
        int buttonNum = (int)button;

        // Génère le nom du KeyCode (ex: "Joystick1Button0" pour le bouton A du Joueur 1)
        string keyName = $"Joystick{joystickNum}Button{buttonNum}";
        
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName);
    }

    /// 
    /// Retourne true à la frame exacte où le bouton est pressé
    /// 
    public bool GetButtonDown(PlayerID player, XboxButton button)
    {
        return Input.GetKeyDown(GetKeyCode(player, button));
    }

    /// 
    /// Retourne true à la frame exacte où le bouton est relâché
    /// 
    public bool GetButtonUp(PlayerID player, XboxButton button)
    {
        return Input.GetKeyUp(GetKeyCode(player, button));
    }

    /// 
    /// Retourne true tant que le bouton est maintenu enfoncé
    /// 
    public bool GetButton(PlayerID player, XboxButton button)
    {
        return Input.GetKey(GetKeyCode(player, button));
    }

    /// 
    /// Retourne la liste des manettes actuellement connectées
    /// 
    public string[] GetConnectedControllers()
    {
        return Input.GetJoystickNames();
    }
    
    /// 
    /// Vérifie si les deux manettes sont branchées
    /// 
    public bool AreTwoControllersConnected()
    {
        string[] controllers = Input.GetJoystickNames();
        int count = 0;
        foreach (string ctrl in controllers)
        {
            if (!string.IsNullOrEmpty(ctrl)) count++;
        }
        return count >= 2;
    }
}