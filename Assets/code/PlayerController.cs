using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;   // ความเร็วตอนเดินปกติ
    public float runSpeed = 6f;    // ความเร็วตอนวิ่ง (กด Shift)
    public float rotationSpeed = 15f; // ความเร็วในการหันหน้าตัวละคร

    private CharacterController controller;
    private Animator animator;
    private Transform camTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // ดึงตำแหน่งของ Main Camera มาใช้คำนวณทิศทางการเดิน
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // 1. รับค่าปุ่มกด W, A, S, D (Horizontal = A/D, Vertical = W/S)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetSpeed = 0f;

        if (direction.magnitude >= 0.1f)
        {
            // 2. เช็คว่ากดปุ่ม Shift ค้างไว้หรือไม่ เพื่อเปลี่ยนเป็นวิ่ง
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            targetSpeed = isRunning ? runSpeed : walkSpeed;

            // 3. คำนวณมุมการหันหน้าของตัวละครให้สัมพันธ์กับมุมกล้อง
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + (camTransform != null ? camTransform.eulerAngles.y : 0f);

            // หมุนตัวละครไปตามทิศทางที่กดอย่างนุ่มนวล
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // 4. เคลื่อนที่ไปข้างหน้าตามทิศทางที่หัน
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);
        }

        // เพิ่มแรงโน้มถ่วงดึงลงพื้น
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }

        // 5. ส่งค่าความเร็วไปยัง Animator
        if (animator != null)
        {
            // ถ้าไม่ได้กดเดิน ค่า Speed = 0 (Idle)
            // ถ้าเดินธรรมดา ค่า Speed = 0.5 (Walk)
            // ถ้าวิ่ง (กด Shift) ค่า Speed = 1.0 (Run)
            float animSpeedValue = 0f;
            if (direction.magnitude >= 0.1f)
            {
                bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                animSpeedValue = isRunning ? 1.0f : 0.5f;
            }

            // ส่งค่าเข้า Parameter ชื่อ "Speed" ใน Animator (แนะนำให้ใช้ Float)
            animator.SetFloat("Speed", animSpeedValue, 0.1f, Time.deltaTime);
        }
    }
}