using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] private int ammoAmount = 15; // จำนวนกระสุนที่จะได้เมื่อเก็บ

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าชนกับ Player หรือไม่
        if (other.CompareTag("Player"))
        {
            // ค้นหา GunController จากตัว Player (หรือลูกของ Player)
            GunController gun = other.GetComponentInChildren<GunController>();
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                //Debug.Log("Picked up " + ammoAmount + " rounds of ammo.");
                Destroy(gameObject); // ทำลายกล่องกระสุนทิ้งหลังเก็บ
            }
        }
    }
}