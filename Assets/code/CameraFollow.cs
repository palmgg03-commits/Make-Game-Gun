using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // ลากตัว Player มาใส่ที่นี่

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 15f, -10f); // ระยะห่างระหว่างกล้องกับ Player
    [SerializeField] private float smoothSpeed = 5f; // ความเร็วในการเลื่อนตาม (ยิ่งมากยิ่งตามไว)

    void LateUpdate()
    {
        if (target == null) return;

        // 1. คำนวณตำแหน่งเป้าหมายที่กล้องควรจะไป
        Vector3 targetPosition = target.position + offset;

        // 2. เคลื่อนที่เข้าหาเป้าหมายแบบสมูท (Lerp)
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}