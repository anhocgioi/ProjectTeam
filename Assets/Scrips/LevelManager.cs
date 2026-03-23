using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Vị trí xuất hiện")]
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    void Start()
    {
        // Nếu chạy thẳng từ Scene mà không qua Menu, báo lỗi để dễ sửa
        if (GameDataManager.instance == null)
        {
            Debug.LogError("⚠️ GameDataManager null! Hãy chạy game từ màn hình Menu.");
            return;
        }

        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        var data = GameDataManager.instance;

        // ===================== P1 =====================
        if (data.selectedP1 != null && data.selectedP1.characterPrefab != null)
        {
            GameObject p1 = Instantiate(
                data.selectedP1.characterPrefab,
                spawnPointP1.position,
                Quaternion.identity
            );

            p1.name = "Player_1";
            p1.tag = "Player"; // Đảm bảo gắn tag để AI tìm thấy

            // 🔥 QUAN TRỌNG: Ép ID là 1 cho P1
            PlayerMove moveP1 = p1.GetComponent<PlayerMove>();
            if (moveP1 != null)
            {
                moveP1.playerID = 1;
                moveP1.enabled = true; // Luôn bật cho P1
            }
        }

        // ===================== P2 =====================
        if (data.selectedP2 != null && data.selectedP2.characterPrefab != null)
        {
            // Xoay mặt P2 sang trái khi xuất hiện (thường P2 đứng bên phải)
            Quaternion p2Rotation = Quaternion.Euler(0, 180, 0);

            GameObject p2 = Instantiate(
                data.selectedP2.characterPrefab,
                spawnPointP2.position,
                p2Rotation
            );

            p2.name = "Player_2";
            PlayerMove moveP2 = p2.GetComponent<PlayerMove>();

            if (moveP2 != null)
            {
                // 🔥 QUAN TRỌNG: Ép ID là 2 cho P2
                moveP2.playerID = 2;

                // ===================== 🔥 SINGLE PLAYER → BOT =====================
                if (data.isSinglePlayer)
                {
                    // ❌ Tắt script PlayerMove của P2 để tránh nhận phím bấm
                    moveP2.enabled = false;

                    // ❌ Reset vật lý
                    Rigidbody2D rb = p2.GetComponent<Rigidbody2D>();
                    if (rb != null) rb.velocity = Vector2.zero;

                    // ✅ Thêm Bot AI
                    if (!p2.TryGetComponent<BotAI>(out _))
                    {
                        p2.AddComponent<BotAI>();
                    }
                    Debug.Log("P2 Spawned as BOT AI");
                }
                else
                {
                    // ✅ Nếu là MultiPlayer, đảm bảo PlayerMove của P2 được bật
                    moveP2.enabled = true;
                    Debug.Log("P2 Spawned as PLAYER 2 (Arrows + Num1)");
                }
            }
        }
    }
}