using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaDrainRate = 20f;   // อัตราการลดลงต่อวินาทีตอนวิ่ง
    [SerializeField] private float idleRegenRate = 2.0f;     // ยืนอยู่กับที่เพิ่ม 2 หน่วย/วินาที
    [SerializeField] private float walkRegenRate = 1.0f;     // เดินเพิ่ม 1 หน่วย/วินาที

    [Header("UI Reference")]
    [SerializeField] private Slider staminaSlider;           // (ถ้ามี UI หลอดสตามิน่า)

    private CharacterController characterController;
    private bool isExhausted = false; // เผื่อทำระบบเหนื่อยหอบ (วิ่งไม่ได้ชั่วคราว)

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    void Start()
    {
        currentStamina = maxStamina;
        characterController = GetComponent<CharacterController>();

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        // ตรวจสอบสถานะการเคลื่อนไหวของ Player
        bool isMoving = characterController != null && characterController.velocity.magnitude > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isExhausted;

        if (isRunning)
        {
            // --- กำลังวิ่ง: ลด Stamina ---
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true; // หมดแรง วิ่งต่อไม่ได้ชั่วคราว
            }
        }
        else
        {
            // ถ้าพักเหนื่อยจน Stamina กลับมามากกว่า 20% ให้กลับมาวิ่งได้ปกติ
            if (currentStamina >= maxStamina * 0.2f)
            {
                isExhausted = false;
            }

            if (!isMoving)
            {
                // --- ยืนอยู่กับที่: เพิ่ม 2 หน่วยต่อวินาที ---
                currentStamina += idleRegenRate * Time.deltaTime;
            }
            else
            {
                // --- กำลังเดิน: เพิ่ม 1 หน่วยต่อวินาที ---
                currentStamina += walkRegenRate * Time.deltaTime;
            }

            // จำกัดไม่ให้เกินค่าสูงสุด
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        // อัปเดต UI หลอดสตามิน่า
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    // ฟังก์ชันเช็คว่าผู้เล่นสามารถวิ่งได้หรือไม่ (สามารถนำไปเรียกใช้ในสคริปต์เคลื่อนที่ของ Player ได้)
    public bool CanRun()
    {
        return !isExhausted && currentStamina > 0;
    }
}