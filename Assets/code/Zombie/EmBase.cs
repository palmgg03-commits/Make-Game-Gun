using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class EmBase : MonoBehaviour, IMovable
{
    [Header("Data Configuration")]
    [SerializeField] protected EmDataSO emData;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform playerTarget;

    // Encapsulated Property ตามหลัก OOP
    public EmDataSO EmData => emData;
    public bool IsMoving => agent != null && agent.velocity.magnitude > 0.1f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        // ค้นหา Player อัตโนมัติจาก Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    // Implementation จาก Interface IMovable
    public virtual void MoveTo(Vector3 destination)
    {
        if (agent != null) agent.SetDestination(destination);
    }

    public virtual void StopMoving()
    {
        if (agent != null) agent.ResetPath();
    }

    // Virtual Method สำหรับจัดการ Animator
    protected virtual void UpdateAnimator(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }
}