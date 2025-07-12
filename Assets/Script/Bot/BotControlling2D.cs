using UnityEngine;

public class BotControlling2D : MonoBehaviour
{
    [Header("Bot Settings")]
    public float speed = 2f;
    public int maxHealth = 1;
    private int currentHealth;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void Update()
    {
        if(player != null)
        {
            Vector2 targetPos = new Vector2(player.position.x, player.position.y);
            Vector2 direction = (targetPos - rb.position).normalized;
            movement = direction;
        }
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * movement));
    }

    public void ResetBot()
    {
        movement = Vector2.zero;
        
        if(rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = transform.position; // Reset position
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            TakeDamager(1);
            // Handle collision with player
            Debug.Log("Bot collided with player!");
        }
    }

    public void TakeDamager(int damage)
    {
        damage = 100;
        currentHealth -= damage;
        Debug.Log($"Bot took {damage} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        Debug.Log("Bot died!");
        PoolingManager.Instance.RecycleBot(transform.parent.gameObject);
    }
}
