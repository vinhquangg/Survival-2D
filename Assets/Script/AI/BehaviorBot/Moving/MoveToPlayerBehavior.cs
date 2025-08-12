using UnityEngine;

public class MoveToPlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;
    public MonoBehaviour targetingComponent;
    public IdleBehavior idleBehavior;
    
    [HideInInspector]public ITargetBehavior targetingBehavior;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("[Awake] Rigidbody 2D đã được gán: " + (rb != null));

        targetingBehavior = targetingComponent as ITargetBehavior;
    }

    private void Update()
    {
        if(targetingBehavior == null)
        {
            Debug.Log("[Update] targettingBehavior chưa được gán!");
            return;
        }

        Debug.Log("[Update] isPlayerDetected = " + targetingBehavior.IsPlayerDetected);

        if (targetingBehavior.IsPlayerDetected)
        {
            Debug.Log("[Update] Người chơi đã bị phát hiện -> di chuyển đến.");
            MoveToTarget();
        }
        else
        {
            Debug.Log("[Update] Người chơi KHÔNG bị phát hiện -> dừng lại");
            idleBehavior?.DoIdle();
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void MoveToTarget()
    {
        if(targetingBehavior.Target == null)
        {
            Debug.Log("[MoveToTarget] Biến player chưa được gán trong TargetingBehavior!");
            return;
        }

        Vector2 direction = (targetingBehavior.Target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        Debug.Log("[MoveToTarget] Hướng di chuyển: " + direction + " | Vận tốc: " + rb.linearVelocity);
    }
}
