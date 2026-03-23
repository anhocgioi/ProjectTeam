using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar; // Kéo thanh máu tương ứng vào đây trong Inspector

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }
    void InitHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }
    // Hàm gọi khi bị tấn công
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Ép máu không âm

        if (healthBar != null) healthBar.SetHealth(currentHealth);

        // 🔥 QUAN TRỌNG: Kiểm tra máu ngay khi vừa bị trừ
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public int playerIndex; // 1 cho P1, 2 cho P2

    public void Die()
    {
        // Tìm BattleManager trong Scene
        BattleManager bm = FindObjectOfType<BattleManager>();
        if (bm != null)
        {
            bm.FinishGame(playerIndex); // playerIndex của Player 2 là 2
        }

        // Tắt nhân vật để không đánh tiếp được nữa
        gameObject.SetActive(false);
    }
}