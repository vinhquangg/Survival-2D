using UnityEngine;

public interface ITargetBehavior
{
    Transform Target { get; }
    float DetectionRange { get; }
    bool IsPlayerDetected { get; }

    void AcquireTarget(Transform target);
    void UpdateTargetInfo();
    bool CheckPlayerVisible();
}
