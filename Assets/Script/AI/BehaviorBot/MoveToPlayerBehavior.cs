using UnityEngine;

public class MoveToPlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;
    public TargetingBehavior targetingBehavior;
    public IdleBehavior idleBehavior;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("[Awake] Rigidbody 2D đã được gán: " + (rb != null));
    }

    private void Update()
    {
        if(targetingBehavior == null)
        {
            Debug.Log("[Update] targettingBehavior chưa được gán!");
            return;
        }

        Debug.Log("[Update] isPlayerDetected = " + targetingBehavior.isPlayerDetected);

        if (targetingBehavior != null && targetingBehavior.isPlayerDetected)
        {
            Debug.Log("[Update] Người chơi đã bị phát hiện -> di chuyển đến.");
            MoveToTarget();
        }
        else
        {
            Debug.Log("[Update] Người chơi KHÔNG bị phát hiện -> dừng lại");
            if(idleBehavior != null)
            {
                idleBehavior.DoIdle();
            }
            else
            {
                Debug.Log("[Update] idleBehavior chưa được gán!");
            }
            
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void MoveToTarget()
    {
        if(targetingBehavior.player == null)
        {
            Debug.Log("[MoveToTarget] Biến player chưa được gán trong TargetingBehavior!");
            return;
        }

        Vector2 direction = (targetingBehavior.player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        Debug.Log("[MoveToTarget] Hướng di chuyển: " + direction + " | Vận tốc: " + rb.linearVelocity);
    }
}
