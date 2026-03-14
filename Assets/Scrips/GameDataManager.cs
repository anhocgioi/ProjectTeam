using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public CharacterData selectedP1;
    public CharacterData selectedP2;

    void Awake()
    {
        // Giữ cho Object này không bị xóa khi đổi Scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}