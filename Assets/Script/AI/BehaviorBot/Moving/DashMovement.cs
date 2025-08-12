using System.Collections;
using UnityEngine;

/// <summary>
/// Dash Movement tối ưu cho game 2D mobile
/// Chỉ dash tới target với performance cao nhất
/// </summary>
public class DashMovement : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;           // Tốc độ dash
    [SerializeField] private float dashCooldown = 2f;         // Cooldown giữa các lần dash
    [SerializeField] private float dashDuration = 0.3f;       // Thời gian dash (ngắn cho mobile)
    [SerializeField] private float dashDistance = 8f;         // Khoảng cách tối đa để dash
    [SerializeField] private float minDashDistance = 2f;      // Khoảng cách tối thiểu để dash
    
    [Header("Mobile Optimization")]
    [SerializeField] private bool useFixedUpdate = true;      // Sử dụng FixedUpdate cho physics
    [SerializeField] private int maxDashAttempts = 3;         // Số lần thử dash tối đa
    [SerializeField] private float dashCheckInterval = 0.1f;  // Interval kiểm tra dash
    
    [Header("Visual Effects")]
    [SerializeField] private bool enableTrail = true;         // Bật trail effect
    [SerializeField] private GameObject trailPrefab;          // Trail prefab
    [SerializeField] private float trailDuration = 0.2f;      // Thời gian trail
    
    // Private variables - tối ưu memory allocation
    private Rigidbody2D rb;
    private ITargetBehavior targetBehavior;
    private Transform currentTarget;
    private Vector2 targetPosition;
    private float nextDashTime;
    private bool isDashing;
    private Coroutine dashCoroutine;
    private GameObject activeTrail;
    private float dashDistanceSquared;
    private float minDashDistanceSquared;
    private float lastDashCheckTime;
    private int dashAttempts;
    
    // Cached values để tránh allocation
    private Vector2 dashDirection;
    private Vector2 currentPosition;
    private float distanceSquared;

    private void Awake()
    {
        // Cache components
        rb = GetComponent<Rigidbody2D>();
        targetBehavior = GetComponent<ITargetBehavior>();
        
        // Pre-calculate squared distances
        dashDistanceSquared = dashDistance * dashDistance;
        minDashDistanceSquared = minDashDistance * minDashDistance;
        
        // Validate components
        if (rb == null)
        {
            Debug.LogError("[DashMovement] Rigidbody2D not found!");
            enabled = false;
        }
        
        if (targetBehavior == null)
        {
            Debug.LogError("[DashMovement] ITargetBehavior not found!");
            enabled = false;
        }
    }

    private void Start()
    {
        // Initialize timing
        nextDashTime = Time.time;
        lastDashCheckTime = Time.time;
    }

    private void Update()
    {
        if (!useFixedUpdate && !isDashing)
        {
            CheckAndExecuteDash();
        }
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate && !isDashing)
        {
            CheckAndExecuteDash();
        }
    }

    /// <summary>
    /// Kiểm tra và thực hiện dash - tối ưu cho mobile
    /// </summary>
    private void CheckAndExecuteDash()
    {
        // Throttle dash checks để tiết kiệm CPU
        if (Time.time - lastDashCheckTime < dashCheckInterval)
            return;
            
        lastDashCheckTime = Time.time;
        
        // Kiểm tra cooldown
        if (Time.time < nextDashTime)
            return;
            
        // Lấy target
        currentTarget = targetBehavior?.Target;
        if (currentTarget == null)
            return;
            
        // Cache position để tránh allocation
        currentPosition = transform.position;
        targetPosition = currentTarget.position;
        
        // Tính khoảng cách bình phương (tối ưu)
        distanceSquared = (currentPosition - targetPosition).sqrMagnitude;
        
        // Kiểm tra điều kiện dash
        if (CanDash())
        {
            ExecuteDash();
        }
    }

    /// <summary>
    /// Kiểm tra có thể dash không
    /// </summary>
    private bool CanDash()
    {
        // Khoảng cách phù hợp
        if (distanceSquared < minDashDistanceSquared || distanceSquared > dashDistanceSquared)
            return false;
            
        // Không đang dash
        if (isDashing)
            return false;
            
        // Không vượt quá số lần thử
        if (dashAttempts >= maxDashAttempts)
            return false;
            
        return true;
    }

    /// <summary>
    /// Thực hiện dash
    /// </summary>
    private void ExecuteDash()
    {
        // Tính hướng dash
        dashDirection = (targetPosition - currentPosition).normalized;
        
        // Bắt đầu dash
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
        
        dashCoroutine = StartCoroutine(DashRoutine());
        
        // Set cooldown
        nextDashTime = Time.time + dashCooldown;
        dashAttempts++;
    }

    /// <summary>
    /// Coroutine dash tối ưu
    /// </summary>
    private IEnumerator DashRoutine()
    {
        isDashing = true;
        
        // Bắt đầu trail effect
        if (enableTrail && trailPrefab != null)
        {
            StartTrailEffect();
        }
        
        // Thực hiện dash
        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Kết thúc dash
        EndDash();
    }

    /// <summary>
    /// Kết thúc dash
    /// </summary>
    private void EndDash()
    {
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        dashCoroutine = null;
        
        // Dừng trail
        StopTrailEffect();
        
        // Reset attempts sau một khoảng thời gian
        StartCoroutine(ResetDashAttempts());
    }

    /// <summary>
    /// Reset số lần thử dash
    /// </summary>
    private IEnumerator ResetDashAttempts()
    {
        yield return new WaitForSeconds(dashCooldown * 0.5f);
        dashAttempts = 0;
    }

    /// <summary>
    /// Bắt đầu trail effect
    /// </summary>
    private void StartTrailEffect()
    {
        if (activeTrail != null)
        {
            Destroy(activeTrail);
        }
        
        activeTrail = Instantiate(trailPrefab, transform.position, transform.rotation);
        activeTrail.transform.SetParent(transform);
        
        // Tự động destroy trail
        Destroy(activeTrail, trailDuration);
    }

    /// <summary>
    /// Dừng trail effect
    /// </summary>
    private void StopTrailEffect()
    {
        if (activeTrail != null)
        {
            Destroy(activeTrail);
            activeTrail = null;
        }
    }

    /// <summary>
    /// Dừng dash khi va chạm
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            EndDash();
        }
    }

    /// <summary>
    /// Dừng dash khi trigger
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDashing)
        {
            EndDash();
        }
    }

    // ========== PUBLIC API ==========

    /// <summary>
    /// Kiểm tra đang dash
    /// </summary>
    public bool IsDashing => isDashing;

    /// <summary>
    /// Lấy cooldown còn lại
    /// </summary>
    public float GetCooldownRemaining => Mathf.Max(0, nextDashTime - Time.time);

    /// <summary>
    /// Force dash tới vị trí
    /// </summary>
    public void ForceDashToPosition(Vector2 position)
    {
        if (!isDashing)
        {
            targetPosition = position;
            dashDirection = (targetPosition - (Vector2)transform.position).normalized;
            
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            
            dashCoroutine = StartCoroutine(DashRoutine());
        }
    }

    /// <summary>
    /// Reset cooldown
    /// </summary>
    public void ResetCooldown()
    {
        nextDashTime = Time.time;
        dashAttempts = 0;
    }

    /// <summary>
    /// Thay đổi dash speed
    /// </summary>
    public void SetDashSpeed(float newSpeed)
    {
        dashSpeed = newSpeed;
    }

    /// <summary>
    /// Thay đổi dash cooldown
    /// </summary>
    public void SetDashCooldown(float newCooldown)
    {
        dashCooldown = newCooldown;
    }

    /// <summary>
    /// Thay đổi dash distance
    /// </summary>
    public void SetDashDistance(float newDistance)
    {
        dashDistance = newDistance;
        dashDistanceSquared = newDistance * newDistance;
    }

    private void OnDestroy()
    {
        // Cleanup
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
        
        if (activeTrail != null)
        {
            Destroy(activeTrail);
        }
    }
}