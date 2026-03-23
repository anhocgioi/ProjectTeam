using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Vị trí xuất hiện")]
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    [Header("Thanh máu UI")]
    public HealthBar healthBarUI_P1;
    public HealthBar healthBarUI_P2;

    void Start()
    {
        // Kiểm tra GameDataManager để tránh lỗi Null ngay từ đầu
        if (GameDataManager.instance == null)
        {
            Debug.LogError("⚠️ GameDataManager null! Bạn phải chạy game từ Scene Menu.");
            return;
        }

        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        var data = GameDataManager.instance;

        // ===================== KHỞI TẠO P1 =====================
        if (data.selectedP1 != null && data.selectedP1.characterPrefab != null)
        {
            // Kiểm tra SpawnPoint trước khi Instantiate để tránh lỗi NullReference
            if (spawnPointP1 == null) { Debug.LogError("Chưa kéo SpawnPointP1 vào LevelManager!"); return; }

            GameObject p1 = Instantiate(data.selectedP1.characterPrefab, spawnPointP1.position, Quaternion.identity);
            p1.name = "Player_1";
            p1.layer = LayerMask.NameToLayer("P1"); // Đảm bảo gán đúng Layer để có thể bị đánh trúng

            // Gán thông số di chuyển
            PlayerMove moveP1 = p1.GetComponent<PlayerMove>();
            if (moveP1 != null)
            {
                moveP1.playerID = 1;
                moveP1.enabled = true;
            }

            // Kết nối CharacterStats với thanh máu UI
            CharacterStats statsP1 = p1.GetComponent<CharacterStats>();
            if (statsP1 != null)
            {
                statsP1.healthBar = healthBarUI_P1; // healthBarUI_P1 phải được kéo vào Inspector
                statsP1.playerIndex = 1;
            }
            else { Debug.LogWarning("P1 thiếu script CharacterStats!"); }
        }

        // ===================== KHỞI TẠO P2 =====================
        if (data.selectedP2 != null && data.selectedP2.characterPrefab != null)
        {
            if (spawnPointP2 == null) { Debug.LogError("Chưa kéo SpawnPointP2 vào LevelManager!"); return; }

            Quaternion p2Rotation = Quaternion.Euler(0, 180, 0);
            GameObject p2 = Instantiate(data.selectedP2.characterPrefab, spawnPointP2.position, p2Rotation);
            p2.name = "Player_2";
            p2.layer = LayerMask.NameToLayer("P2"); // Đảm bảo gán đúng Layer P2

            PlayerMove moveP2 = p2.GetComponent<PlayerMove>();
            if (moveP2 != null)
            {
                moveP2.playerID = 2;
                if (data.isSinglePlayer)
                {
                    moveP2.enabled = false;
                    if (!p2.TryGetComponent<BotAI>(out _)) p2.AddComponent<BotAI>();
                }
                else { moveP2.enabled = true; }
            }

            CharacterStats statsP2 = p2.GetComponent<CharacterStats>();
            if (statsP2 != null)
            {
                statsP2.healthBar = healthBarUI_P2;
                statsP2.playerIndex = 2;
            }
            else { Debug.LogWarning("P2 thiếu script CharacterStats!"); }
        }
    }
}