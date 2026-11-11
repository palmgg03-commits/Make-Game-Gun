//using UnityEngine;

//public class EnemyFollow : MonoBehaviour
//{
//    [Header("Target & Settings")]
//    public Transform player;          // ลาก Player Object มาใส่ที่นี่
//    public float detectRadius = 5f;   // รัศมีการตรวจจับ (r = 5 ตามต้องการ)
//    public float moveSpeed = 1f;    // ความเร็วในการเดินของศัตรู
//    public float stoppingDistance = 1.0f; // ระยะที่จะให้หยุดเดิน (ไม่ให้เดินไปเบียดชิดตัว Player เกินไป)

//    void Update()
//    {
//        // ป้องกัน Error ถ้าไม่ได้ใส่ Player
//        if (player == null) return;

//        // 1. คำนวณระยะห่างระหว่าง ศัตรู กับ Player (แบบ 3D หรือจะใช้ Vector2 ถ้านับแค่ระนาบแบนก็ได้ครับ)
//        float distance = Vector3.Distance(transform.position, player.position);

//        // 2. ถ้า Player เข้ามาอยู่ในรัศมี 5 หน่วย
//        if (distance <= detectRadius)
//        {
//            // ถ้าเข้าใกล้เกินระยะหยุด ให้หยุดเดิน แต่ยังหันหน้ามอง Player อยู่
//            if (distance > stoppingDistance)
//            {
//                // เคลื่อนที่เข้าหา Player ทีละนิดด้วย MoveTowards
//                transform.position = Vector3.MoveTowards(
//                    transform.position,
//                    player.position,
//                    moveSpeed * Time.deltaTime
//                );
//            }

//            // หมุนหน้าหันไปหา Player เสมอเมื่ออยู่ในระยะ
//            Vector3 targetDirection = new Vector3(player.position.x, transform.position.y, player.position.z);
//            transform.LookAt(targetDirection);
//        }
//    }

//    // ฟังก์ชันช่วยวาดเส้นวงกลมจำลองรัศมี R=5 ในหน้า Scene View (เพื่อให้มองเห็นง่ายตอนจัดฉาก)
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, detectRadius);

//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
//    }
//}

using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Target & Settings")]
    public Transform player;
    public float detectRadius = 5f;
    public float moveSpeed = 3.5f;
    public float stoppingDistance = 1.0f;

    private Animator animator; // ตัวแปรสำหรับเรียกใช้ Animator

    void Start()
    {
        // ดึง Animator component ที่ติดอยู่กับตัวศัตรูอัตโนมัติ
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool isMoving = false; // ตัวแปรเช็คสถานะการเดิน

        // ถ้า Player เข้ามาอยู่ในรัศมี 5 หน่วย
        if (distance <= detectRadius)
        {
            // ถ้ายังไม่ถึงระยะหยุด ให้เดินเข้าหา
            if (distance > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );

                isMoving = true; // เปลี่ยนสถานะเป็นกำลังเดิน
            }

            // หมุนหน้าหันไปหา Player เสมอเมื่ออยู่ในระยะ
            Vector3 targetDirection = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetDirection);
        }

        // ส่งค่าสถานะ isMoving ไปยัง Animator Parameter ชื่อ "IsWalking"
        if (animator != null)
        {
            animator.SetBool("IsWalking", isMoving);
        }
    }

    // วาดเส้นรัศมีใน Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}