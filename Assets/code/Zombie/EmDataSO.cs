using UnityEngine;

[CreateAssetMenu(fileName = "NewEmData", menuName = "Em System/Em Data")]
public class EmDataSO : ScriptableObject
{
    [field: SerializeField] public string EmName { get; private set; } = "Basic Em";
    [field: SerializeField] public float DetectRadius { get; private set; } = 5.0f;       // รัศมี r = 5
    [field: SerializeField] public float StoppingDistance { get; private set; } = 1.0f; // ระยะหยุดชิดตัว
    [field: SerializeField] public float WanderRadius { get; private set; } = 5.0f;    // รัศมีสุ่มเดิน
    [field: SerializeField] public float WanderSpeed { get; private set; } = 1.0f;      // ความเร็วเดินสุ่ม
    [field: SerializeField] public float ChaseSpeed { get; private set; } = 1f;       // ความเร็ววิ่งตาม Player
    [field: SerializeField] public float WaitTime { get; private set; } = 1.0f;         // เวลายืนรอ 6 วินาที
}