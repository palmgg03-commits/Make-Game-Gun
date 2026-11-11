

//using UnityEngine;
//using System.Collections;

//public class EmAI : EmBase, IDamageable
//{
//    [Header("Combat Settings")]
//    [SerializeField] private float maxHealth = 50f;
//    [SerializeField] private float attackDamage = 10f;
//    [SerializeField] private float attackCooldown = 1.5f; // โจมตีทุกๆ 1.5 วินาที
//    private float currentHealth;
//    private float lastAttackTime;
//    private bool isDead = false;




//    public float CurrentHealth => currentHealth;

//    protected override void Awake()
//    {
//        base.Awake();
//        currentHealth = maxHealth;
//    }

//    void Update()
//    {
//        if (isDead || playerTarget == null || emData == null) return;

//        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

//        // ถ้า Player อยู่ในรัศมีตรวจจับ (r = 5)
//        if (distanceToPlayer <= emData.DetectRadius)
//        {
//            // ถ้าถึงระยะโจมตี/หยุดเดิน ให้ทำการโจมตี
//            if (distanceToPlayer <= emData.StoppingDistance)
//            {
//                StopMoving();
//                UpdateAnimator(false);

//                // หันหน้ามอง Player
//                Vector3 lookDirection = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
//                transform.LookAt(lookDirection);

//                // สั่งโจมตีตามเวลา Cooldown
//                TryAttackPlayer();
//            }
//            else
//            {
//                // เดินตาม Player
//                agent.speed = emData.ChaseSpeed;
//                MoveTo(playerTarget.position);
//                UpdateAnimator(true);
//            }
//        }
//        else
//        {
//            // อยู่นอกรัศมี -> เดินสุ่ม + ยืนรอ 6 วินาที (จากโค้ดเดิม)
//            HandleWandering();
//        }
//    }

//    private void TryAttackPlayer()
//    {
//        if (Time.time >= lastAttackTime + attackCooldown)
//        {
//            lastAttackTime = Time.time;

//            // เล่นอนิเมชันโจมตี (Trigger ใน Animator ชื่อ "Attack")
//            if (animator != null)
//            {
//                animator.SetTrigger("Attack");
//            }

//            // สร้างดาเมจให้ Player (ถ้า Player มีคอมโพเนนต์ IDamageable)
//            IDamageable playerHealth = playerTarget.GetComponent<IDamageable>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(attackDamage);
//            }
//        }
//    }

//    // ฟังก์ชันรับดาเมจของ Em
//    public void TakeDamage(float amount)
//    {
//        if (isDead) return;

//        currentHealth -= amount;
//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    // ระบบความตายของ Em (หน่วง 2 วินาทีแล้วลบ Object)
//    public void Die()
//    {
//        if (isDead) return;
//        isDead = true;

//        StopMoving();
//        enabled = false; // ปิด Script AI
//        if (agent != null) agent.enabled = false; // ปิด NavMeshAgent

//        // เล่นอนิเมชันตาย (Trigger หรือ Bool ใน Animator ชื่อ "Die")
//        if (animator != null)
//        {
//            animator.SetTrigger("Die");
//        }

//        // เริ่ม Coroutine หน่วงเวลา 2 วินาทีก่อนลบ Object
//        StartCoroutine(DestroyAfterDelay(2.0f));
//    }

//    private IEnumerator DestroyAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        Destroy(gameObject);
//    }

//    // (ฟังก์ชัน HandleWandering และอื่นๆ คงเดิมตามโค้ดก่อนหน้า)
//    private void HandleWandering() { /* ใช้โค้ดสุ่มเดินเดิมได้เลยครับ */ }
//}













//using UnityEngine;
//using UnityEngine.AI;

//public class EmAI : EmBase , IDamageable
//{
//    private float waitTimer;
//    private bool isWaiting = false;
//    private Vector3 startPosition;

//    [Header("Combat Settings")]
//    [SerializeField] private float maxHealth = 50f;
//    [SerializeField] private float attackDamage = 10f;
//    [SerializeField] private float attackCooldown = 1.5f; // โจมตีทุกๆ 1.5 วินาที
//    private float currentHealth;
//    private float lastAttackTime;
//    private bool isDead = false;

