using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI Reference")]
    [SerializeField] private Slider healthSlider;

    // Property ตามสัญญาของ IDamageable
    public float CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // ฟังก์ชันรับดาเมจ (เรียกใช้จากกระสุนของ Em หรือเหตุการณ์อื่นๆ)
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died!");
        // โค้ดจัดการเมื่อ Player ตาย เช่น แสดงหน้าจอ Game Over
    }

    // ฟังก์ชันสำหรับเพิ่มเลือด (เผื่อเก็บไอเทมฟื้นเลือด)
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}