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

    // Hàm gọi khi bị tấn công
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " đã gục ngã!");
        // Thêm hiệu ứng biến mất hoặc kết thúc trận đấu ở đây
    }
}