//    public float CurrentHealth => currentHealth;




//    protected override void Awake()
//    {
//        base.Awake();
//        currentHealth = maxHealth;
//        startPosition = transform.position;
//        if (emData != null)
//        {
//            waitTimer = emData.WaitTime;
//        }
//    }

//    void Update()
//    {
//        if (playerTarget == null || emData == null) return;

//        // คำนวณระยะห่างระหว่าง Em กับ Player
//        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

//        // ----------------------------------------------------------------
//        // สถานการณ์ที่ 1: Player เข้ามาในรัศมี (r = 5) -> เดินตาม Player
//        // ----------------------------------------------------------------
//        if (distanceToPlayer <= emData.DetectRadius)
//        {
//            HandleChasing(distanceToPlayer);
//        }
//        // ----------------------------------------------------------------
//        // สถานการณ์ที่ 2: Player อยู่นอกรัศมี -> เดินสุ่ม + ยืนรอ 6 วินาที
//        // ----------------------------------------------------------------
//        else
//        {
//            HandleWandering();
//        }
//    }

//    private void HandleChasing(float distanceToPlayer)
//    {
//        agent.speed = emData.ChaseSpeed;
//        isWaiting = false; // รีเซ็ตสถานะยืนรอ

//        if (distanceToPlayer > emData.StoppingDistance)
//        {
//            MoveTo(playerTarget.position);
//            UpdateAnimator(true);
//        }
//        else
//        {
//            StopMoving();
//            UpdateAnimator(false);

//            // หันหน้ามอง Player เสมอเมื่อถึงระยะหยุด
//            Vector3 lookDirection = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
//            transform.LookAt(lookDirection);
//        }
//    }


//    ///----------------------------------------------------------------------------------------------------------------------------

//    private void TryAttackPlayer()
//    {
//        if (Time.time >= lastAttackTime + attackCooldown)
//        {
//            lastAttackTime = Time.time;

//            // เล่นอนิเมชันโจมตี (Trigger ใน Animator ชื่อ "Attack")
//            if (animator != null)
//            {
//                animator.SetTrigger("Attack");
//            }

//            // สร้างดาเมจให้ Player (ถ้า Player มีคอมโพเนนต์ IDamageable)
//            IDamageable playerHealth = playerTarget.GetComponent<IDamageable>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(attackDamage);
//            }
//        }
//    }

//    // ฟังก์ชันรับดาเมจของ Em
//    public void TakeDamage(float amount)
//    {
//        if (isDead) return;

//        currentHealth -= amount;
//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    // ระบบความตายของ Em (หน่วง 2 วินาทีแล้วลบ Object)
//    public void Die()
//    {
//        if (isDead) return;
//        isDead = true;

//        StopMoving();
//        enabled = false; // ปิด Script AI
//        if (agent != null) agent.enabled = false; // ปิด NavMeshAgent

//        // เล่นอนิเมชันตาย (Trigger หรือ Bool ใน Animator ชื่อ "Die")
//        if (animator != null)
//        {
//            animator.SetTrigger("Die");
//        }

//        // เริ่ม Coroutine หน่วงเวลา 2 วินาทีก่อนลบ Object
//        StartCoroutine(DestroyAfterDelay(2.0f));
//    }

//    private IEnumerator DestroyAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        Destroy(gameObject);
//    }

//    //----------------------------------------------------------------------------------------------------------------------------------------------------



//    private void HandleWandering()
//    {
//        agent.speed = emData.WanderSpeed;

//        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//        {
//            if (!isWaiting)
//            {
//                // เริ่มต้นยืนรอ 6 วินาที
//                isWaiting = true;
//                waitTimer = emData.WaitTime;
//                UpdateAnimator(false);
//            }
//            else
//            {
//                // นับเวลาถอยหลังระหว่างยืนรอ
//                waitTimer -= Time.deltaTime;
//                if (waitTimer <= 0f)
//                {
//                    // หมดเวลารอ -> สุ่มจุดเดินใหม่
//                    Vector3 randomPoint = GetRandomNavMeshPoint(startPosition, emData.WanderRadius);
//                    MoveTo(randomPoint);
//                    isWaiting = false;
//                    UpdateAnimator(true);
//                }
//            }
//        }
//        else
//        {
//            UpdateAnimator(true);
//        }
//    }

