using UnityEngine;

public class TargetingBehavior : MonoBehaviour
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

    private PlayerMovement playerScript;

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerScript = player.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if(player == null || playerScript == null)
        {
            return;
        }
        // Visible
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);
        isPlayerDetected = CheckPlayerVisible();
        Debug.Log("Is Player Detected: " + isPlayerDetected);
        //Take Data player
        //playerSpeed = playerScript.speed;
        //Debug.Log("Player Speed: " + playerSpeed);
        //playerHealth = playerScript.health;
        //Debug.Log("Player Health: " + playerHealth);
        //playerIsAttacking = playerScript.isAttacking;
        //Debug.Log("Player Is Attacking: " + playerIsAttacking);
    }

    private bool CheckPlayerVisible()
    {
        if (distanceToPlayer > detectionRange)
        {
            return false;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);

        return hit.collider != null && hit.collider.transform == player;
    }
}
