using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Vị trí xuất hiện")]
    public Transform spawnPointP1; // Kéo Object Pos_P1 vào đây
    public Transform spawnPointP2; // Kéo Object Pos_P2 vào đây

    void Start()
    {
        // Luôn kiểm tra GameDataManager để tránh lỗi Null
        if (GameDataManager.instance != null)
        {
            SpawnCharacters();
        }
        else
        {
            Debug.LogError("LỖI: Không tìm thấy GameDataManager! Bạn phải Play từ màn hình chọn nhân vật.");
        }
    }

    void SpawnCharacters()
    {
        // 1. Lấy dữ liệu từ "xe chở dữ liệu"
        CharacterData dataP1 = GameDataManager.instance.selectedP1;
        CharacterData dataP2 = GameDataManager.instance.selectedP2;

        // 2. Tạo và cấu hình Player 1
        if (dataP1 != null && dataP1.characterPrefab != null)
        {
            GameObject p1 = Instantiate(dataP1.characterPrefab, spawnPointP1.position, Quaternion.identity);
            p1.name = "P1_" + dataP1.charName;

            // TỰ ĐỘNG GÁN ID = 1 CHO P1
            PlayerMove moveScriptP1 = p1.GetComponent<PlayerMove>();
            if (moveScriptP1 != null)
            {
                moveScriptP1.playerID = 1;
            }
        }

        // 3. Tạo và cấu hình Player 2
        if (dataP2 != null && dataP2.characterPrefab != null)
        {
            // Quaternion.Euler(0, 180, 0) giúp P2 quay mặt sang trái nhìn P1
            GameObject p2 = Instantiate(dataP2.characterPrefab, spawnPointP2.position, Quaternion.Euler(0, 180, 0));
            p2.name = "P2_" + dataP2.charName;

            // TỰ ĐỘNG GÁN ID = 2 CHO P2
            PlayerMove moveScriptP2 = p2.GetComponent<PlayerMove>();
            if (moveScriptP2 != null)
            {
                moveScriptP2.playerID = 2; // Dòng này giúp P2 dùng phím mũi tên
            }
        }
    }
}