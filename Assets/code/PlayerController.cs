using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 15f;

    [Header("Health & UI")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    public GameObject gameOverPanel;
    [SerializeField] private Slider healthSlider; // ลากหลอดเลือด Player มาใส่ที่นี่

    [Header("Stamina Settings & UI")]
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaDrainRate = 20f;   // วิ่งลด 20 หน่วย/วินาที
    [SerializeField] private float idleRegenRate = 2.0f;     // ยืนนิ่งเพิ่ม 2 หน่วย/วินาที
    [SerializeField] private float walkRegenRate = 1.0f;     // เดินเพิ่ม 1 หน่วย/วินาที
    private bool isExhausted = false;                        // สถานะเหนื่อยหอบ วิ่งชั่วคราวไม่ได้
    [SerializeField] private Slider staminaSlider;           // ลากหลอด Stamina มาใส่ที่นี่

    private CharacterController controller;
    private Animator animator;
    private Transform camTransform;

    public float CurrentHealth => currentHealth;
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // ตั้งค่าเริ่มต้น UI Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        if (isDead) return;

        // 1. รับค่าปุ่มกด W, A, S, D
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isMoving = direction.magnitude >= 0.1f;

        // เช็คการวิ่ง (กด Shift + กำลังเคลื่อนที่ + มีสามิน่า + ไม่เหนื่อยหอบ)
        bool isRunning = isMoving && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentStamina > 0 && !isExhausted;

        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        // 2. จัดการระบบ Stamina (ลดตอนวิ่ง / เพิ่มตอนยืนหรือเดิน)
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true; // หมดแรง ห้ามวิ่งชั่วคราว
            }
        }
        else
        {
            // ถ้ารีจนถึง 20% ให้กลับมาวิ่งได้ปกติ
            if (currentStamina >= maxStamina * 0.2f)
            {
                isExhausted = false;
            }

            if (!isMoving)
            {
                currentStamina += idleRegenRate * Time.deltaTime; // ยืนนิ่ง +2
            }
            else
            {
                currentStamina += walkRegenRate * Time.deltaTime; // เดิน +1
            }

            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        // อัปเดต UI Stamina Slider
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }

        // 3. การเคลื่อนที่และการหมุนตัวละคร
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + (camTransform != null ? camTransform.eulerAngles.y : 0f);

            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            // ถ้าวิ่งให้ใช้ความเร็ว runSpeed ถ้าเดินใช้ walkSpeed
            controller.Move(moveDir.normalized * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime);
        }

        // เพิ่มแรงโน้มถ่วงดึงลงพื้น
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }

        // 4. ส่งค่าความเร็วไปยัง Animator
        if (animator != null)
        {
            float animSpeedValue = 0f;
            if (isMoving)
            {
                animSpeedValue = isRunning ? 1.0f : 0.5f;
            }
            animator.SetFloat("Speed", animSpeedValue, 0.1f, Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // อัปเดต UI หลอดเลือดทันทีที่โดนดาเมจ
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        enabled = false;
    }
}

































// --------------------------------------------------------------- Back Up ----------------------------------------------------------------------------------------------
//using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
//public class PlayerController : MonoBehaviour, IDamageable
//{
//    [Header("Movement Settings")]
//    public float walkSpeed = 3f;   // ความเร็วตอนเดินปกติ
//    public float runSpeed = 6f;    // ความเร็วตอนวิ่ง (กด Shift)
//    public float rotationSpeed = 15f; // ความเร็วในการหันหน้าตัวละคร



//    [Header("Health & UI")]
//    [SerializeField] private float maxHealth = 100f;
//    private float currentHealth;
//    private bool isDead = false;

//    [Header("UI Reference")]
//    public GameObject gameOverPanel; // ลาก UI Panel แพ้มาใส่ที่นี่


//    private CharacterController controller;
//    private Animator animator;
//    private Transform camTransform;




//    public float CurrentHealth => currentHealth;

//    void Start()
//    {
//        currentHealth = maxHealth;
//        controller = GetComponent<CharacterController>();
//        animator = GetComponent<Animator>();

//        // ดึงตำแหน่งของ Main Camera มาใช้คำนวณทิศทางการเดิน
//        if (Camera.main != null)
//        {
//            camTransform = Camera.main.transform;
//        }

//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(false); // ซ่อน UI แพ้ตอนเริ่มเกม
//        }
//    }

//    void Update()
//    {
//        if (isDead) return;

//        // 1. รับค่าปุ่มกด W, A, S, D (Horizontal = A/D, Vertical = W/S)
//        float horizontal = Input.GetAxisRaw("Horizontal");
//        float vertical = Input.GetAxisRaw("Vertical");
//        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//        float targetSpeed = 0f;

//        if (direction.magnitude >= 0.1f)
//        {
//            // 2. เช็คว่ากดปุ่ม Shift ค้างไว้หรือไม่ เพื่อเปลี่ยนเป็นวิ่ง
//            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
//            targetSpeed = isRunning ? runSpeed : walkSpeed;

