using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private int itemValue = 1;

    void Reset()
    {
        // ตั้งค่า Collider เป็น Trigger อัตโนมัติตอนสร้างคอมโพเนนต์
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelObjectiveManager.Instance != null)
            {
                LevelObjectiveManager.Instance.CollectItem(itemValue);
            }

            // ทำลายไอเทมชิ้นนี้เมื่อเก็บแล้ว
            Destroy(gameObject);
        }
    }
}