//using UnityEngine;
//using System.Collections;

//public class GunController : MonoBehaviour
//{
//    [Header("Gun Settings")]
//    [SerializeField] private GameObject bulletPrefab;      // ลาก Prefab ของกระสุนมาใส่ที่นี่
//    [SerializeField] private Transform firePoint;        // ตำแหน่งปากกระบอกปืน (จุดที่กระสุนจะพุ่งออกมา)
//    [SerializeField] private float fireRate = 0.2f;        // อัตราความเร็วในการยิง (วินาทีต่อ 1 นัด)
//    [SerializeField] private int maxMagCapacity = 30;      // ความจุกระสุนสูงสุดต่อแมกกาซีน
//    [SerializeField] private int currentAmmo = 30;         // กระสุนปัจจุบัน
//    [SerializeField] private int totalReserveAmmo = 90;    // กระสุนสำรองทั้งหมด
//    [SerializeField] private float reloadTime = 1.5f;      // เวลารีโหลด

//    private float nextFireTime = 0f;
//    private bool isReloading = false;

//    void Start()
//    {
//        currentAmmo = maxMagCapacity;

//        // ถ้ายังไม่ได้สร้าง Fire Point แยก ให้ใช้ตำแหน่งของปืนเองชั่วคราว
//        if (firePoint == null)
//        {
//            firePoint = transform;
//        }
//    }

//    void Update()
//    {
//        if (isReloading) return;

//        // กด R หรือกระสุนหมดเพื่อรีโหลด
//        if ((Input.GetKeyDown(KeyCode.R) && currentAmmo < maxMagCapacity && totalReserveAmmo > 0) ||
//            (currentAmmo <= 0 && totalReserveAmmo > 0))
//        {
//            StartCoroutine(Reload());
//            return;
//        }

//        // คลิกซ้ายยิงปืน (Fire1)
//        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
//        {
//            if (currentAmmo > 0)
//            {
//                nextFireTime = Time.time + fireRate;
//                Shoot();
//            }
//        }
//    }

//    private void Shoot()
//    {
//        currentAmmo--;
//        Debug.Log("Shooting! Ammo left: " + currentAmmo + "/" + totalReserveAmmo);

//        // สร้างกระสุน (Instantiate) ออกมาจากตำแหน่งปากกระบอกปืนและหันหน้าไปทิศทางเดียวกับปืน
//        if (bulletPrefab != null && firePoint != null)
//        {
//            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//        }
//        else
//        {
//            Debug.LogWarning("Bullet Prefab or FirePoint is not assigned in GunController!");
//        }
//    }

//    private IEnumerator Reload()
//    {
//        isReloading = true;
//        Debug.Log("Reloading...");

//        yield return new WaitForSeconds(reloadTime);

//        int ammoNeeded = maxMagCapacity - currentAmmo;
//        if (totalReserveAmmo >= ammoNeeded)
//        {
//            totalReserveAmmo -= ammoNeeded;
//            currentAmmo = maxMagCapacity;
//        }
//        else
//        {
//            currentAmmo += totalReserveAmmo;
//            totalReserveAmmo = 0;
//        }

//        isReloading = false;
//        Debug.Log("Reload complete! Current Ammo: " + currentAmmo);
//    }

//    public void AddAmmo(int amount)
//    {
//        totalReserveAmmo += amount;
//        Debug.Log("Added ammo! Total Reserve: " + totalReserveAmmo);
//    }
//}








using UnityEngine;
using System.Collections;
using TMPro; // นำเข้า TextMeshPro

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private int maxMagCapacity = 30;     // ความจุสูงสุดในแม็ก
    [SerializeField] private int currentAmmo = 30;        // กระสุนปัจจุบันในแม็ก
    [SerializeField] private int totalReserveAmmo = 90;   // กระสุนสำรองจำกัด
    [SerializeField] private float reloadTime = 1.5f;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI ammoText;   // ลาก AmmoTextUI มาใส่ที่นี่

    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxMagCapacity;
        if (firePoint == null) firePoint = transform;

        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        // กด R รีโหลด
        if ((Input.GetKeyDown(KeyCode.R) && currentAmmo < maxMagCapacity && totalReserveAmmo > 0) ||
            (currentAmmo <= 0 && totalReserveAmmo > 0))
        {
            StartCoroutine(Reload());
            return;
        }

        // คลิกซ้ายยิง
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI(); // อัปเดตตัวเลขทันทีที่ยิง

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        if (ammoText != null) ammoText.text = "RELOADING...";

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxMagCapacity - currentAmmo;
        if (totalReserveAmmo >= ammoNeeded)
        {
            totalReserveAmmo -= ammoNeeded;
            currentAmmo = maxMagCapacity;
        }
        else
        {
            currentAmmo += totalReserveAmmo;
            totalReserveAmmo = 0;
        }

        isReloading = false;
        UpdateAmmoUI(); // อัปเดตตัวเลขหลังรีโหลดเสร็จ
    }

    // เพิ่มกระสุนสำรองจากการเก็บ AmmoPickup
    public void AddAmmo(int amount)
    {
        totalReserveAmmo += amount;
        UpdateAmmoUI(); // อัปเดตตัวเลขทันทีที่เก็บกระสุน
        Debug.Log("Added ammo! Total Reserve: " + totalReserveAmmo);
    }

    // ฟังก์ชันอัปเดต UI ข้อความกระสุน
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {totalReserveAmmo}";
        }
    }
}