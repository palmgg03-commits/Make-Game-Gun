using UnityEngine;
using UnityEngine.AI;

public class EmAI : EmBase
{
    private float waitTimer;
    private bool isWaiting = false;
    private Vector3 startPosition;

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
        if (emData != null)
        {
            waitTimer = emData.WaitTime;
        }
    }

    void Update()
    {
        if (playerTarget == null || emData == null) return;

        // คำนวณระยะห่างระหว่าง Em กับ Player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // ----------------------------------------------------------------
        // สถานการณ์ที่ 1: Player เข้ามาในรัศมี (r = 5) -> เดินตาม Player
        // ----------------------------------------------------------------
        if (distanceToPlayer <= emData.DetectRadius)
        {
            HandleChasing(distanceToPlayer);
        }
        // ----------------------------------------------------------------
        // สถานการณ์ที่ 2: Player อยู่นอกรัศมี -> เดินสุ่ม + ยืนรอ 6 วินาที
        // ----------------------------------------------------------------
        else
        {
            HandleWandering();
        }
    }

    private void HandleChasing(float distanceToPlayer)
    {
        agent.speed = emData.ChaseSpeed;
        isWaiting = false; // รีเซ็ตสถานะยืนรอ

        if (distanceToPlayer > emData.StoppingDistance)
        {
            MoveTo(playerTarget.position);
            UpdateAnimator(true);
        }
        else
        {
            StopMoving();
            UpdateAnimator(false);

            // หันหน้ามอง Player เสมอเมื่อถึงระยะหยุด
            Vector3 lookDirection = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
            transform.LookAt(lookDirection);
        }
    }

    //private void HandleWandering()
    //{
    //    agent.speed = emData.WanderSpeed;

    //    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    //    {
    //        if (!isWaiting)
    //        {
    //            // เริ่มต้นยืนรอ 6 วินาที
    //            isWaiting = true;
    //            waitTimer = emData.WaitTime;
    //            UpdateAnimator(false);
    //        }
    //        else
    //        {
    //            // นับเวลาถอยหลังระหว่างยืนรอ
    //            waitTimer -= Time.deltaTime;
    //            if (waitTimer <= 0f)
    //            {
    //                // หมดเวลารอ -> สุ่มจุดเดินใหม่
    //                Vector3 randomPoint = GetRandomNavMeshPoint(startPosition, emData.WanderRadius);
    //                MoveTo(randomPoint);
    //                isWaiting = false;
    //                UpdateAnimator(true);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        UpdateAnimator(true);
    //    }
    //}

    //private Vector3 GetRandomNavMeshPoint(Vector3 center, float range)
    //{
    //    Vector3 randomDirection = Random.insideUnitSphere * range;
    //    randomDirection += center;
    //    NavMeshHit hit;
    //    NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas);
    //    return hit.position;
    //}


    private void HandleWandering()
    {
        agent.speed = emData.WanderSpeed;

        // เช็คว่าถึงจุดหมาย หรือยังไม่มีเส้นทางเดิน
        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath))
        {
            if (!isWaiting)
            {
                // เริ่มต้นยืนรอ 6 วินาทีตามที่ตั้งไว้
                isWaiting = true;
                waitTimer = emData.WaitTime;
                UpdateAnimator(false);
            }
            else
            {
                // นับเวลาถอยหลังระหว่างยืนรอ
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    // หมดเวลารอ -> สุ่มจุดเดินใหม่รอบตัว Em ในรัศมี WanderRadius
                    Vector3 randomPoint = GetRandomNavMeshPoint(transform.position, emData.WanderRadius);
                    MoveTo(randomPoint);
                    isWaiting = false;
                    UpdateAnimator(true);
                }
            }
        }
        else
        {
            UpdateAnimator(true);
        }
    }

    // ฟังก์ชันสุ่มตำแหน่งบน NavMesh ที่ปลอดภัยยิ่งขึ้น
    private Vector3 GetRandomNavMeshPoint(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++) // พยายามสุ่มหาจุดที่ถูกต้องสูงสุด 30 ครั้ง
        {
            Vector3 randomDirection = Random.insideUnitSphere * range;
            randomDirection += center;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return center; // ถ้าหาไม่เจอให้ยืนอยู่ที่เดิมก่อน
    }






    // วาด Gizmos จำลองรัศมี R=5 ในหน้า Scene View
    private void OnDrawGizmosSelected()
    {
        if (emData != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, emData.DetectRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, emData.StoppingDistance);
        }
    }

}