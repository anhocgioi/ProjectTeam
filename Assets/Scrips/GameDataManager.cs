using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    [Header("Selected Characters")]
    public CharacterData selectedP1;
    public CharacterData selectedP2;

    [Header("Game Mode")]
    public bool isSinglePlayer = false;

    void Awake()
{
    if (instance != null && instance != this)
    {
        Destroy(gameObject);
        return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
}

    // ================= DEBUG =================
    public void DebugData()
    {
        Debug.Log("=== GAME DATA ===");
        Debug.Log("P1: " + (selectedP1 != null ? selectedP1.name : "NULL"));
        Debug.Log("P2: " + (selectedP2 != null ? selectedP2.name : "NULL"));
        Debug.Log("Mode: " + (isSinglePlayer ? "Single" : "Multi"));
    }

    // ================= RESET DATA =================
    public void ResetData()
    {
        selectedP1 = null;
        selectedP2 = null;
        isSinglePlayer = false;
    }
}