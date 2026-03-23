using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Vị trí xuất hiện")]
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    void Start()
    {
        if (GameDataManager.instance == null) return;

        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        var data = GameDataManager.instance;

        // ===================== P1 =====================
        if (data.selectedP1 != null)
        {
            GameObject p1 = Instantiate(
                data.selectedP1.characterPrefab,
                spawnPointP1.position,
                Quaternion.identity
            );

            p1.name = "Player_1";
        }

        // ===================== P2 =====================
        if (data.selectedP2 != null)
        {
            GameObject p2 = Instantiate(
                data.selectedP2.characterPrefab,
                spawnPointP2.position,
                Quaternion.identity
            );

            p2.name = "Player_2";

            // ===================== 🔥 SINGLE PLAYER → BOT =====================
            if (data.isSinglePlayer)
            {
                // ❌ TẮT PLAYER CONTROL
                PlayerMove move = p2.GetComponent<PlayerMove>();
                if (move != null)
                {
                    move.enabled = false;
                }

                // ❌ TẮT PHYSICS CHUYỂN ĐỘNG NGAY KHI SPAWN
                Rigidbody2D rb = p2.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }

                // ✅ THÊM BOT AI (KHÔNG ADD TRÙNG)
                if (!p2.TryGetComponent<BotAI>(out _))
                {
                    p2.AddComponent<BotAI>();
                }
            }
        }
    }
}