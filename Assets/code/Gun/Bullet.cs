using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 20f;      // ความเร็วในการพุ่งของกระสุน
    [SerializeField] private float damage = 25f;     // ดาเมจที่ทำต่อ Em
    [SerializeField] private float lifeTime = 3f;    // ระยะเวลาทำลายตัวเองหากยิงไม่โดนอะไรเลย

    private void Start()
    {
        // ทำลายกระสุนตัวเองอัตโนมัติหลังผ่านไป 3 วินาที (ป้องกันรกแรม/ซีน)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // ให้กระสุนพุ่งไปข้างหน้า (ตามแกน Z ของตัวมันเอง) ด้วยความเร็วที่กำหนด
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ป้องกันกระสุนชนกับตัว Player เอง
        if (other.CompareTag("Player")) return;

        // ตรวจสอบว่าชนกับวัตถุที่มีเลือด (IDamageable) เช่น Em หรือไม่
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage); // สร้างดาเมจให้ Em
        }

        // เมื่อชนกับอะไรก็ตาม (พื้น, กำแพง, หรือ Em) ให้ทำลายกระสุนทิ้งทันที
        Destroy(gameObject);
    }
}