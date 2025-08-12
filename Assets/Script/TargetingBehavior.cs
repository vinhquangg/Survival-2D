using UnityEngine;

public class TargetingBehavior : MonoBehaviour, ITargetBehavior
{
    public Transform currentTarget;
    public float detectionRange = 15f;

    [HideInInspector] public bool isPlayerDetected = false;
    [HideInInspector] public float distanceToTarget = 0f;

    public Transform Target => currentTarget;
    public float DetectionRange => detectionRange;
    public bool IsPlayerDetected => isPlayerDetected;

    public void AcquireTarget(Transform target)
    {
        currentTarget = target;
    }

    public bool CheckPlayerVisible()
    {
        if (currentTarget == null) return false;
        return (transform.position - currentTarget.position).sqrMagnitude <= detectionRange * detectionRange;
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTargetInfo), 0f, 0.2f);
    }

    public void UpdateTargetInfo()
    {
        if (currentTarget == null)
        {
            isPlayerDetected = false;
            distanceToTarget = 0f;
            return;
        }

        distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        isPlayerDetected = CheckPlayerVisible();
    }
}
