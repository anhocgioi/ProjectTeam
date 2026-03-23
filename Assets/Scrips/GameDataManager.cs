using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    [Header("Selected Characters")]
    public CharacterData selectedP1;
    public CharacterData selectedP2;

    [Header("Game Mode")]
    public bool isSinglePlayer = false;

    [Header("Map Settings")]
    // Lưu tên Scene của Map đã chọn (Mặc định là Map1)
    public string selectedMapName = "Map1";

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
        Debug.Log("Selected Map: " + selectedMapName);
    }

    // ================= RESET DATA =================
    public void ResetData()
    {
        selectedP1 = null;
        selectedP2 = null;
        isSinglePlayer = false;
        selectedMapName = "Map1"; // Reset về map mặc định
    }
}