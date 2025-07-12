using UnityEngine;

public class IdleBehavior : MonoBehaviour
{
    public TargetingBehavior targetingBehavior;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("[Awake] Rigidbody 2D đã được gán: " + (rb != null));
    }

    private void Update()
    {
        if(targetingBehavior == null || rb == null)
        {
            Debug.Log("[Update] targetingBehavior hoặc Rigidbody2D chưa được gán!");
            return;
        }

        if (!targetingBehavior.isPlayerDetected)
        {
            DoIdle();
        }
    }

    public void DoIdle()
    {
        rb.linearVelocity = Vector2.zero; // Dừng chuyển động

        transform.rotation = Quaternion.Euler(0f, 0f, Time.time * 50f);

        Debug.Log("[Idle] Nhân vật đang ở trạng thái nghỉ");
    }
}
