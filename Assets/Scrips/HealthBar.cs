using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float speed = 5f; // Tốc độ giảm của thanh máu (càng cao càng nhanh)

    private float targetHealth;

    // Thiết lập máu tối đa khi bắt đầu trận
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        targetHealth = health;
    }

    // Hàm này chỉ cập nhật "mục tiêu" máu mới
    public void SetHealth(float health)
    {
        targetHealth = health;
    }

    void Update()
    {
        // Kiểm tra nếu giá trị hiện tại của Slider khác với mục tiêu
        if (Mathf.Abs(slider.value - targetHealth) > 0.01f)
        {
            // Sử dụng Lerp để trượt giá trị slider.value tới targetHealth
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * speed);
        }
        else
        {
            slider.value = targetHealth;
        }
    }
}