using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private float targetHealth;
    public void SetMaxHealth(float health)
    {
        if (slider == null) slider = GetComponent<Slider>();
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        if (slider != null)
        {
            slider.value = health; // Gán thẳng giá trị, chắc chắn sẽ tụt nếu gán đúng
        }
    }
}