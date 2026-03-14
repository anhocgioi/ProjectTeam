using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Vị trí xuất hiện")]
    public Transform spawnPointP1; // Kéo 1 Object bên trái vào đây
    public Transform spawnPointP2; // Kéo 1 Object bên phải vào đây

    void Start()
    {
        // Kiểm tra xem "xe chở dữ liệu" có tồn tại không
        if (GameDataManager.instance != null)
        {
            SpawnCharacters();
        }
        else
        {
            Debug.LogError("Không tìm thấy GameDataManager! Hãy chạy game từ màn hình chọn nhân vật.");
        }
    }

    void SpawnCharacters()
    {
        // 1. Lấy dữ liệu nhân vật đã chọn
        CharacterData dataP1 = GameDataManager.instance.selectedP1;
        CharacterData dataP2 = GameDataManager.instance.selectedP2;

        // 2. Tạo nhân vật P1 (Xoay mặt sang phải)
        if (dataP1 != null && dataP1.characterPrefab != null)
        {
            GameObject p1 = Instantiate(dataP1.characterPrefab, spawnPointP1.position, Quaternion.identity);
            p1.name = "Player 1_" + dataP1.charName;
            // Bạn có thể thêm code set PlayerIndex = 1 cho script điều khiển ở đây
        }

        // 3. Tạo nhân vật P2 (Xoay mặt sang trái)
        if (dataP2 != null && dataP2.characterPrefab != null)
        {
            GameObject p2 = Instantiate(dataP2.characterPrefab, spawnPointP2.position, Quaternion.Euler(0, 180, 0));
            p2.name = "Player 2_" + dataP2.charName;
            // Bạn có thể thêm code set PlayerIndex = 2 cho script điều khiển ở đây
        }
    }
}