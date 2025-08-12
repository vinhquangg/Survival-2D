using UnityEngine;

public class TargetingBehavior : MonoBehaviour, ITargetBehavior
{
    [Header("Targeting Settings")]
    public Transform player;
    public LayerMask playerLayer;
    public float detectionRange = 15f;

    [HideInInspector] public bool isPlayerDetected = false;
    [HideInInspector] public float distanceToPlayer = 0f;

    //Lấy thông tin người chơi
    [HideInInspector] public float playerSpeed = 0f;
    [HideInInspector] public float playerHealth = 0f;
    [HideInInspector] public bool playerIsAttacking = false;

    private PlayerController playerScript;

    //Interface ItargetBehavior
    public Transform Target => player;
    public float DetectionRange => detectionRange;
    public bool IsPlayerDetected => isPlayerDetected;
    public void AcquireTarget(Transform target)
    {
        player = target;
        playerScript = player.GetComponent<PlayerController>();
    }
    public bool CheckPlayerVisible()
    {
        if((transform.position - player.position).sqrMagnitude > detectionRange * detectionRange)
        {
            return false;
        }
        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);
        return hit.collider != null && hit.collider.transform == player; ;
    }
    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerScript = player.GetComponent<PlayerController>();
        }
        InvokeRepeating(nameof(UpdateTargetInfo), 0f, 0.2f);
    }
    public void UpdateTargetInfo()
    {
        if (player == null || playerScript == null)
        {
            return;
        }
        distanceToPlayer = (transform.position - player.position).magnitude;
        isPlayerDetected = CheckPlayerVisible();
        if (isPlayerDetected)
        {
            playerSpeed = playerScript.speed;
            playerHealth = playerScript.health;
            playerIsAttacking = playerScript.isAttacking;
        }
        else
        {
            playerSpeed = 0f;
            playerHealth = 0f;
            playerIsAttacking = false;
        }
    }
}
