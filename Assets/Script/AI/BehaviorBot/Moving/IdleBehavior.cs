using UnityEngine;

public class IdleBehavior : MonoBehaviour
{
    public TargetingBehavior targeting; // Tham chiếu trực tiếp TargetingBehavior
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("[Awake] Rigidbody 2D đã được gán: " + (rb != null));
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckIdle), 0f, 0.2f);
    }

    private void CheckIdle()
    {
        if(targeting == null || rb == null)
        {
            Debug.Log("[Update] targetingBehavior hoặc Rigidbody2D chưa được gán!");
            return;
        }

        if (!targeting.IsPlayerDetected)
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