//            // 3. คำนวณมุมการหันหน้าของตัวละครให้สัมพันธ์กับมุมกล้อง
//            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + (camTransform != null ? camTransform.eulerAngles.y : 0f);

//            // หมุนตัวละครไปตามทิศทางที่กดอย่างนุ่มนวล
//            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
//            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

//            // 4. เคลื่อนที่ไปข้างหน้าตามทิศทางที่หัน
//            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
//            controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);
//        }

//        // เพิ่มแรงโน้มถ่วงดึงลงพื้น
//        if (!controller.isGrounded)
//        {
//            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
//        }

//        // 5. ส่งค่าความเร็วไปยัง Animator
//        if (animator != null)
//        {
//            // ถ้าไม่ได้กดเดิน ค่า Speed = 0 (Idle)
//            // ถ้าเดินธรรมดา ค่า Speed = 0.5 (Walk)
//            // ถ้าวิ่ง (กด Shift) ค่า Speed = 1.0 (Run)
//            float animSpeedValue = 0f;
//            if (direction.magnitude >= 0.1f)
//            {
//                bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
//                animSpeedValue = isRunning ? 1.0f : 0.5f;
//            }

//            // ส่งค่าเข้า Parameter ชื่อ "Speed" ใน Animator (แนะนำให้ใช้ Float)
//            animator.SetFloat("Speed", animSpeedValue, 0.1f, Time.deltaTime);
//        }







//    }
//    public void TakeDamage(float amount)
//    {
//        if (isDead) return;

//        currentHealth -= amount;
//        //Debug.Log("Player HP Left: " + currentHealth);

//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    public void Die()
//    {
//        if (isDead) return;
//        isDead = true;

//        // เล่นอนิเมชันตายของ Player (Trigger ชื่อ "Die")
//        if (animator != null)
//        {
//            animator.SetTrigger("Die");
//        }

//        // แสดง UI แพ้
//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(true);
//        }

//        // ปิดการควบคุม
//        enabled = false;
//    }
//}

//---------------------------------------------------------------------------------- Back Up ------------------------------------------------------------------------------------------------------------------------------------------


//using UnityEngine;
//using UnityEngine.UI; // สำหรับจัดการ UI

//[RequireComponent(typeof(CharacterController))]
//public class PlayerController : MonoBehaviour, IDamageable
//{
//    [Header("Movement Settings")]
//    public float walkSpeed = 3f;
//    public float runSpeed = 6f;
//    public float rotationSpeed = 15f;

//    [Header("Health & UI")]
//    [SerializeField] private float maxHealth = 100f;
//    private float currentHealth;
//    private bool isDead = false;

//    [Header("UI Reference")]
//    public GameObject gameOverPanel; // ลาก UI Panel แพ้มาใส่ที่นี่

//    private CharacterController controller;
//    private Animator animator;
//    private Transform camTransform;

//    public float CurrentHealth => currentHealth;

//    void Start()
//    {
//        currentHealth = maxHealth;
//        controller = GetComponent<CharacterController>();
//        animator = GetComponent<Animator>();

//        if (Camera.main != null)
//        {
//            camTransform = Camera.main.transform;
//        }

//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(false); // ซ่อน UI แพ้ตอนเริ่มเกม
//        }
//    }

//    void Update()
//    {
//        if (isDead) return;

//        // การควบคุมการเดิน (W, A, S, D + Shift) ตามเดิม
//        float horizontal = Input.GetAxisRaw("Horizontal");
//        float vertical = Input.GetAxisRaw("Vertical");
//        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//        float targetSpeed = 0f;
//        if (direction.magnitude >= 0.1f)
//        {
//            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
//            targetSpeed = isRunning ? runSpeed : walkSpeed;

//            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + (camTransform != null ? camTransform.eulerAngles.y : 0f);
//            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
//            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

//            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
//            controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);
//        }

//        if (!controller.isGrounded)
//        {
//            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
//        }

//        // จัดการ Animator ของ Player
//        if (animator != null)
//        {
//            float animSpeedValue = 0f;
//            if (direction.magnitude >= 0.1f)
//            {
//                bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
//                animSpeedValue = isRunning ? 1.0f : 0.5f;
//            }
//            animator.SetFloat("Speed", animSpeedValue, 0.1f, Time.deltaTime);
//        }
//    }

//    public void TakeDamage(float amount)
//    {
//        if (isDead) return;

//        currentHealth -= amount;
//        Debug.Log("Player HP Left: " + currentHealth);

//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    public void Die()
//    {
//        if (isDead) return;
//        isDead = true;

//        // เล่นอนิเมชันตายของ Player (Trigger ชื่อ "Die")
//        if (animator != null)
//        {
//            animator.SetTrigger("Die");
//        }

//        // แสดง UI แพ้
//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(true);
//        }

//        // ปิดการควบคุม
//        enabled = false;
//    }
//}