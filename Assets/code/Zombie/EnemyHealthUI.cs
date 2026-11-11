using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;

    private IDamageable damageableTarget;
    private Camera mainCamera;

    void Start()
    {
        // ดึงคอมโพเนนต์ IDamageable จากตัวแม่ (Em)
        damageableTarget = GetComponentInParent<IDamageable>();
        mainCamera = Camera.main;

        if (healthSlider != null && damageableTarget != null)
        {
            // ตั้งค่าค่าเริ่มต้นของ Slider ตาม MaxHealth (สมมติว่าเริ่มเต็มหลอด)
            healthSlider.maxValue = damageableTarget.CurrentHealth;
            healthSlider.value = damageableTarget.CurrentHealth;
        }
    }

    void Update()
    {
        // 1. อัปเดตค่าหลอดเลือดตามเลือดปัจจุบัน
        if (healthSlider != null && damageableTarget != null)
        {
            healthSlider.value = damageableTarget.CurrentHealth;
        }

        // 2. ให้ Canvas หันหน้าเข้าหาลานกล้องตลอดเวลา (Billboard Effect)
        if (mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}