//    private Vector3 GetRandomNavMeshPoint(Vector3 center, float range)
//    {
//        Vector3 randomDirection = Random.insideUnitSphere * range;
//        randomDirection += center;
//        NavMeshHit hit;
//        NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas);
//        return hit.position;
//    }

//    // วาด Gizmos จำลองรัศมี R=5 ในหน้า Scene View
//    private void OnDrawGizmosSelected()
//    {
//        if (emData != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(transform.position, emData.DetectRadius);

//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere(transform.position, emData.StoppingDistance);
//        }
//    }
//}























using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EmAI : EmBase, IDamageable
{
    [Header("Combat Settings")]
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f; // โจมตีทุกๆ 1.5 วินาที
    [Header("Drop Settings")]
    [SerializeField] private GameObject ammoDropPrefab; // ลาก Prefab กล่องกระสุนมาใส่ที่นี่
    private float currentHealth;
    private float lastAttackTime;
    private bool isDead = false;

    // Property ตามสัญญาของ IDamageable
    public float CurrentHealth => currentHealth;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead || playerTarget == null || emData == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // ถ้า Player อยู่ในรัศมีตรวจจับ (r = 5)
        if (distanceToPlayer <= emData.DetectRadius)
        {
            // ถ้าถึงระยะหยุด/โจมตี ให้ทำการโจมตี
            if (distanceToPlayer <= emData.StoppingDistance)
            {
                StopMoving();
                UpdateAnimator(false);
                LookAtPlayer();

                // หันหน้ามอง Player
                Vector3 lookDirection = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
                transform.LookAt(lookDirection);



                // สั่งโจมตีตามเวลา Cooldown
                TryAttackPlayer();
            }
            else
            {
                // เดินตาม Player
                agent.speed = emData.ChaseSpeed;
                MoveTo(playerTarget.position);
                UpdateAnimator(true);
            }
        }
        else
        {
            // อยู่นอกรัศมี -> เดินสุ่ม + ยืนรอ 6 วินาที
            HandleWandering();
        }


    }

    private void LookAtPlayer()
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0; // ป้องกันไม่ให้ตัว Em เอียงตามแนวแกน Y (ก้มหรือเงย)

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // หมุนตัวเข้าหา Player แบบราบรื่น (Rotation Speed = 10f)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void TryAttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // เล่นอนิเมชันโจมตี (Trigger ใน Animator ชื่อ "Attack")
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            // สร้างดาเมจให้ Player
            IDamageable playerHealth = playerTarget.GetComponent<IDamageable>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        StopMoving();
        enabled = false; // ปิด Script AI
        if (agent != null) agent.enabled = false; // ปิด NavMeshAgent

        // เล่นอนิเมชันตาย (Trigger ใน Animator ชื่อ "Die")
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (ammoDropPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 0.5f; // ยกขึ้นนิดหน่อยกันจมพื้น
            Instantiate(ammoDropPrefab, spawnPosition, Quaternion.identity);
        }


        // หน่วงเวลา 2 วินาทีก่อนลบ Object ออกจากเกม
        StartCoroutine(DestroyAfterDelay(2.0f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void HandleWandering()
    {
        agent.speed = emData.WanderSpeed;

        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath))
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = emData.WaitTime;
                UpdateAnimator(false);
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
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

    private float waitTimer;
    private bool isWaiting = false;

    private Vector3 GetRandomNavMeshPoint(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * range;
            randomDirection += center;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return center;
    }

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

    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าชนโดน Player หรือไม่
        if (other.CompareTag("Player"))
        {
            // ค้นหาคอมโพเนนต์รับดาเมจจากตัว Player
            IDamageable playerHealth = other.GetComponent<IDamageable>();
            if (playerHealth != null)
            {
                // สั่งหักเลือด Player ตามดาเมจของ Em
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }
}